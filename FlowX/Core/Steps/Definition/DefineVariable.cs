using System;
using FlowX.Core.Steps.Value;

namespace FlowX.Core.Steps.Definition
{
    public class DefineVariable<T>: IStep
    {
        private Variable<T> _value;

        public DefineVariable(out Variable<T> name, Func<T> func)
        {
            _value = new Variable<T>(func);
            name = _value;
        }
        
        public void RunStep(IFlow flow)
        {
            _value.Refresh();
        }
    }
}