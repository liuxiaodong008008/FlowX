using System;

namespace FlowX.Core
{
    public interface IStepMiddleware
    {
        void Run(IFlow flow, IStep step, Action next);
    }
}