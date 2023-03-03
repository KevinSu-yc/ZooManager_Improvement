using System;

namespace ZooManager
{
    /// <summary>
    /// (Feature c)
    /// Create a class called Chick and make it Bird's subclass.
    /// Chick flees from Cat and doesn't hunt.
    /// 
    /// (Feature n)
    /// Chick turns into Raptor after being on the board for over 3 turns
    /// </summary>
    public class Chick : Bird
    {
        /// <summary>
        /// Initialize emoji, species, name, reaction time, and predator of Chick
        /// </summary>
        /// <param name="name">A name to be assigned to the chick</param>
        public Chick(string name)
        {
            emoji = "🐥";
            species = "chick";
            this.name = name;
            reactionTime = new Random().Next(6, 11); // Chick's reaction time can be 6 to 10

            /* (Feature c)
             * Chick runs away from Cat
             * Assign an array of 1 string that represent Cat's species
             * Is used in Animal.Flee(), loop through predators array to detect "cat".
             * I make predators an array because there might be another class that has more than 1 predators.
             */
            predators = new string[1] { "cat" };
        }

        /// <summary>
        /// Calls Activate() in Bird first, then prints a message about being a chick on Console.
        /// Flees from predator if there is one, otherwise, stays at the same location.
        /// </summary>
        /// <returns>Returns a message about the action made while calling this method.</returns>
        public override string Activate()
        {
            base.Activate();
            Console.WriteLine("specifically, a chick.");

            string message = Flee(); // Flee() returns a message about if flee or not

            /* If the message is an empty string, it means the chick doesn't flee.
             * Assigned a string about staying at the same location to the message.
             */
            if (message == "")
            {
                message = $"[Stay] A {species} stays at {location.x},{location.y}";

                /* If the Chick is mature, add message about getting ready to grow.
                 * Otherwise, show how many turns the chick is staying on the board.
                 */
                if (Mature())
                {
                    message += " (Ready to grow)";
                }
                else
                {
                    message += $" (Grow {turnOnBoard}/3)"; // Chick takes 3 turns to get ready to grow
                }
            }
            return message;
        }

        /// <summary>
        /// (Feature n)
        /// Checks if the Chick is being on the board for over 3 turns.
        /// </summary>
        /// <returns>Returns true if over 3 turns, otherwise, returns false.</returns>
        public bool Mature()
        {
            return (turnOnBoard > 3);
        }
    }
}

