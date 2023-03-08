using System;
using System.Collections.Generic;
using System.Linq;

namespace ZooManager
{    
    /// <summary>
     /// An Animal's subclass called Mouse.
     /// </summary>
    public class Mouse : Animal
    {
        private bool reproduced = false;

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

                /* If the Chick is mature, add message about getting ready to grow.
                 * Otherwise, show how many turns the chick is staying on the board.
                 */
                if (!reproduced)
                {
                    if (turnOnBoard >= 4)
                    {
                        message += " (Ready to reproduce)";
                    }
                    else
                    {
                        message += $" (Reproduce {turnOnBoard}/3)"; // Mouse takes 3 turns to get ready to reproduce
                    }
                }
            }
            return message;
        }

        /// <summary>
        /// (Feature q)
        /// 
        /// </summary>
        public int Reproduce()
        {
            if (reproduced || turnOnBoard < 4)
            {
                return -1;
            }

            List<int> randomizedDirections = new List<int> { 0, 1, 2, 3 };
            randomizedDirections = randomizedDirections.OrderBy(d => new Random().Next()).ToList();

            foreach (int d in randomizedDirections)
            {
                if (Seek(location.x, location.y, (Direction)d) == 0)
                {
                    switch (d)
                    {
                        case ((int)Direction.up):
                            Game.animalZones[location.y - 1][location.x].occupant = new Mouse("Squeaky");
                            break;
                        case ((int)Direction.down):
                            Game.animalZones[location.y + 1][location.x].occupant = new Mouse("Squeaky");
                            break;
                        case ((int)Direction.left):
                            Game.animalZones[location.y][location.x - 1].occupant = new Mouse("Squeaky");
                            break;
                        case ((int)Direction.right):
                            Game.animalZones[location.y][location.x + 1].occupant = new Mouse("Squeaky");
                            break;
                    }
                    reproduced = true;
                    return d;
                }
            }

            return 4;
        }
    }
}

