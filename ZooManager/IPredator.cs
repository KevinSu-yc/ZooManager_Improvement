
namespace ZooManager
{
    /// <summary>
    /// Interface that hunts preys
    /// </summary>
    public interface IPredator
    {
        string[] preys { get; }

        string Hunt();

        /* I make the target string parameter optional
         * since Alien hunts every thing and doesn't have to specify target when attacking
         */
        Occupant Attack(IPredator attacker, Direction d, string target = "");
    }
}
