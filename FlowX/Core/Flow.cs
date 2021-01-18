using System;

namespace FlowX.Core
{
    public class Flow: IFlow
    {
        public Action<IFlow, IStep> StepMiddlewareAction { get; set; }
        public Action<IFlow> FlowMiddlewareAction { get; set; }
        public IStep Step { get; set; }
    }
    
    public class Flow<TData>: IFlow
    {
        public Action<IFlow, IStep> StepMiddlewareAction { get; set; }
        public Action<IFlow> FlowMiddlewareAction { get; set; }
        public IStep Step { get; set; }
        
        public TData Data { get; set; }
    }
}