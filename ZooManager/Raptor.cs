using System;

namespace ZooManager
{
    /// <summary>
    /// (Feature a & b)
    /// Create a class called Raptor and make it Bird's subclass.
    /// Raptor hunts Cat and Mouse and doesn't have predators.
    /// </summary>
    public class Raptor : Bird
    {
        /// <summary>
        /// Initialize emoji, species, name, reaction time, and preys of Raptor
        /// </summary>
        /// <param name="name">A name to be assigned to the raptor</param>
        public Raptor(string name)
        {
            emoji = "🦅";
            species = "raptor";
            this.name = name;
            reactionTime = 1; // Raptor's reaction time is always 1

            /* (Feature d)
             * Make Raptor hunts Cat and Mouse
             * Assign an array of 2 strings that represent Cat and Mouse's species
             * Is used in Animal.Hunt(), loop through preys array to detect "cat" and "mouse".
             */
            preys = new string[2] {"cat", "mouse"};
            predators = new string[1] { "alien" };
        }

        /// <summary>
        /// Calls Activate() in Bird first, then prints a message about being a raptor on Console.
        /// Hunts prey if there is one, otherwise, stays at the same location.
        /// </summary>
        /// <returns>Returns a message about the action made while calling this method.</returns>
        public override string Activate()
        {
            base.Activate();
            Console.WriteLine("specifically, a raptor.");

            /* Flee() returns a message about if flee or not
             * (Feature e) Cat avoid first then hunt
             */
            string message = Flee();

            /* If the message is an empty string, it means the raptor doesn't flee.
             * Check if it's possible to Hunt.
             */
            if (message == "")
            {
                message = Hunt(); // Hunt() returns a message about if hunt or not

                /* If the message is an empty string, it means the raptor doesn't hunt.
                 * Assigned a string about staying at the same location to the message.
                 */
                if (message == "")
                {
                    message = $"[Stay] A {species} stays at {location.x},{location.y}";
                }
                return message;
            };

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

