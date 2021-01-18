using System;

namespace FlowX.Core
{
    public interface IFlow
    {
        Action<IFlow, IStep> StepMiddlewareAction { get; set; }
        
        Action<IFlow> FlowMiddlewareAction { get; set; }
            
        IStep Step { get; set; }

        internal sealed void RunWithStepMiddlewares()
        {
            Step.Run(this);
        }
        
        internal sealed void RunWithFlowMiddlewares()
        {
            FlowMiddlewareAction(this);
        }

        sealed void Run()
        {
            RunWithFlowMiddlewares();
        }

    }
}