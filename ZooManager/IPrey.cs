
namespace ZooManager
{
    public interface IPrey
    {
        string[] predators { get; }

        string Flee();
        bool Retreat(IPrey runner, Direction d);
    }
}
