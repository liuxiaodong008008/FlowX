using System;

namespace FlowX.Core.Extensions
{
    static class GeneralExtensions
    {
        public static T As<T>(this T self, out T name)
        {
            name = self;
            return self;
        }

        public static T Assert<T>(this T self, Func<T, bool> condition, string message = null)
        {
            if (!condition(self))
            {
                throw new Exception(message ?? "Asserion Error.");
            }

            return self;
        }
    }
}