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
         * A list of message(string) about important actions of each turn of Animal activations
         * Is used in Index.razor to print the messages onto the web page
         */
        public static List<string> updateMessages { get; private set; } = new List<string>();

        /* The Zones are public for Animals to move around them
         */
        public static List<List<Zone>> animalZones = new List<List<Zone>>();
        public static Zone holdingPen = new Zone(-1, -1, null);

        /// <summary>
        /// Create a board of Zones for Animals to take actions on
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
        /// Add a row or a column of Zones to the animal board
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
        /// to decide whether to activates the animals on board or not
        /// </summary>
        /// <param name="clickedZone">The clicked Zone</param>
        static public void ZoneClick(Zone clickedZone)
        {
            // Console.Write("Got animal ");
            // Console.WriteLine(clickedZone.emoji == "" ? "none" : clickedZone.emoji);
            // Console.Write("Held animal is ");
            // Console.WriteLine(holdingPen.emoji == "" ? "none" : holdingPen.emoji);

            if (clickedZone.occupant != null) clickedZone.occupant.ReportLocation(); // Report the location of the clicked zone if it contains an Animal

            // If the holding pen is empty and the clicked zone contain an Animal, take animal from zone to holding pen
            if (holdingPen.occupant == null && clickedZone.occupant != null)
            {
                NewTurn(); // Call NewTurn() to get ready for a new turn when the click activates Animals

                // take animal from zone to holding pen
                Console.WriteLine("Taking " + clickedZone.emoji);
                holdingPen.occupant = clickedZone.occupant;
                holdingPen.occupant.location.x = -1;
                holdingPen.occupant.location.y = -1;
                clickedZone.occupant = null;

                // Add a message about the action to the list
                updateMessages.Add($"[Hold] Hold a {holdingPen.occupant.species}");

                ActivateAnimals(); // Activates the Animals on the board
            }
            // If the holding pen contain an Animal and the clicked zone is empty, put animal in zone from holding pen
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
                
                ActivateAnimals();
            }
            // If both the holding pen and the clicked zone contain an Animal, don't activate animals
            else if (holdingPen.occupant != null && clickedZone.occupant != null)
            {
                // Don't call NewTurn() since the animals are not activated under this condition

                Console.WriteLine("Could not place animal.");
                updateMessages.Add("[Action Failed] Could not place animal");
                // Don't activate animals since user didn't get to do anything
            }

            /* (Feature o) In order to let every Animal to only take one action in a turn
             * I create a property for Animal to keep track of if it's activated
             * So at the end of each turn, have to call DeactivateAnimals() to reset the activate status for the a new turn
             */
            DeactivateAnimals();
        }

        /// <summary>
        /// When a new Animal is chosen to add to holding pen, check the holding pen
        /// to see it's possible to hold a new Animal and activate the animals on board
        /// </summary>
        /// <param name="animalType">The chosen type of Animal</param>
        static public void AddAnimalToHolding(string animalType)
        {
            // if already hold an animal
            if (holdingPen.occupant != null)
            {
                // Add a message about action failed
                updateMessages.Add("[Action Failed] Can't hold a new animal");
                return; // return directly so the animals on board are not activated and the game remain the same turn
            }

            // Able to add animal to holding pen, call NewTurn() to get ready
            NewTurn();

            // Add the animal to holdign pen accrording to the animal type
            if (animalType == "cat") holdingPen.occupant = new Cat("Fluffy");
            if (animalType == "mouse") holdingPen.occupant = new Mouse("Squeaky");
            if (animalType == "raptor") holdingPen.occupant = new Raptor("Rapty");
            if (animalType == "chick") holdingPen.occupant = new Chick("Chick");
            
            // Add a message about holding the animal
            string message = $"[Hold] Hold a {holdingPen.occupant.species}";
            updateMessages.Add(message);
            Console.WriteLine(message);

            ActivateAnimals(); // Activates animals for current turn
            DeactivateAnimals(); // Deactivate animals for next turn
        }

        /// <summary>
        /// Go through all the zones on board to activate all the animals in the order of their reaction time
        /// </summary>
        static public void ActivateAnimals()
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

            GrowChicks(); // After every animal acts, call GrowAnimal to grow the animals that are ready
        }

        /// <summary>
        /// Deactivate all the animals on board
        /// </summary>
        static public void DeactivateAnimals()
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
        /// (Feature n) Go through the zones on board and replace the chicks that are mature with raptors
        /// </summary>
        static public void GrowChicks()
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
                        if (zone.occupant.species == "chick" && ((Chick)zone.occupant).Mature())
                        {
                            zone.occupant = new Raptor("Rapty"); // Replace the occupant with raptor
                            updateMessages.Add($"[Grow] A chick at {x},{y} grows into a raptor after staying on board for over 3 turns"); // Add the message about growing the chick
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

