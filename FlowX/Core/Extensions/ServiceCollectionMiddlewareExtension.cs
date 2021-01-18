using Microsoft.Extensions.DependencyInjection;

namespace FlowX.Core.Extensions
{
    public static class ServiceCollectionMiddlewareExtension
    {
        public static ServiceCollection AddStepMiddleware<T>(this ServiceCollection collection)
            where T : class, IStepMiddleware
        {
            collection.AddSingleton<IStepMiddleware, T>();
            return collection;
        }
        
        public static ServiceCollection AddFlowMiddleware<T>(this ServiceCollection collection)
            where T : class, IFlowMiddleware
        {
            collection.AddSingleton<IFlowMiddleware, T>();
            return collection;
        }
    }
}