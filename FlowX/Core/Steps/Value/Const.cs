namespace FlowX.Core.Steps.Value
{
    public class Const<T> : IValue<T>
    {
        public T Value { get; }

        public Const(T value)
        {
            Value = value;
        }
    }
}