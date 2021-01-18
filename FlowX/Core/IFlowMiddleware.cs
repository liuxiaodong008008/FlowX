using System;

namespace FlowX.Core
{
    public interface IFlowMiddleware
    {
        void Run(IFlow flow, Action next);
    }
}