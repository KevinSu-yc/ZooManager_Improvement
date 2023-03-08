using System;

namespace ZooManager
{
    /// <summary>
    /// A class that holds Animal or empty string
    /// </summary>
    public class Zone
    {
        // Zone doesn't contain a Animal when it's created
        private Occupant _occupant = null;
        public Occupant occupant
        {
            get { return _occupant; }
            
            // When placing a Animal into a Zone, also set the Animal's location to be same as the Zone
            set {
                _occupant = value;
                if (_occupant != null) {
                    _occupant.location = location;
                }
            }
        }

        public Point location;

        /* Readonly. If the Zone doesn't contains a Animal, it's emoji should be an empty string
         * Is used to show emoji on the web page
         */
        public string emoji
        {
            get
            {
                if (occupant == null) return "";
                return occupant.emoji;
            }
        }

        /* Readonly. If the Zone doesn't contains a Animal, it's reaction time label should be an empty string
         * Is used to show reaction time on the web page
         */
        public string rtLabel // Reaction Time Label
        {
            get
            {
                if (occupant == null) return "";
                return occupant.reactionTime.ToString();
            }
        }

        /// <summary>
        /// Creates a Zone that can hold Animal at a location
        /// </summary>
        /// <param name="x">x coordinate of the location of the Zone</param>
        /// <param name="y">y coordinate of the location of the Zone</param>
        /// <param name="animal">The Animal to be hold</param>
        public Zone(int x, int y, Animal animal)
        {
            location.x = x;
            location.y = y;

            occupant = animal;
        }
    }
}
