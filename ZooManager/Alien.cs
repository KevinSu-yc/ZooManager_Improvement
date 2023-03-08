using System;

namespace ZooManager
{
    public class Alien : Occupant, IPredator
    {
        public string[] preys { get; protected set; } = { "all occupants" };

        public Alien(string name)
        {
            emoji = "👽";
            species = "alien";
            this.name = name;
            reactionTime = 5; // Alien's reaction time is always 0
        }

        /// <summary>
        /// Print message about activating an animal on the Console,
        /// then set the status of the animal to be activated.
        /// Also add 1 turn to turn on board.
        /// </summary>
        /// <returns>Return the message about activating an animal</returns>
        public override string Activate()
        {
            Console.WriteLine($"Alien {name} at {location.x},{location.y} activated");
            isActivated = true;
            turnOnBoard += 1;

            string message = Hunt(); // Hunt() returns a message about if hunt or not

            /* If the message is an empty string, it means the raptor doesn't hunt.
             * Assigned a string about staying at the same location to the message.
             */
            if (message == "")
            {
                message = $"[Stay] A {species} stays at {location.x},{location.y}";
            }

            return message;
        }

        /// <summary>
        /// Set the status of an animal to be not activated.
        /// </summary>
        public override void Deactivate()
        {
            isActivated = false; // (Feature o) At the end of a turn, switch the status back as not activated to get ready for a new turn
        }

        /// <summary>
        /// Attack if there is an occupant around.
        /// </summary>
        /// <returns>Returns a message about the action made while calling this method.</returns>
        public string Hunt()
        {
            string message = "";
            
            // If there is a prey in the direction, attack and return message
            if (Seek(location.x, location.y, Direction.up) > 0)
            {
                string target = Game.animalZones[location.y - 1][location.x].occupant.species;
                message += $"[Hunt] An {species} at {location.x},{location.y} moves to ";
                Attack(this, Direction.up);
                message += $"{location.x},{location.y} to hunt a {target}";
                return message;
            }
                
            if (Seek(location.x, location.y, Direction.down) > 0)
            {
                string target = Game.animalZones[location.y + 1][location.x].occupant.species;
                message += $"[Hunt] An {species} at {location.x},{location.y} moves to ";
                Attack(this, Direction.down);
                message += $"{location.x},{location.y} to hunt a {target}";
                return message;
            }
                
            if (Seek(location.x, location.y, Direction.left) > 0)
            {
                string target = Game.animalZones[location.y][location.x - 1].occupant.species;
                message += $"[Hunt] An {species} at {location.x},{location.y} moves to ";
                Attack(this, Direction.left);
                message += $"{location.x},{location.y} to hunt a {target}";
                return message;
            }
                
            if (Seek(location.x, location.y, Direction.right) > 0)
            {
                string target = Game.animalZones[location.y][location.x + 1].occupant.species;
                message += $"[Hunt] An {species} at {location.x},{location.y} moves to ";
                Attack(this, Direction.right);
                message += $"{location.x},{location.y} to hunt a {target}";
                return message;
            }           

            return message; // return empty string if doesn't find a prey around
        }

        /// <summary>
        /// Should always be called after getting true from Seek().
        /// Assume that a prey is found at a given direction, replace the prey with acttacker
        /// and empty the zone where the attacker originally stayed
        /// </summary>
        /// <param name="attacker">The Alien that is attcking</param>
        /// <param name="d">The direction to attack</param>
        public void Attack(IPredator attacker, Direction d)
        {
            if (attacker is Alien == false)
            {
                return;
            }
            Alien alienAttacker = (Alien) attacker;
            
            int x = alienAttacker.location.x;
            int y = alienAttacker.location.y;

            // replace the target with acttacker according to the given direction
            switch (d)
            {
                case Direction.up:
                    Game.animalZones[y - 1][x].occupant = alienAttacker;
                    break;
                case Direction.down:
                    Game.animalZones[y + 1][x].occupant = alienAttacker;
                    break;
                case Direction.left:
                    Game.animalZones[y][x - 1].occupant = alienAttacker;
                    break;
                case Direction.right:
                    Game.animalZones[y][x + 1].occupant = alienAttacker;
                    break;
            }
            Game.animalZones[y][x].occupant = null; // empty the zone where the attacker originally stayed
        }

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
            return -1;
        }

    }
}
