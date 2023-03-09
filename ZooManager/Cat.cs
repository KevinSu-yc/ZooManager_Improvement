using System;

namespace ZooManager
{
    /// <summary>
    /// An Animal's subclass called Cat.
    /// </summary>
    public class Cat : Animal
    {
        /// <summary>
        /// Initialize emoji, species, name, reaction time, preys, and predators of Cat
        /// </summary>
        /// <param name="name">A name to be assigned to the cat</param>
        public Cat(string name)
        {
            emoji = "🐱";
            species = "cat";
            this.name = name;
            reactionTime = new Random().Next(1, 6); // Cat's reaction time can be 1 to 5

            /* (Feature e)
             * Cat hunts Chick and Mouse, run away from Raptor and Alien.
             * Assigned the arrays of strings to preys and predators accordingly.
             * Are used in Hunt() and Flee()
             */
            preys = new string[2] { "chick", "mouse"};
            predators = new string[2] { "raptor", "alien" };
        }

        /// <summary>
        /// Calls Activate() in Animal first, then prints a message about being a cat on Console.
        /// In one move, flee from Raptor first if there is one around, then hunt if possible.
        /// </summary>
        /// <returns>Returns a message about the action made while calling this method.</returns>
        public override string Activate()
        {
            base.Activate();
            Console.WriteLine("I am a cat. Meow.");

            /* Flee() returns a message about if flee or not
             * (Feature e) Cat avoid first then hunt
             */
            string message = Flee(); 

            /* If the message is an empty string, it means the cat doesn't flee.
             * Check if it's possible to Hunt.
             */
            if (message == "")
            {
                message = Hunt(); // Hunt() returns a message about if hunt or not

                /* If the message is an empty string, it means the cat doesn't hunt.
                 * Assigned a string about staying at the same location to the message.
                 */
                if (message == "")
                {
                    message = $"[Stay] A {species} stays at {location.x},{location.y}";
                }
                return message;
            };

            // At this point, the message should be about how the cat flee
            return message;
        }

        /// <summary>
        /// (Feature e)
        /// Overrides Animal.Flee() with more advanced detection.
        /// While Cat flees, it detects whether there's a prey on its way.
        /// Create a message according to different condition.
        /// </summary>
        /// <returns>Returns a message about the action made while calling this method.</returns>
        public override string Flee()
        {
            string message = "";

            // Go through every predator and seek if there is one (in this case, only raptor)
            foreach (string predator in predators)
            {
                // If there is a predator in this direction, then it should retreat or attack if there's a prey in its way
                if (Seek(location.x, location.y, Direction.up, predator) > 0)
                {
                    // At this point, the cat must be taking an action related to Flee, set the beginning of the message here
                    message = "[Flee]";

                    // Go through every preys to check if there is one in the way to flee
                    foreach (string prey in preys)
                    {
                        // If there's a prey, flee and hunt at the same time
                        if (Seek(location.x, location.y, Direction.down, prey) > 0)
                        {
                            message = $"[Flee & Hunt] A {species} at {location.x},{location.y} run away from a {predator} to {location.x},{location.y} to eat a {prey}";
                            Attack(this, Direction.down);
                            break; // exit the loop to avoid moving more than 1 times
                        }

                        // If there's no prey and able to retreat
                        if (Retreat(this, Direction.down))
                        {
                            message = $"[Flee] A {species} at {location.x},{location.y - 1} run away from a {predator} to {location.x},{location.y}";
                            break; // exit the loop to avoid moving more than 1 times
                        }
                    }

                    // If the message remain "[Flee]" at this point, it means the cat can't run away from the predator
                    if (message == "[Flee]")
                    {
                        message = $"[Flee] A {species} at {location.x},{location.y} can't run away from a {predator}";
                    }

                    break; // exit the loop to avoid moving more than 1 times
                }
                else if (Seek(location.x, location.y, Direction.down, predator) > 0)
                {
                    message = "[Flee]";
                    foreach (string prey in preys)
                    {
                        if (Seek(location.x, location.y, Direction.up, prey) > 0)
                        {
                            message = $"[Flee & Hunt] A {species} at {location.x},{location.y} run away from a {predator} to {location.x},{location.y} to eat a {prey}";
                            Attack(this, Direction.up);
                            break;
                        }
                        
                        if (Retreat(this, Direction.up))
                        {
                            message = $"[Flee] A {species} at {location.x},{location.y + 1} run away from a {predator} to {location.x},{location.y}";
                            break;
                        }
                    }

                    if (message == "[Flee]")
                    {
                        message = $"[Flee] A {species} at {location.x},{location.y} can't run away from a {predator}";
                    }

                    break;
                }
                else if (Seek(location.x, location.y, Direction.left, predator) > 0)
                {
                    message = "[Flee]";
                    foreach (string prey in preys)
                    {
                        if (Seek(location.x, location.y, Direction.right, prey) > 0)
                        {
                            message = $"[Flee & Hunt] A {species} at {location.x},{location.y} run away from a {predator} to {location.x},{location.y} to eat a {prey}";
                            Attack(this, Direction.right);
                            break;
                        }
                        
                        if (Retreat(this, Direction.right))
                        {
                            message = $"[Flee] A {species} at {location.x - 1},{location.y} run away from a {predator} to {location.x},{location.y}";
                            break;
                        }
                    }

                    if (message == "[Flee]")
                    {
                        message = $"[Flee] A {species} at {location.x},{location.y} can't run away from a {predator}";
                    }

                    break;
                }
                else if (Seek(location.x, location.y, Direction.right, predator) > 0)
                {
                    message = "[Flee]";
                    foreach (string prey in preys)
                    {
                        if (Seek(location.x, location.y, Direction.left, prey) > 0)
                        {
                            message = $"[Flee & Hunt] A {species} at {location.x},{location.y} run away from a {predator} to {location.x},{location.y} to eat {prey}";
                            Attack(this, Direction.left);
                            break;
                        }

                        if (Retreat(this, Direction.left))
                        {
                            message = $"[Flee] A {species} at {location.x + 1},{location.y} run away from a {predator} to {location.x},{location.y}.";
                            break;
                        }
                    }

                    if (message == "[Flee]")
                    {
                        message = $"[Flee] A {species} at {location.x},{location.y} can't run away from a {predator}";
                    }

                    break;
                }
            }
            return message; // return empty string: doesn't run away from predator
        }
    }
}

