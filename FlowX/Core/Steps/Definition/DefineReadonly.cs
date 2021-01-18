using System;
using FlowX.Core.Steps.Value;

namespace FlowX.Core.Steps.Definition
{
    public class DefineReadonly<T>: IStep
    {
        private Readonly<T> _value;

        public DefineReadonly(out Readonly<T> name, Func<T> func)
        {
            _value = new Readonly<T>(func);
            name = _value;
        }
        
        public void RunStep(IFlow flow)
        {
            _value.Refresh();
        }
    }
}