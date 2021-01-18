namespace FlowX.Core.Steps.Value
{
    public interface IValue<out T>
    {
        T Value { get; }
    }
}