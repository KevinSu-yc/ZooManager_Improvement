using System;
using System.Collections.Generic;

namespace ZooManager
{
    /// <summary>
    /// A class that controls information of the Game
    /// </summary>
    public static class Game
    {
        /* Constant for maximum number of cells that can be created
         * These should remain the same and don't have to be accessible by other classes
         */
        private const int MAX_CELLS_X = 10;
        private const int MAX_CELLS_Y = 10;

        /* The information that can affect the game should only be controled by Game itself
         * so I use private setter for them.
         */
        public static int numCellsX { get; private set; } = 4;
        public static int numCellsY { get; private set; } = 4;
        public static int turn { get; private set; } = 0;
        
        /* (Feature s)
         * A list of message(string) about important actions of each turn of activations
         * Is used in Index.razor to print the messages onto the web page
         */
        public static List<string> updateMessages { get; private set; } = new List<string>();

        /* The Zones remain public for Occupants to move around them
         */
        public static List<List<Zone>> animalZones = new List<List<Zone>>();
        public static Zone holdingPen = new Zone(-1, -1, null);

        /// <summary>
        /// Create a board of Zones for Occupant to take actions on
        /// </summary>
        static public void SetUpGame()
        {
            for (var y = 0; y < numCellsY; y++)
            {
                List<Zone> rowList = new List<Zone>();
                // Note one-line variation of for loop below!
                for (var x = 0; x < numCellsX; x++) rowList.Add(new Zone(x, y, null));
                animalZones.Add(rowList);
            }
        }

        /// <summary>
        /// Add a row or a column of Zones to the board
        /// Currently only able to add the Zones to the right or the left
        /// </summary>
        /// <param name="d">Direction to add the Zones</param>
        static public void AddZones(Direction d)
        {
            if (d == Direction.down || d == Direction.up)
            {
                if (numCellsY >= MAX_CELLS_Y) return; // hit maximum height!
                List<Zone> rowList = new List<Zone>();
                for (var x = 0; x < numCellsX; x++)
                {
                    rowList.Add(new Zone(x, numCellsY, null));
                }
                numCellsY++;
                if (d == Direction.down) animalZones.Add(rowList);
                // if (d == Direction.up) animalZones.Insert(0, rowList);
            }
            else // must be left or right...
            {
                if (numCellsX >= MAX_CELLS_X) return; // hit maximum width!
                for (var y = 0; y < numCellsY; y++)
                {
                    var rowList = animalZones[y];
                    // if (d == Direction.left) rowList.Insert(0, new Zone(null));
                    if (d == Direction.right) rowList.Add(new Zone(numCellsX, y, null));
                }
                numCellsX++;
            }
        }

        /// <summary>
        /// Called when a Zone is clicked. Check the holding pen and the clicked zone
        /// to decide whether to activates the occupants on board or not
        /// </summary>
        /// <param name="clickedZone">The clicked Zone</param>
        static public void ZoneClick(Zone clickedZone)
        {
            // Console.Write("Got animal ");
            // Console.WriteLine(clickedZone.emoji == "" ? "none" : clickedZone.emoji);
            // Console.Write("Held animal is ");
            // Console.WriteLine(holdingPen.emoji == "" ? "none" : holdingPen.emoji);

            if (clickedZone.occupant != null) clickedZone.occupant.ReportLocation(); // Report the location of the clicked zone if it contains an Occupant

            // If the holding pen is empty and the clicked zone contain an Occupant, take occupant from zone to holding pen
            if (holdingPen.occupant == null && clickedZone.occupant != null)
            {
                NewTurn(); // Call NewTurn() to get ready for a new turn when the click activates Occupants

                // take occupant from zone to holding pen
                Console.WriteLine("Taking " + clickedZone.emoji);
                holdingPen.occupant = clickedZone.occupant;
                holdingPen.occupant.location.x = -1;
                holdingPen.occupant.location.y = -1;
                clickedZone.occupant = null;

                // Add a message about the action to the list
                updateMessages.Add($"[Hold] Hold a {holdingPen.occupant.species}");

                ActivateOccupants(); // Activates the Occupants on the board
            }
            // If the holding pen contain an Occupant and the clicked zone is empty, put Occupant in zone from holding pen
            else if (holdingPen.occupant != null && clickedZone.occupant == null)
            {
                NewTurn();

                // put animal in zone from holding pen
                updateMessages.Add($"[Release] Release a {holdingPen.occupant.species} from hold");
                Console.WriteLine("Placing " + holdingPen.emoji);
                clickedZone.occupant = holdingPen.occupant;
                clickedZone.occupant.location = clickedZone.location;
                holdingPen.occupant = null;
                Console.WriteLine("Empty spot now holds: " + clickedZone.emoji);
                
                ActivateOccupants();
            }
            // If both the holding pen and the clicked zone contain an Occupant, don't activate Occupants
            else if (holdingPen.occupant != null && clickedZone.occupant != null)
            {
                // Don't call NewTurn() since the occupants are not activated under this condition

                Console.WriteLine("Could not place animal.");
                updateMessages.Add("[Action Failed] Could not place animal");
                // Don't activate occupants since user didn't get to do anything
            }

            /* (Feature o) In order to let every Occupant to only take one action in a turn
             * I create a property for Occupant to keep track of if it's activated
             * So at the end of each turn, have to call DeactivateOccupants() to reset the activate status for the a new turn
             */
            DeactivateOccupants();
        }

        /// <summary>
        /// When a new occupant is chosen to add to holding pen, check the holding pen
        /// to see it's possible to hold a new Occupant and activate the occupants on board
        /// </summary>
        /// <param name="occupantType">The chosen type of Animal</param>
        static public void AddOccupantToHolding(string occupantType)
        {
            // if already hold an occupant
            if (holdingPen.occupant != null)
            {
                // Add a message about action failed
                updateMessages.Add("[Action Failed] Can't hold a new occupant");
                return; // return directly so the occupants on board are not activated and the game remain the same turn
            }

            // Able to add occupant to holding pen, call NewTurn() to get ready
            NewTurn();

            // Add the occupant to holding pen accrording to the occupant type
            if (occupantType == "cat") holdingPen.occupant = new Cat("Fluffy");
            if (occupantType == "mouse") holdingPen.occupant = new Mouse("Squeaky");
            if (occupantType == "raptor") holdingPen.occupant = new Raptor("Rapty");
            if (occupantType == "chick") holdingPen.occupant = new Chick("Chick");
            if (occupantType == "alien") holdingPen.occupant = new Alien("ET");

            // Add a message about holding the occupant
            string message = $"[Hold] Hold a {holdingPen.occupant.species}";
            updateMessages.Add(message);
            Console.WriteLine(message);

            ActivateOccupants(); // Activates occupants for current turn
            DeactivateOccupants(); // Deactivate occupants for next turn
        }

        /// <summary>
        /// Go through all the zones on board to activate all the occupants in the order of their reaction time
        /// After every ocupant is activated, 
        /// </summary>
        static public void ActivateOccupants()
        {
            for (var r = 1; r < 11; r++) // reaction times from 1 to 10
            {
                /* This nested loop go through the zones from left to right, from top to bottom,
                 * so if two animals have the same reaction time, the one at left or top activates sooner
                 */
                for (var y = 0; y < numCellsY; y++)
                {
                    for (var x = 0; x < numCellsX; x++)
                    {
                        var zone = animalZones[y][x];

                        /* (Feature o) In addtion to check if the zone contains an animal and it's time for the animal to take action,
                         * also check if the animal is activated in a turn so all animals only act once
                         */
                        if (zone.occupant != null && zone.occupant.reactionTime == r && !zone.occupant.isActivated)
                        {
                            updateMessages.Add(zone.occupant.Activate());// add the message returned from Activate() about how different animals are activated
                        }
                    }
                }
            }

            ActivateSpecialAnimals(); // After every animal acts, call GrowAnimal to grow the animals that are ready
        }

        /// <summary>
        /// Deactivate all the animals on board
        /// </summary>
        static public void DeactivateOccupants()
        {
            for (var y = 0; y < numCellsY; y++)
            {
                for (var x = 0; x < numCellsX; x++)
                {
                    var zone = animalZones[y][x];
                    if (zone.occupant != null)
                    {
                        zone.occupant.Deactivate();
                    }
                }
            }
        }

        /// <summary>
        /// After activating all the animals on board,
        /// Go through the zones on board again to activate the animals that have special features
        /// (Feature n) Replace the chicks that are mature with raptors
        /// (Feature q) Let mice to reproduce if they are ready
        /// </summary>
        static public void ActivateSpecialAnimals()
        {
            for (var y = 0; y < numCellsY; y++)
            {
                for (var x = 0; x < numCellsX; x++)
                {
                    var zone = animalZones[y][x];
                    if (zone.occupant != null)
                    {
                        /* && check the left statement first, here I first check if the occupant is chick,
                         * if true, cast the occupant to Chick and call it's Mature() method 
                         * to see if it has stayed on board for over 3 turns
                         */
                        if (zone.occupant is Chick && ((Chick)zone.occupant).Mature())
                        {
                            zone.occupant = new Raptor("Rapty"); // Replace the occupant with raptor
                            updateMessages.Add($"[Grow] A chick at {x},{y} grows into a raptor after staying on board for over 3 turns"); // Add the message about growing the chick
                        }

                        // If the occupant is Mouse, cast it to Mouse
                        if (zone.occupant is Mouse m)
                        {
                            // Create a message about reproducing a mouse in a specific location according to the direction or not able to reproduce
                            switch (m.Reproduce())
                            {
                                case (((int)Direction.up)):
                                    updateMessages.Add($"[Reproduce] A mouse at {x},{y} reproduces a mouse at {x},{y - 1}");
                                    break;
                                case (((int)Direction.down)):
                                    updateMessages.Add($"[Reproduce] A mouse at {x},{y} reproduces a mouse at {x},{y + 1}");
                                    break;
                                case (((int)Direction.left)):
                                    updateMessages.Add($"[Reproduce] A mouse at {x},{y} reproduces a mouse at {x - 1},{y}");
                                    break;
                                case (((int)Direction.right)):
                                    updateMessages.Add($"[Reproduce] A mouse at {x},{y} reproduces a mouse at {x + 1},{y}");
                                    break;
                                case 4:
                                    updateMessages.Add($"[Reproduce] A mouse at {x},{y} can't reproduces a mouse");
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resets the messages for a new turn and add 1 to turn for keeping track of how many turns the game goes through
        /// </summary>
        static public void NewTurn()
        {
            updateMessages = new List<string>();
            turn += 1;
        }
    }
}

