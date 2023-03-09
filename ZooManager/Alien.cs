using System;

namespace ZooManager
{
    /// <summary>
    /// Alien extends Occupant and implements IPredator
    /// Aliens hunt everything except aliens
    /// </summary>
    public class Alien : Occupant, IPredator
    {
        // Aliens hunt everything except aliens, to implement the IPredator interface, I add a placeholder string into the preys array
        public string[] preys { get; protected set; } = { "all occupants" };

        /// <summary>
        /// Initialize emoji, species, name, and reaction time of Alien
        /// </summary>
        /// <param name="name">A name to be assigned to the alien</param>
        public Alien(string name)
        {
            emoji = "👽";
            species = "alien";
            this.name = name;
            reactionTime = 5; // Alien's reaction time is always 5, aliens hunt every occupants so I decide not to let them react too fast
        }

        /// <summary>
        /// Print message about activating an alien on the Console,
        /// then set the status of the alien to be activated.
        /// Also add 1 turn to turn on board.
        /// </summary>
        /// <returns>Return the message about activating an alien</returns>
        public override string Activate()
        {
            Console.WriteLine($"Alien {name} at {location.x},{location.y} activated");
            isActivated = true;
            turnOnBoard += 1;

            string message = Hunt(); // Hunt() returns a message about if hunt or not

            /* If the message is an empty string, it means the alien doesn't hunt.
             * Assigned a string about staying at the same location to the message.
             */
            if (message == "")
            {
                message = $"[Stay] A {species} stays at {location.x},{location.y}";
            }

            return message;
        }

        /// <summary>
        /// Set the status of an alien to be not activated.
        /// </summary>
        public override void Deactivate()
        {
            isActivated = false; // (Feature o) At the end of a turn, switch the status back as not activated to get ready for a new turn
        }

        /// <summary>
        /// Attack if there is an occupant(not alien) around.
        /// </summary>
        /// <returns>Returns a message about the action made while calling this method.</returns>
        public string Hunt()
        {
            string message = "";
            Occupant target;

            target = Attack(this, Direction.up); // Attack Return the occupant that is being attacked
            // If target is not null, it means the alien attacks successfully, create and return the message about attacking
            if (target != null)
            {
                message += $"[Hunt] An {species} at {location.x},{location.y + 1} moves to {location.x},{location.y} to hunt a {target.species}";
                return message;
            }

            target = Attack(this, Direction.down);
            if (target != null)
            {
                message += $"[Hunt] An {species} at {location.x},{location.y - 1} moves to {location.x},{location.y} to hunt a {target.species}";
                return message;
            }

            target = Attack(this, Direction.left);
            if (target != null)
            {
                message += $"[Hunt] An {species} at {location.x + 1},{location.y} moves to {location.x},{location.y} to hunt a {target.species}";
                return message;
            }

            target = Attack(this, Direction.right);
            if (target != null)
            {
                message += $"[Hunt] An {species} at {location.x - 1},{location.y} moves to {location.x},{location.y} to hunt a {target.species}";
                return message;
            }           

            return message; // return empty string if doesn't find a prey around
        }

        /// <summary>
        /// In this version, I modify this attack method to call seek before attacking,
        /// so this method no longer need to be called after a seek method.
        /// If a prey is found at a given direction, replace the prey with acttacker
        /// and empty the zone where the attacker originally stayed
        /// </summary>
        /// <param name="attacker">The Alien that is attcking</param>
        /// <param name="d">The direction to attack</param>
        /// <returns>Returns the occupant that is being attacked</returns>
        public Occupant Attack(IPredator attacker, Direction d, string target = "")
        {
            Occupant prey = null;

            // While calling Alien.Attack, the attacker should be an alien. If not return null directly.
            if (attacker is Alien == false)
            {
                return prey;
            }

            // Create an alien attacker by casting the attaker into an alien. Use the alien attacker to call Alien's methods later
            Alien alienAttacker = (Alien) attacker;
            if (Seek(alienAttacker.location.x, alienAttacker.location.y, d) <= 0) 
            {
                // If the seeked Zone is out of range or contains an alien, don't attack and return null directly
                return prey;
            }

            int x = alienAttacker.location.x;
            int y = alienAttacker.location.y;

            // replace the target with acttacker according to the given direction
            switch (d)
            {
                case Direction.up:
                    prey = Game.animalZones[y - 1][x].occupant; // Keep track of the occupant that is being attacked to return it later
                    Game.animalZones[y - 1][x].occupant = alienAttacker;
                    break;
                case Direction.down:
                    prey = Game.animalZones[y + 1][x].occupant;
                    Game.animalZones[y + 1][x].occupant = alienAttacker;
                    break;
                case Direction.left:
                    prey = Game.animalZones[y][x - 1].occupant;
                    Game.animalZones[y][x - 1].occupant = alienAttacker;
                    break;
                case Direction.right:
                    prey = Game.animalZones[y][x + 1].occupant;
                    Game.animalZones[y][x + 1].occupant = alienAttacker;
                    break;
            }
            Game.animalZones[y][x].occupant = null; // empty the zone where the attacker originally stayed
            return prey;
        }

        /// <summary>
        /// Check if there's a specific occupant at a given direction 
        /// according to the location of the occupant that is seeking
        /// </summary>
        /// <param name="x">X coordinate of the occupant that is seeking</param>
        /// <param name="y">Y coordinate of the occupant that is seeking</param>
        /// <param name="d">Direction to look at</param>
        /// <param name="target">Optional, a occupant species to be seek</param>
        /// <returns>Returns -1 if seeked Zone is out of range or contains an alien, returns 0 if seeked Zone is empty, returns 1 if seeked Zone contains the target</returns>
        protected override int Seek(int x, int y, Direction d, string target = "")
        {
            switch (d)
            {
                case Direction.up:
                    y--;
                    break;
                case Direction.down:
                    y++;
                    break;
                case Direction.left:
                    x--;
                    break;
                case Direction.right:
                    x++;
                    break;
            }

            if (y < 0 || x < 0 || y > Game.numCellsY - 1 || x > Game.numCellsX - 1) return -1; // The seeked Zone is out of range
            if (Game.animalZones[y][x].occupant == null) return 0; // The seeked Zone is empty
            if (Game.animalZones[y][x].occupant is Alien != true) return 1; // The seeked Zone contains an occupant that is not an alien
            return -1; // at this point the seeked Zone should contains an alien
        }

    }
}
