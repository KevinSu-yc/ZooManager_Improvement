using System;
namespace ZooManager
{
    public class Animal
    {
        public string emoji;
        public string species;
        public string name;
        public int reactionTime = 5; // default reaction time for animals (1 - 10)
        public bool isActivated = false;
        public int turnOnBoard = 0;
        
        public string[] preys;
        public string[] predators;

        public Point location;

        public void ReportLocation()
        {
            Console.WriteLine($"I am at {location.x},{location.y}");
        }

        virtual public string Activate()
        {
            string message = $"Animal {name} at {location.x},{location.y} activated";
            Console.WriteLine(message);
            Console.WriteLine(preys == null);
            isActivated = true;
            turnOnBoard += 1;
            return message;
        }

        public void Deactivate()
        {
            isActivated = false;
        }

        public string Hunt()
        {
            if (preys == null) // The animal doesn't have preys so it shouldn't hunt
            {
                return "";
            }

            string message = "";
            foreach (string prey in preys)
            {
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

        virtual public string Flee()
        {
            if (predators == null) // The animal doesn't have predators so it shouldn't need to flee
            {
                return "";
            }

            string message = "";
            foreach (string predator in predators)
            {
                if (Seek(location.x, location.y, Direction.up, predator))
                {
                    message += $"[Flee] A {species} at {location.x},{location.y} run away from ";
                    if (Retreat(this, Direction.down))
                    {
                        message += $"a {predator} to {location.x},{location.y}";
                        return message;
                    }
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

        static public bool Seek(int x, int y, Direction d, string target)
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

        /* This method currently assumes that the attacker has determined there is prey
         * in the target direction. In addition to bug-proofing our program, can you think
         * of creative ways that NOT just assuming the attack is on the correct target (or
         * successful for that matter) could be used?
         */
        static protected void Attack(Animal attacker, Direction d)
        {
            Console.WriteLine($"{attacker.name} is attacking {d.ToString()}");
            int x = attacker.location.x;
            int y = attacker.location.y;

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
            Game.animalZones[y][x].occupant = null;
        }


        /* We can't make the same assumptions with this method that we do with Attack, since
         * the animal here runs AWAY from where they spotted their target (using the Seek method
         * to find a predator in this case). So, we need to figure out if the direction that the
         * retreating animal wants to move is valid. Is movement in that direction still on the board?
         * Is it just going to send them into another animal? With our cat & mouse setup, one is the
         * predator and the other is prey, but what happens when we have an animal who is both? The animal
         * would want to run away from their predators but towards their prey, right? Perhaps we can generalize
         * this code (and the Attack and Seek code) to help our animals strategize more...
         */
        static protected bool Retreat(Animal runner, Direction d)
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
