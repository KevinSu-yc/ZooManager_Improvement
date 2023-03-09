using System;

namespace ZooManager
{
    /// <summary>
    /// A class that holds Occupants or empty string
    /// </summary>
    public class Zone
    {
        // Zone doesn't contain an Occupant when it's created
        private Occupant _occupant = null;
        public Occupant occupant
        {
            get { return _occupant; }

            // When placing an Occupant into a Zone, also set the Occupant's location to be same as the Zone
            set
            {
                _occupant = value;
                if (_occupant != null) {
                    _occupant.location = location;
                }
            }
        }

        public Point location;

        /* Readonly. If the Zone doesn't contains an Occupant, it's emoji should be an empty string
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

        /* Readonly. If the Zone doesn't contains an Occupant, it's reaction time label should be an empty string
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

        /* Readonly. If the Zone doesn't contains an Occupant, it's number of turns on board should be an empty string
         * Is used to show turns on board on the web page
         */
        public string turnLabel // Turn On Board Label
        {
            get
            {
                if (occupant == null) return "";
                return occupant.turnOnBoard.ToString();
            }
        }

        /// <summary>
        /// Creates a Zone that can hold Occupant at a location
        /// </summary>
        /// <param name="x">x coordinate of the location of the Zone</param>
        /// <param name="y">y coordinate of the location of the Zone</param>
        /// <param name="occupant">The Occupant to be hold</param>
        public Zone(int x, int y, Occupant occupant)
        {
            location.x = x;
            location.y = y;

            this.occupant = occupant;
        }
    }
}
