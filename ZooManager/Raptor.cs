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

    }
}

