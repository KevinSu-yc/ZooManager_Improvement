using System;

namespace ZooManager
{
    public class Raptor : Bird
    {
        public Raptor(string name)
        {
            emoji = "🦅";
            species = "raptor";
            this.name = name;
            reactionTime = 1; // reaction time 1
            
            preys = new string[2] {"cat", "mouse"};
        }

        public override string Activate()
        {
            base.Activate();
            Console.WriteLine("specifically, a raptor.");
            string message = Hunt();
            if (message == "")
            {
                message = $"[Stay] A {species} stays at {location.x},{location.y}";
            }
            return message;
        }

    }
}

