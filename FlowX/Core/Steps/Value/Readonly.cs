using System;

namespace FlowX.Core.Steps.Value
{
    public class Readonly<T>: IValue<T>
    {
        internal readonly Func<T> _func;
        
        public T Value { get; internal set; }

        public Readonly(Func<T> func)
        {
            _func = func;
        }
        
        internal void Refresh()
        {
            Value = _func();
        }
    }
}