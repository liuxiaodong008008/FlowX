using System;
using FlowX.Core.Steps;

namespace FlowX.Core
{
    public class Step: IStep
    {
        private readonly Action _action;
        
        public Step(Action action)
        {
            _action = action;
        }
        
        public static implicit operator Step(Action action)
        {
            return new Step(action);
        }

        public void RunStep(IFlow flow)
        {
            _action();
        }
    }
}