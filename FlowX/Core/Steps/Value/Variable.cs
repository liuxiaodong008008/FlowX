using System;

namespace FlowX.Core.Steps.Value
{
    public class Variable<T>: IValue<T>
    {
        internal readonly Func<T> _func;
        
        public T Value { get; set; }

        public Variable(Func<T> func)
        {
            _func = func;
        }

        public void Refresh()
        {
            Value = _func();
        }
    }
}