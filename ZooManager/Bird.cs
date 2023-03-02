using System;

namespace ZooManager
{
    public class Bird : Animal
    {
        public override string Activate()
        {
            base.Activate();
            string message = "I am a bird......";
            Console.WriteLine(message);
            return message;
        }
    }
}

