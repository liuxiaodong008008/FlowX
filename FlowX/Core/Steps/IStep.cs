using System;

namespace FlowX.Core
{
    public interface IStep
    {
        sealed void Run(IFlow flow)
        {
            flow.StepMiddlewareAction(flow, this);
        }

        void RunStep(IFlow flow);
        
        public static IStep operator |(IStep left, IStep right)
        {
            if (left is Sequence sequence)
            {
                sequence.Add(right);
                return sequence;
            }
            else
            {
                return new Sequence(left, right);
            }
        }
        
        public static IStep operator |(IStep left, Action right)
        {
            return left | (Step)right;
        }
    }
}