using System;
using FlowX.Core.Steps;

namespace FlowX.Core
{
    public class While
    {
        private readonly Func<bool> _condition;

        public While(Func<bool> condition)
        {
            _condition = condition;
        }

        public WhileDo Do(Action action)
        {
            return this.Do((Step) action);
        }
        
        public WhileDo Do(IStep step)
        {
            return new WhileDo(this, step);
        }

        public class WhileDo : IStep
        {
            private readonly While _while;
            private readonly IStep _do;

            public WhileDo(While @while, IStep @do)
            {
                _while = @while;
                _do = @do;
            }

            public void RunStep(IFlow flow)
            {
                while (_while._condition())
                {
                    _do.Run(flow);
                }
            }
        }
    }
}