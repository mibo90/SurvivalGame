namespace Svelto.ECS.Example.Survive.Enemies
{
    public interface IDestroyComponent
    {
        DispatchOnChange<bool> destroyed { get; }
    }
}