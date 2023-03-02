using System;

namespace ZooManager
{
    public class Chick : Bird
    {
        public Chick(string name)
        {
            emoji = "🐥";
            species = "chick";
            this.name = name;
            reactionTime = new Random().Next(6, 11); // reaction time of 6 to 10

            predators = new string[1] { "cat" };
        }

        public override string Activate()
        {
            base.Activate();
            Console.WriteLine("specifically, a chick.");

            string message = Flee();
            if (message == "")
            {
                message = $"[Stay] A {species} stays at {location.x},{location.y}";
                if (Mature())
                {
                    message += " (Ready to grow)";
                }
                else
                {
                    message += $" (Grow {turnOnBoard}/3)";
                }
            }
            return message;
        }

        public bool Mature()
        {
            return (turnOnBoard > 3);
        }
    }
}

