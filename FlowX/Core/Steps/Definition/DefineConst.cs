using FlowX.Core.Steps.Value;

namespace FlowX.Core.Steps.Definition
{
    public class DefineConst<T>: IStep
    {
        private readonly T _value;

        public DefineConst(out Const<T> name, T value)
        {
            _value = value;
            name = new Const<T>(value);
        }

        public void RunStep(IFlow flow)
        {
        }
    }
}