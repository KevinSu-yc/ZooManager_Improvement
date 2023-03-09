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
        // (Feature q) Keep track of whether if the mouse reproduced
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

            /* Mouse runs away from Cat, Raptor, and Alien
             * Assign an array of 3 string that represent Cat, Raptors, and Alien species
             * Is used in Animal.Flee(), loop through predators array to detect "cat", "raptor", and "alien"
             */
            predators = new string[3] { "cat", "raptor", "alien" };
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

                // If the Mouse hasn't reproduced yet
                if (!reproduced)
                {
                    /* If the number of turns on board is enough, show message about getting ready to reproduce
                     * Otherwise, show how many turn is away from reproduce
                     */
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
        /// When a Mouse stays on the board for over 3 turns, 
        /// randomly reproduce a new Mouse in a orthogonal adjacent Zone that is empty.
        /// A Mouse only reproduces once.
        /// </summary>
        /// <returns>Returns int that represent the direction to reproduce</returns>
        public int Reproduce()
        {
            // If the mouse has reproduced or the number of turns on board is not enough
            if (reproduced || turnOnBoard < 4)
            {
                return -1; // return -1 directly so won't reproduce
            }

            /* Create a list of int from 0 to 3 and use Linq OrderBy method to shuffle the int in the list.
             * Later use this list of int to call Seek method in a random order 
             */
            List<int> randomizedDirections = new List<int> { 0, 1, 2, 3 };
            randomizedDirections = randomizedDirections.OrderBy(d => new Random().Next()).ToList();

            // Use the list of int to call Seek method in a random order 
            foreach (int d in randomizedDirections)
            {
                if (Seek(location.x, location.y, (Direction)d) == 0) // Animal.Seek returns 0 when the seeked Zone is empty
                {
                    // If the seeked Zone is empty, put a new Mouse into that Zone
                    switch ((Direction)d)
                    {
                        case (Direction.up):
                            Game.animalZones[location.y - 1][location.x].occupant = new Mouse("Squeaky");
                            break;
                        case (Direction.down):
                            Game.animalZones[location.y + 1][location.x].occupant = new Mouse("Squeaky");
                            break;
                        case (Direction.left):
                            Game.animalZones[location.y][location.x - 1].occupant = new Mouse("Squeaky");
                            break;
                        case (Direction.right):
                            Game.animalZones[location.y][location.x + 1].occupant = new Mouse("Squeaky");
                            break;
                    }
                    reproduced = true;
                    return d; // returns the int that represent the seeking direction
                }
            }

            return 4; // returns 4 if there's no empty Zone to reproduce
        }
    }
}

