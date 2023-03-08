using System;

namespace ZooManager
{
     abstract public class Occupant 
    {
        public string emoji { get; protected set; }
        public string species { get; protected set; }
        public string name { get; protected set; }

        public Point location;

        public int reactionTime { get; protected set; } = 5; // default reaction time for animals is set to 5 (lower the faster)
        public bool isActivated { get; protected set; } = false; // every animal starts as not activated
        public int turnOnBoard { get; protected set; } = 0; // (Feature m) keep track of how many turns the animal is staying on board

        /// <summary>
        /// Activate the occupant
        /// </summary>
        /// <returns>Return the message about activating an occupant</returns>
        abstract public string Activate();

        /// <summary>
        /// Set the status of an occupant to be not activated.
        /// </summary>
        abstract public void Deactivate();

        /// <summary>
        /// Print self location on the Console
        /// </summary>
        public void ReportLocation()
        {
            Console.WriteLine($"I am at {location.x},{location.y}");
        }

        /// <summary>
        /// Check if there's a specific animal at a given direction 
        /// according to the location of the animal that is seeking
        /// </summary>
        /// <param name="x">X coordinate of the animal that is seeking</param>
        /// <param name="y">Y coordinate of the animal that is seeking</param>
        /// <param name="d">Direction to look at</param>
        /// <param name="target">a animal species to be seek</param>
        /// <returns>Returns -1 if seeked Zone is out of range or does not contain the target, returns 0 if seeked Zone is empty, returns 1 if seeked Zone contains the target</returns>
        protected virtual int Seek(int x, int y, Direction d, string target = "")
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
            if (Game.animalZones[y][x].occupant.species == target)
            {
                return 1;
            }
            return -1;
        }

    }
}
