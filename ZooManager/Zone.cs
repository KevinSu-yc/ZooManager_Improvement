using System;
namespace ZooManager
{
    public class Zone
    {
        private Animal _occupant = null;
        public Animal occupant
        {
            get { return _occupant; }
            set {
                _occupant = value;
                if (_occupant != null) {
                    _occupant.location = location;
                }
            }
        }

        public Point location;

        public string emoji
        {
            get
            {
                if (occupant == null) return "";
                return occupant.emoji;
            }
        }

        public string rtLabel // Reaction Time Label
        {
            get
            {
                if (occupant == null) return "";
                return occupant.reactionTime.ToString();
            }
        }

        public string turnLabel // Turn On Board
        {
            get
            {
                if (occupant == null) return "";
                return occupant.turnOnBoard.ToString();
            }
        }

        public Zone(int x, int y, Animal animal)
        {
            location.x = x;
            location.y = y;

            occupant = animal;
        }
    }
}
