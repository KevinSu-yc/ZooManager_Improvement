using System;
namespace ZooManager
{    
    /// <summary>
     /// An Animal's subclass called Mouse.
     /// </summary>
    public class Mouse : Animal
    {
        /// <summary>
        /// Initialize emoji, species, name, reaction time, predators of Mouse
        /// </summary>
        /// <param name="name">A name to be assigned to the Mouse</param>
        public Mouse(string name)
        {
            emoji = "🐭";
            species = "mouse";
            this.name = name;
            reactionTime = new Random().Next(1, 4); // Mouse's reaction time can be 1 to 5

            /* Mouse runs away from Cat and Raptor
             * Assign an array of 2 string that represent Cat and Raptors species
             * Is used in Animal.Flee(), loop through predators array to detect "cat" and "raptor"
             */
            predators = new string[2] { "cat", "raptor" };
        }

        /// <summary>
        /// Calls Activate() in Animal first, then prints a message about being a mouse on Console.
        /// Flees from predator if there is one, otherwise, stays at the same location.
        /// </summary>
        /// <returns>Returns a message about the action made while calling this method.</returns>
        public override string Activate()
        {
            base.Activate();
            Console.WriteLine("I am a mouse. Squeak.");

            string message = Flee(); // Flee() returns a message about if flee or not
            
            /* If the message is an empty string, it means the mouse doesn't flee.
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

