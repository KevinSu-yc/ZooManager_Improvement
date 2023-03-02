using System;
using System.Collections.Generic;

namespace ZooManager
{
    public static class Game
    {
        static public int numCellsX = 4;
        static public int numCellsY = 4;

        static private int maxCellsX = 10;
        static private int maxCellsY = 10;

        static public List<List<Zone>> animalZones = new List<List<Zone>>();
        static public Zone holdingPen = new Zone(-1, -1, null);

        static public List<string> updateMessages = new List<string>();
        static public int turn = 0;

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

        static public void AddZones(Direction d)
        {
            if (d == Direction.down || d == Direction.up)
            {
                if (numCellsY >= maxCellsY) return; // hit maximum height!
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
                if (numCellsX >= maxCellsX) return; // hit maximum width!
                for (var y = 0; y < numCellsY; y++)
                {
                    var rowList = animalZones[y];
                    // if (d == Direction.left) rowList.Insert(0, new Zone(null));
                    if (d == Direction.right) rowList.Add(new Zone(numCellsX, y, null));
                }
                numCellsX++;
            }
        }

        static public void ZoneClick(Zone clickedZone)
        {
            Console.Write("Got animal ");
            Console.WriteLine(clickedZone.emoji == "" ? "none" : clickedZone.emoji);
            Console.Write("Held animal is ");
            Console.WriteLine(holdingPen.emoji == "" ? "none" : holdingPen.emoji);
            if (clickedZone.occupant != null) clickedZone.occupant.ReportLocation();
            if (holdingPen.occupant == null && clickedZone.occupant != null)
            {
                NewTurn();

                // take animal from zone to holding pen
                Console.WriteLine("Taking " + clickedZone.emoji);
                holdingPen.occupant = clickedZone.occupant;
                holdingPen.occupant.location.x = -1;
                holdingPen.occupant.location.y = -1;
                clickedZone.occupant = null;
                updateMessages.Add($"[Hold] Hold a {holdingPen.occupant.species}");

                ActivateAnimals();
            }
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
            else if (holdingPen.occupant != null && clickedZone.occupant != null)
            {
                Console.WriteLine("Could not place animal.");
                updateMessages.Add("[Action Failed] Could not place animal");
                // Don't activate animals since user didn't get to do anything
            }
            DeactivateAnimals();
        }

        static public void AddAnimalToHolding(string animalType)
        {
            // if already hold an animal
            if (holdingPen.occupant != null)
            {
                updateMessages.Add("[Action Failed] Can't hold a new animal");
                return;
            }

            NewTurn();
            if (animalType == "cat") holdingPen.occupant = new Cat("Fluffy");
            if (animalType == "mouse") holdingPen.occupant = new Mouse("Squeaky");
            if (animalType == "raptor") holdingPen.occupant = new Raptor("Rapty");
            if (animalType == "chick") holdingPen.occupant = new Chick("Chick");
            // string message = $"Holding pen occupant at {holdingPen.occupant.location.x},{holdingPen.occupant.location.y}";
            string message = $"[Hold] Hold a {holdingPen.occupant.species}";
            Console.WriteLine(message);
            updateMessages.Add(message);
            ActivateAnimals();
            DeactivateAnimals();
        }

        static public void ActivateAnimals()
        {
            for (var r = 1; r < 11; r++) // reaction times from 1 to 10
            {
                for (var y = 0; y < numCellsY; y++)
                {
                    for (var x = 0; x < numCellsX; x++)
                    {
                        var zone = animalZones[y][x];
                        if (zone.occupant != null && zone.occupant.reactionTime == r && !zone.occupant.isActivated)
                        {
                            updateMessages.Add(zone.occupant.Activate());
                        }
                    }
                }
            }
            GrowAnimals();
        }

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

        static public void GrowAnimals()
        {
            for (var y = 0; y < numCellsY; y++)
            {
                for (var x = 0; x < numCellsX; x++)
                {
                    var zone = animalZones[y][x];
                    if (zone.occupant != null)
                    {
                        if (zone.occupant.species == "chick" && ((Chick)zone.occupant).Mature())
                        {
                            zone.occupant = new Raptor("Rapty");
                            updateMessages.Add($"[Grow] A chick at {x},{y} grows into a raptor after staying on board for over 3 turns");
                        }
                    }
                }
            }
        }

        static public void NewTurn()
        {
            updateMessages = new List<string>();
            turn += 1;
        }
    }
}

