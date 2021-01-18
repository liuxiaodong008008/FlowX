using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace FlowX.Core
{
    public abstract class FlowBuilder: StepBuilder
    {
        public abstract IStep BuildStep(IServiceProvider serviceProvider);

        public virtual IFlow CreateFlow(IServiceProvider serviceProvider)
        {
            return new Flow();
        }
        
        public IFlow Build(IServiceProvider serviceProvider)
        {
            IFlow flow = CreateFlow(serviceProvider);

            flow.StepMiddlewareAction = BuildStepMiddlewareAction(serviceProvider);
            flow.FlowMiddlewareAction = BuildFlowMiddlewareAction(serviceProvider);
            flow.Step = BuildStep(serviceProvider);
            
            return flow;
        }
        
        internal static Action<IFlow,IStep> BuildStepMiddlewareAction(IServiceProvider serviceProvider)
        {
            IEnumerable<IStepMiddleware> middlewares = serviceProvider?.GetServices<IStepMiddleware>() ?? new IStepMiddleware[]{};
            
            Action<IFlow, IStep> action = (flow, step) => step.RunStep(flow);
            
            foreach (var middleware in middlewares.Reverse())
            {
                var action2 = action;
                action = (flow,step) =>
                {
                    middleware.Run(flow, step, () => action2(flow, step));
                };
            }

            return action;
        }
        
        internal static Action<IFlow> BuildFlowMiddlewareAction(IServiceProvider serviceProvider)
        {
            IEnumerable<IFlowMiddleware> middlewares = serviceProvider?.GetServices<IFlowMiddleware>() ?? new IFlowMiddleware[]{};

            Action<IFlow> action = flow => flow.RunWithStepMiddlewares();
            
            foreach (var middleware in middlewares.Reverse())
            {
                var action2 = action;
                action = flow =>
                {
                    middleware.Run(flow, () => action2(flow));
                };
            }

            return action;
        }
    }
}