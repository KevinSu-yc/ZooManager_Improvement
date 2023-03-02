using System;

namespace ZooManager
{
    public class Cat : Animal
    {
        public Cat(string name)
        {
            emoji = "🐱";
            species = "cat";
            this.name = name;
            reactionTime = new Random().Next(1, 6); // reaction time 1 (fast) to 5 (medium)

            preys = new string[2] { "chick", "mouse" };
            predators = new string[1] { "raptor" };
        }

        public override string Activate()
        {
            base.Activate();
            Console.WriteLine("I am a cat. Meow.");
            string message = Flee();
            if (message == "")
            {
                message = Hunt();
                if (message == "")
                {
                    message = $"[Stay] A {species} stays at {location.x},{location.y}";
                }
                return message;
            };
            return message;
        }

        /* Note that our cat is currently not very clever about its hunting.
         * It will always try to attack "up" and will only seek "down" if there
         * is no mouse above it. This does not affect the cat's effectiveness
         * very much, since the overall logic here is "look around for a mouse and
         * attack the first one you see." This logic might be less sound once the
         * cat also has a predator to avoid, since the cat may not want to run in
         * to a square that sets it up to be attacked!
         */
        //public string Hunt()
        //{
        //}
        public override string Flee()
        {
            string message = "";
            foreach (string predator in predators)
            {
                if (Seek(location.x, location.y, Direction.up, predator))
                {
                    message = "[Flee]";
                    foreach (string prey in preys)
                    {
                        if (Seek(location.x, location.y, Direction.down, prey))
                        {
                            message = $"[Flee & Hunt] A {species} at {location.x},{location.y} run away from a {predator} to {location.x},{location.y} to eat a {prey}";
                            Attack(this, Direction.down);
                        }
                        else if (Retreat(this, Direction.down))
                        {
                            message = $"[Flee] A {species} at {location.x},{location.y} run away from a {predator} to {location.x},{location.y}";
                        }
                    }

                    if (message == "[Flee]")
                    {
                        message = $"[Flee] A {species} at {location.x},{location.y} can't run away from a {predator}";
                    }

                    break;
                }
                else if (Seek(location.x, location.y, Direction.down, predator))
                {
                    message = "[Flee]";
                    foreach (string prey in preys)
                    {
                        if (Seek(location.x, location.y, Direction.up, prey))
                        {
                            message = $"[Flee & Hunt] A {species} at {location.x},{location.y} run away from a {predator} to {location.x},{location.y} to eat a {prey}";
                            Attack(this, Direction.up);
                        }
                        else if (Retreat(this, Direction.up))
                        {
                            message = $"[Flee] A {species} at {location.x},{location.y} run away from a {predator} to {location.x},{location.y}";
                        }

                    }

                    if (message == "[Flee]")
                    {
                        message = $"[Flee] A {species} at {location.x},{location.y} can't run away from a {predator}";
                    }

                    break;
                }
                else if (Seek(location.x, location.y, Direction.left, predator))
                {
                    message = "[Flee]";
                    foreach (string prey in preys)
                    {
                        if (Seek(location.x, location.y, Direction.right, prey))
                        {
                            message = $"[Flee & Hunt] A {species} at {location.x},{location.y} run away from a {predator} to {location.x},{location.y} to eat a {prey}";
                            Attack(this, Direction.right);
                        }
                        else if (Retreat(this, Direction.right))
                        {
                            message = $"[Flee] A {species} at {location.x},{location.y} run away from a {predator} to {location.x},{location.y}";
                        }
                    }

                    if (message == "[Flee]")
                    {
                        message = $"[Flee] A {species} at {location.x},{location.y} can't run away from a {predator}";
                    }

                    break;
                }
                else if (Seek(location.x, location.y, Direction.right, predator))
                {
                    message = "[Flee]";
                    foreach (string prey in preys)
                    {
                        if (Seek(location.x, location.y, Direction.left, prey))
                        {
                            message = $"[Flee & Hunt] A {species} at {location.x},{location.y} run away from a {predator} to {location.x},{location.y} to eat {prey}";
                            Attack(this, Direction.left);
                        }
                        else if (Retreat(this, Direction.left))
                        {
                            message = $"[Flee] A {species} at {location.x},{location.y} run away from a {predator} to {location.x},{location.y}.";
                        }
                    }

                    if (message == "[Flee]")
                    {
                        message = $"[Flee] A {species} at {location.x},{location.y} can't run away from a {predator}";
                    }

                    break;
                }
            }
            return message; // return empty string: doesn't run away from predator
        }
    }
}

