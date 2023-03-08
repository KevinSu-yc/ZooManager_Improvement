using System;

namespace ZooManager
{
    /// <summary>
    /// (Feature b)
    /// Create an Animal's subclass called Bird.
    /// Bird has two subclasses called Raptor and Chick
    /// </summary>
    public abstract class Bird : Animal
    {
        /// <summary>
        /// Calls Activate() in Animal first, then prints a message about being a bird on Console.
        /// </summary>
        /// <returns>Returns the message that's printed on Console.</returns>
        public override string Activate()
        {
            base.Activate();
            string message = "I am a bird......";
            Console.WriteLine(message);
            return message;
        }
    }
}

