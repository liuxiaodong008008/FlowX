using System;
using System.Collections.Generic;

namespace FlowX.Core
{
    public class If
    {
        internal readonly List<IfThen> _ifThens;

        internal readonly Func<bool> _condition;

        public If(List<IfThen> ifThens, Func<bool> condition)
        {
            _ifThens = ifThens;
            _condition = condition;
        }

        public If(Func<bool> condition)
        {
            _ifThens = new List<IfThen>();
            _condition = condition;
        }

        public bool RunIfThens(IFlow flow)
        {
            foreach (var ifThen in _ifThens)
            {
                if (ifThen._if._condition())
                {
                    ifThen._then.Run(flow);
                    return true;
                }
            }

            return false;
        }

        public IfThen Then(IStep then)
        {
            return new IfThen(_condition, then);
        }
        
        public IfThen Then(Action then)
        {
            return new IfThen(_condition, then);
        }

        public class IfThen : IStep
        {
            internal readonly If _if;
            internal readonly IStep _then;
            
            public IfThen(Func<bool> condition, IStep then): this(new If(condition), then)
            {
            }
            
            public IfThen(Func<bool> condition, Action then): this(condition, (Step) then)
            {
            }
            
            public IfThen(If @if, IStep then)
            {
                _if = @if;
                _then = (Step)then;
            }
            
            public If ElseIf(Func<bool> condition)
            {
                var ifThens = new List<IfThen>(_if._ifThens);
                ifThens.Add(this);
                
                return new If(ifThens, condition);
            }

            public IfThenElse Else(IStep @else)
            {
                return new IfThenElse(this, @else);
            }
            
            public IfThenElse Else(Action @else)
            {
                return new IfThenElse(this, (Step)@else);
            }

            public bool RunIfThens(IFlow flow)
            {
                if (_if.RunIfThens(flow))
                {
                    return true;
                }
                else if (_if._condition())
                {
                    _then.Run(flow);
                    return true;
                }
                else
                {
                    return false;    
                }
            }
            
            public void RunStep(IFlow flow)
            {
                RunIfThens(flow);
            }
        }

        public class IfThenElse : IStep
        {
            internal readonly IfThen _ifThen;
            internal readonly IStep _else;

            public IfThenElse(IfThen ifThen, IStep @else)
            {
                _ifThen = ifThen;
                _else = @else;
            }

            public void RunStep(IFlow flow)
            {
                if (!_ifThen.RunIfThens(flow))
                {
                    _else.Run(flow);
                }
            }
        }
    }
}