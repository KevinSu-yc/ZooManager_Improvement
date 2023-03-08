
namespace ZooManager
{
    public interface IPredator
    {
        string[] preys { get; }

        string Hunt();
        void Attack(IPredator attacker, Direction d);
    }
}
