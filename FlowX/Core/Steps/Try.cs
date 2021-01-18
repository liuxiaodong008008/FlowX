using System;
using System.Collections.Generic;
using FlowX.Core.Steps.Value;

namespace FlowX.Core.Steps
{
    public class Try: IStep
    {
        private readonly IStep _body;
        internal Exception Exception { get; set; }

        internal List<ICatchDo> CatchDos  = new List<ICatchDo>();

        private IStep _finallyBody = null;
        
        public Try(IStep body)
        {
            _body = body;
        }

        public CatchBlock<T> Catch<T>(out Readonly<T> exception) where T: Exception
        {
            exception = new Readonly<T>(()=>(T)this.Exception);
            return new CatchBlock<T>(this, exception);
        }

        public Try Finally(IStep finallyBody)
        {
            _finallyBody = finallyBody;
            return this;
        }
        
        public Try Finally(Action finallyBody)
        {
            return Finally((Step) finallyBody);
        }
        
        public class CatchBlock<T> where T : Exception
        {
            private readonly Try _try;
            private readonly Readonly<T> _exception;

            public CatchBlock(Try @try, Readonly<T> exception)
            {
                _try = @try;
                _exception = exception;
            }

            public Try Do(IStep catchBody)
            {
                _try.CatchDos.Add(new CatchDo<T>(_exception, catchBody));
                return _try;
            }
            
            public Try Do(Action catchBody)
            {
                return Do((Step) catchBody);
            }
        }
        
        public interface ICatchDo
        {
            bool Run(Exception exception, IFlow flow);
        }
        
        public class CatchDo<T>: ICatchDo where T: Exception
        {
            private readonly Readonly<T> _exception;
            private readonly IStep _catchBody;

            public CatchDo(Readonly<T> exception, IStep catchBody)
            {
                _exception = exception;
                _catchBody = catchBody;
            }
                
            public bool Run(Exception exception, IFlow flow)
            {
                if (exception is T)
                {
                    _exception.Refresh();
                    _catchBody.Run(flow);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void RunStep(IFlow flow)
        {
            try
            {
                _body.Run(flow);
            }
            catch (Exception exception)
            {
                this.Exception = exception;
                bool catched = false;
                foreach (var catchDo in CatchDos)
                {
                    if (catchDo.Run(exception, flow))
                    {
                        catched = true;
                        break;
                    }
                }
                
                _finallyBody?.Run(flow);

                if (!catched)
                {
                    throw;
                }
            }
        }
    }
}