
namespace ZooManager
{
    /// <summary>
    /// Interface that runs away from predators
    /// </summary>
    public interface IPrey
    {
        string[] predators { get; }

        string Flee();
        bool Retreat(IPrey runner, Direction d);
    }
}
