
# Zoo Manager Improvements

**Part A**

First, I make the Animal class to be in charge of its action by moving Seek(), Attack(), and Retreat() from the Game class to the Animal class. So the Game class only set up the game interface and detect users' interaction to activate animals accordingly.

Next, by adding preys array property and predators array property to the Animal class, I no longer have to manually type the target species for the Hunt() and Flee() methods in different animal subclasses. So I can move the Hunt() and Flee() methods to the Animal class for every subclass to share those methods. The Cat class overrides the Flee() method since it requires further detection while running away from a predator.

As for encapsulation, I make all the properties except the location of the Animal class to be read-only in classes that are not Animal or subclasses of Animal (use protected setters) so the information of the animals won't be affected directly in unrelated classes. In addition, in the Game class, I think the information that can affect the game process should only be controlled by the Game class itself, so I use private setters for them. I also make the maximum number of cells that can be created to be private constant since they should always remain the same and don't have to be accessible by other classes.


**Part B**
-   **(Feature a):** I created a new class **Raptor** that hunts **cats** and the reaction time of its instances is always 1.
-   **(Feature b):** I created a new class **Bird** that inherits the **Animal** class. Also, made **Raptor** a subclass of **Bird.**
-   **(Feature c):** I created another subclass of **Bird** called **Chick** that flees from cats.
-   **(Feature d):** On top of feature a, I modified the **Raptor** class so it also hunts cats and mice.
-   **(Feature e):** I modified the **Cat** class so it hunts mice and chicks and runs away from raptors, moreover, it chooses to flee first instead of hunt. I added more detailed detection in the Flee() method of the **Cat** class so it checks if there's prey in its way while fleeing. If yes, the **Cat** flees and hunts at the same time.
-   **(Feature m):** For the **Animal** class, I added a property called turnOnBoard to keep track of how many turns every animal is staying on the board. Every time the animals activate counts as one turn.
-   **(Feature n):** For the **Chick** class, I added a new **Mature** method. The **Mature** method returns true as a chick stays on the board for more than 3 turns. To replace a chick with a raptor on its fourth turn, I also create a **GrowChicks** method in the **Game** class that check if there are any mature chicks on the board.
-   **(Feature o):** The **ActivateAnimals** method in the **Game** class loop through the zones on the board from left to right, from top to bottom. So if an animal moves right or down, it might be activated more than once when the **ActivateAnimals** method is called. I added a bool property in the Animal class called **isActivated** that represents if an animal is activated in one method call. Then every time an animal calls its **Activate** method, switch its **isActivated** property to true. I then modified the **Game.ActivateAnimals** method to call the **Animal.Activate** method only when the animal's **isActivated** property is false. In addition, I created **Game.DeactivateAnimals** and **Animal.Deactivate** methods to reset the **isActivated** property at the end of a turn.
-   **(Feature s):** I added a "turn updates" area on the interface to show important actions updated in each turn. In order to keep track of the actions, I created a **UpdateMessages** List in the Game class to store all messages about animals' actions. I modified the **Activate, Hunt,** and **Flee** methods in the **Animal** class to return strings that represent what action is made while the methods are called. Also, keep track of holding and releasing animals from holding pen. For instance, the message might be "[Hunt] A raptor at 3,1 moves to 2,1 to eat a mouse" if a raptor hunts a mouse successfully. Finally in Index.razor, loop through the **UpdateMessages** List to show the updates in each turn on the interface.
