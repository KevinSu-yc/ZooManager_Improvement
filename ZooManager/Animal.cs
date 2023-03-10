using System;

namespace ZooManager
{
    public class Animal
    {
        /* I make all the properties except location of Animal 
         * to be readonly in classes that are not Animal or subclasses of Animal
         * so the information of the animals won't be affected directly
         */
        public string emoji { get; protected set; }
        public string species { get; protected set; }
        public string name { get; protected set; }
        public int reactionTime { get; protected set; } = 5; // default reaction time for animals is set to 5 (lower the faster)
        public bool isActivated { get; protected set; } = false; // every animal starts as not activated
        public int turnOnBoard { get; protected set; } = 0; // (Feature m) keep track of how many turns the animal is staying on board
        
        // Arrays for preys and predators for different animals
        public string[] preys { get; protected set; }
        public string[] predators { get; protected set; }

        public Point location;

        /// <summary>
        /// Print self location on the Console
        /// </summary>
        public void ReportLocation()
        {
            Console.WriteLine($"I am at {location.x},{location.y}");
        }

        /// <summary>
        /// Print message about activating an animal on the Console,
        /// then set the status of the animal to be activated.
        /// Also add 1 turn to turn on board.
        /// </summary>
        /// <returns>Return the message about activating an animal</returns>
        virtual public string Activate()
        {
            string message = $"Animal {name} at {location.x},{location.y} activated";
            Console.WriteLine(message);
            isActivated = true; // (Feature o) Switch status to activated so the animal only act once in a turn
            turnOnBoard += 1; // (Feature m) An animal remains on board for 1 turn as it activates once
            return message;
        }

        /// <summary>
        /// Set the status of an animal to be not activated.
        /// </summary>
        public void Deactivate()
        {
            isActivated = false; // (Feature o) At the end of a turn, switch the status back as not activated to get ready for a new turn
        }


        /* After making the preys property for Animal class, I think the Hunt method 
         * can be shared to every subclass instead of creating creating the method separately as before.
         */

        /// <summary>
        /// Go through all the preys and attack if there is one around.
        /// </summary>
        /// <returns>Returns a message about the action made while calling this method.</returns>
        public string Hunt()
        {
            if (preys == null) // The animal doesn't have preys so it shouldn't hunt
            {
                return "";
            }

            string message = "";
            foreach (string prey in preys) // Go through all preys
            {
                // If there is a prey in the direction, attack and return message
                if (Seek(location.x, location.y, Direction.up, prey))
                {
                    message += $"[Hunt] A {species} at {location.x},{location.y} moves to ";
                    Attack(this, Direction.up);
                    message += $"{location.x},{location.y} to eat a {prey}";
                    return message;
                }
                
                if (Seek(location.x, location.y, Direction.down, prey))
                {
                    message += $"[Hunt] A {species} at {location.x},{location.y} moves to ";
                    Attack(this, Direction.down);
                    message += $"{location.x},{location.y} to eat a {prey}";
                    return message;
                }
                
                if (Seek(location.x, location.y, Direction.left, prey))
                {
                    message += $"[Hunt] A {species} at {location.x},{location.y} moves to ";
                    Attack(this, Direction.left);
                    message += $"{location.x},{location.y} to eat a {prey}";
                    return message;
                }
                
                if (Seek(location.x, location.y, Direction.right, prey))
                {
                    message += $"[Hunt] A {species} at {location.x},{location.y} moves to ";
                    Attack(this, Direction.right);
                    message += $"{location.x},{location.y} to eat a {prey}";
                    return message;
                }
            }

            return message; // return empty string if doesn't find a prey around
        }

        /* After making the preys property for Animal class, I think the Flee method 
         * can also be shared to every subclass instead of creating creating the method separately as before.
         * This method should be override if the subclass needs more detection like Cat
         */

        /// <summary>
        /// Go through all the predators and retreat if there is one around.
        /// </summary>
        /// <returns>Returns a message about the action made while calling this method.</returns>
        virtual public string Flee()
        {
            if (predators == null) // The animal doesn't have predators so it shouldn't need to flee
            {
                return "";
            }

            string message = "";
            foreach (string predator in predators) //Go through predators
            {
                // If there's a predator at the direction
                if (Seek(location.x, location.y, Direction.up, predator))
                {
                    message += $"[Flee] A {species} at {location.x},{location.y} run away from ";
                    
                    // Retreat if there's a space at the opposite direction and return message
                    if (Retreat(this, Direction.down))
                    {
                        message += $"a {predator} to {location.x},{location.y}";
                        return message;
                    }

                    // At this point, it mean the animal tend to flee but there's no space
                    message = $"[Flee] A {species} at {location.x},{location.y} can't run away from a {predator}";
                    return message;
                }

                if (Seek(location.x, location.y, Direction.down, predator))
                {
                    message += $"[Flee] A {species} at {location.x},{location.y} run away from ";
                    if (Retreat(this, Direction.up))
                    {
                        message += $"a {predator} to {location.x},{location.y}";
                        return message;
                    }

                    message = $"[Flee] A {species} at {location.x},{location.y} can't run away from a {predator}";
                    return message;
                }

                if (Seek(location.x, location.y, Direction.left, predator))
                {
                    message += $"[Flee] A {species} at {location.x},{location.y} run away from ";
                    if (Retreat(this, Direction.right))
                    {
                        message += $"a {predator} to {location.x},{location.y}";
                        return message;
                    }

                    message = $"[Flee] A {species} at {location.x},{location.y} can't run away from a {predator}";
                    return message;
                }

                if (Seek(location.x, location.y, Direction.right, predator))
                {
                    message += $"[Flee] A {species} at {location.x},{location.y} run away from ";
                    if (Retreat(this, Direction.left))
                    {
                        message += $"a {predator} to {location.x},{location.y}";
                        return message;
                    }

                    message = $"[Flee] A {species} at {location.x},{location.y} can't run away from a {predator}";
                    return message;
                }
            }

            return message; // return empty string if doesn't find a predator around
        }

        /// <summary>
        /// Check if there's a specific animal at a given direction 
        /// according to the location of the animal that is seeking
        /// </summary>
        /// <param name="x">X coordinate of the animal that is seeking</param>
        /// <param name="y">Y coordinate of the animal that is seeking</param>
        /// <param name="d">Direction to look at</param>
        /// <param name="target">a animal species to be seek</param>
        /// <returns>Returns true if there's a target at the given direction, otherwise, return false</returns>
        protected bool Seek(int x, int y, Direction d, string target)
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

            if (y < 0 || x < 0 || y > Game.numCellsY - 1 || x > Game.numCellsX - 1) return false; // The seeked Zone is out of range
            if (Game.animalZones[y][x].occupant == null) return false; // The seeked Zone is empty
            if (Game.animalZones[y][x].occupant.species == target)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Should always be called after getting true from Seek().
        /// Assume that a prey is found at a given direction, replace the prey with acttacker
        /// and empty the zone where the attacker originally stayed
        /// </summary>
        /// <param name="attacker">The Animal that is attcking</param>
        /// <param name="d">The direction to attack</param>
        protected void Attack(Animal attacker, Direction d)
        {
            Console.WriteLine($"{attacker.name} is attacking {d.ToString()}");
            int x = attacker.location.x;
            int y = attacker.location.y;

            // replace the target with acttacker according to the given direction
            switch (d)
            {
                case Direction.up:
                    Game.animalZones[y - 1][x].occupant = attacker;
                    break;
                case Direction.down:
                    Game.animalZones[y + 1][x].occupant = attacker;
                    break;
                case Direction.left:
                    Game.animalZones[y][x - 1].occupant = attacker;
                    break;
                case Direction.right:
                    Game.animalZones[y][x + 1].occupant = attacker;
                    break;
            }
            Game.animalZones[y][x].occupant = null; // empty the zone where the attacker originally stayed
        }

        /// <summary>
        /// Should always be called after getting true from Seek().
        /// Assume that a predator is found at a given direction, move the runner to the opposite direction if possible
        /// and empty the zone where the attacker originally stayed if run successfully
        /// </summary>
        /// <param name="runner">The animal that is running</param>
        /// <param name="d">The direction to run</param>
        /// <returns></returns>
        protected bool Retreat(Animal runner, Direction d)
        {
            Console.WriteLine($"{runner.name} is retreating {d.ToString()}");
            int x = runner.location.x;
            int y = runner.location.y;

            switch (d)
            {
                case Direction.up:
                    /* The logic below uses the "short circuit" property of Boolean &&.
                     * If we were to check our list using an out-of-range index, we would
                     * get an error, but since we first check if the direction that we're modifying is
                     * within the ranges of our lists, if that check is false, then the second half of
                     * the && is not evaluated, thus saving us from any exceptions being thrown.
                     */
                    if (y > 0 && Game.animalZones[y - 1][x].occupant == null)
                    {
                        Game.animalZones[y - 1][x].occupant = runner;
                        Game.animalZones[y][x].occupant = null;
                        return true; // retreat was successful
                    }
                    return false; // retreat was not successful

                /* Note that in these four cases, in our conditional logic we check
                 * for the animal having one square between itself and the edge that it is
                 * trying to run to. For example,in the above case, we check that y is greater
                 * than 0, even though 0 is a valid spot on the list. This is because when moving
                 * up, the animal would need to go from row 1 to row 0. Attempting to go from row 0
                 * to row -1 would cause a runtime error. This is a slightly different way of testing
                 * if 
                 */
                case Direction.down:
                    if (y < Game.numCellsY - 1 && Game.animalZones[y + 1][x].occupant == null)
                    {
                        Game.animalZones[y + 1][x].occupant = runner;
                        Game.animalZones[y][x].occupant = null;
                        return true;
                    }
                    return false;
                case Direction.left:
                    if (x > 0 && Game.animalZones[y][x - 1].occupant == null)
                    {
                        Game.animalZones[y][x - 1].occupant = runner;
                        Game.animalZones[y][x].occupant = null;
                        return true;
                    }
                    return false;
                case Direction.right:
                    if (x < Game.numCellsX - 1 && Game.animalZones[y][x + 1].occupant == null)
                    {
                        Game.animalZones[y][x + 1].occupant = runner;
                        Game.animalZones[y][x].occupant = null;
                        return true;
                    }
                    return false;
            }
            return false; // fallback
        }
    }
}
