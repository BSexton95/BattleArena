using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArena
{
    public enum Scene
    {
        STARTMENU,
        NAMECREATION,
        CHARACTERSELECTION,
        BATTLE,
        RESTARTMENU
    }

    public enum ItemType
    {
        DEFENSE,
        ATTACK,
        HEALTH,
        NONE
    }

    public struct Item
    {
        public string Name;
        public int Cost;
        public float StatBoost;
        public ItemType Type;
    }
    
    class Game
    {
        private bool _gameOver;
        private Scene _currentScene;
        private Player _player;
        private Entity[] _enemies;
        private int _currentEnemyIndex;
        private Entity _currentEnemy;
        private string _playerName;
        private Shop _shop;

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();

            while (!_gameOver)
            {
                Update();
            }

            End();
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            _gameOver = false;
            _currentScene = 0;
            InitializeEnemies();
            InitializeItems();
        }

        /// <summary>
        /// Initializes all the items that can be bought in the shop
        /// </summary>
        public void InitializeItems()
        {
            //Attack items
            Item dagger = new Item { Name = "Dagger - 20g", Cost = 20, StatBoost = 10, Type = ItemType.ATTACK };
            Item sword = new Item { Name = "Sword - 20g", Cost = 20, StatBoost = 10, Type = ItemType.ATTACK };
            Item heavySword = new Item { Name = "Heavy Sword - 50g", Cost = 50, StatBoost = 50, Type = ItemType.ATTACK  };

            //Defense items
            Item armor = new Item { Name = "Armor - 40g", Cost = 20, StatBoost = 20, Type = ItemType.DEFENSE };
            Item shield = new Item { Name = "Shield - 20g", Cost = 20, StatBoost = 15, Type = ItemType.DEFENSE };

            //Health item
            Item healingBoost = new Item { Name = "Healing Boost - 30g", Cost = 30, StatBoost = 50, Type = ItemType.HEALTH };

            //Option in the shop for player to go back to battle
            Item goBackToBattle = new Item { Name = "Go Back To Battle" };

            //Initialize shop array
            Item[] _inventory = new Item[] { heavySword, healingBoost, dagger, shield, sword, armor, goBackToBattle };
            _shop = new Shop(_inventory);
        }

        /// <summary>
        /// Function initializes all enemies and their starting values
        /// </summary>
        public void InitializeEnemies()
        {
            //reset the current enemy to the first enemy in the array
            _currentEnemyIndex = 0;

            //Enemy 1 stats
            Entity gremlin = new Entity("Gremlin", 15, 40, 5);

            //Enemy 2 stats
            Entity cyclops = new Entity("Cyclops", 15, 55, 10);

            //Enemy 2 stats
            Entity witch = new Entity("Witch", 20, 50, 10);

            //Enemies are put into an array
            _enemies = new Entity[] { gremlin, cyclops, witch };

            _currentEnemy = _enemies[_currentEnemyIndex];
        }

        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        public void Update()
        {
            DisplayCurrentScene();
            Console.Clear();
        }

        /// <summary>
        /// This function is called before the applications closes
        /// </summary>
        public void End()
        {
            Console.WriteLine("Goodbye");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Function saves the current enemy player is battling, the players stats and inventoyr, and the current enemys stats
        /// to a text file.
        /// </summary>
        public void Save()
        {
            //Create a new stream writer
            StreamWriter writer = new StreamWriter("SaveData.txt");

            //Save current enemy index
            writer.WriteLine(_currentEnemyIndex);

            //Save player and enemy stats
            _player.Save(writer);
            _currentEnemy.Save(writer);

            //Close writer when done saving
            writer.Close();
        }

        /// <summary>
        /// Function loads in everthing from players previous save file.
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            bool loadSuccessful = true;

            //If the file doesn't exist...
            if (!File.Exists("SaveData.txt"))
            {
                //...return false
                loadSuccessful = false;
            }

            //Create a new reader to read from the text file
            StreamReader reader = new StreamReader("SaveData.txt");

            //If the first line can't be converted into an integer...
            if (!int.TryParse(reader.ReadLine(), out _currentEnemyIndex))
            {
                //...return false
                loadSuccessful = false;
            }
            
            //Load player job
            string job = reader.ReadLine();


            int gold;

            //If the first line can't be converted into an integer...
            if(!int.TryParse(reader.ReadLine(), out gold))
            {
                //...return false
                loadSuccessful = false;
            }

            //Create a new instance of player and try to load player
            _player = new Player(gold);
            if (!_player.Load(reader))
            {
                loadSuccessful = false;
            }

            //Creat a new instance and try to load the enemy
            _currentEnemy = new Entity();
            if (!_currentEnemy.Load(reader))
            {
                loadSuccessful = false;
            }

            //Update the array to match the current enemy stats
            _enemies[_currentEnemyIndex] = _currentEnemy;

            //Close the reader once loading is finished
            reader.Close();

            return loadSuccessful;
        }

        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="options">The options the player can choose from</param>
        /// <returns></returns>
        public int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputReceived = -1;

            while (inputReceived == -1)
            {
                //Print options
                Console.WriteLine(description);

                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + options[i]);
                }
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If the player typed an int...
                if (int.TryParse(input, out inputReceived))
                {
                    //...decrement the input and check if it's within the bounds of the array
                    inputReceived--;
                    if (inputReceived < 0 || inputReceived >= options.Length)
                    {
                        //Set input received to be the default value
                        inputReceived = -1;
                        //Display error message
                        Console.WriteLine("Invalid Input");
                        Console.ReadKey(true);
                    }
                }
                //If the player didn't type an int
                else
                {
                    //Set input received to be the default value
                    inputReceived = -1;
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey(true);
                }

                Console.Clear();

            }
            return inputReceived;
        }

        /// <summary>
        /// Calls the appropriate function(s) based on the current scene index
        /// </summary>
        public void DisplayCurrentScene()
        {
            switch (_currentScene)
            {
                case Scene.STARTMENU:
                    DisplayStartMenu();
                    break;

                case Scene.NAMECREATION:
                    GetPlayerName();
                    break;

                case Scene.CHARACTERSELECTION:
                    CharacterSelection();
                    Console.ReadKey(true);
                    break;

                case Scene.BATTLE:
                    Battle();
                    break;

                case Scene.RESTARTMENU:
                    DisplayMainMenu();
                    break;
            }
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game
        /// </summary>
        public void DisplayMainMenu()
        {
            int choice = GetInput("Play Again?", "Yes", "No");

            //If player chooses to play again...
            if (choice == 0)
            {
                //...set scene back to 0, which asks for player name
                _currentScene = 0;
                InitializeItems();
                InitializeEnemies();
            }
            //If player chooses to quit the game...
            else if (choice == 1)
            {
                //...set gameOver to true end the game.
                _gameOver = true;
            }

        }

        /// <summary>
        /// Displays the start of game and askes player if they want to start new game or load a saved game.
        /// </summary>
        public void DisplayStartMenu()
        {

            int choice = GetInput("Welcome to Battle Arena!", "Start New Game", "Load Game");

            //If player chooses to start game...
            if (choice == 0)
            {
                choice = GetInput("Have you ever wished to be a great and powerful warrior?", "Yes", "No");

                //If player chooses yes...
                if (choice == 0)
                {
                    //...display introduction to player
                    Console.WriteLine("Of course you would. Who wouldn't wish to be a great and powerfull warrior?" +
                        "\n Well here is your chance to prove to you and everyone esle what a great warrior you are.");
                    Console.ReadKey(true);
                    Console.Clear();

                    Console.WriteLine("Welcome to the battle arena where you will fight to prove you are a true warrior." +
                        "\n you have three opponents to face to prove you have what it takes. If you need help you have the shop" +
                        "\n to help boost your defense and attack. You only have 100 gold to spend so spend wisely.");
                    Console.ReadKey(true);
                    Console.Clear();

                    Console.WriteLine("I wish you luck my friend. Fight well!!");
                    Console.ReadKey(true);
                    Console.Clear();

                    //...current scene is set to the very first scene, which is to create a name.
                    _currentScene = Scene.NAMECREATION;
                }
                //If player chooses no...
                else if (choice == 1)
                {
                    //...display text to player and set call the display main menu function to restart the game if they wish
                    Console.WriteLine("Well you are in the wrong place my friend. Have a nice and boring life.");
                    Console.ReadKey(true);
                    Console.Clear();

                    DisplayMainMenu();
                }

               
            }
            //Player wants to load game...
            else if (choice == 1)
            {
                //If load is successful...
                if (Load())
                {
                    //...Displays print that load was successful and sets scene to battle
                    Console.WriteLine("Load Successful!");
                    Console.ReadKey(true);
                    Console.Clear();
                    _currentScene = Scene.BATTLE;
                }
                //If load was not successful...
                else
                {
                    //...Displays print that load failed
                    Console.WriteLine("Load Failed!");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        public void GetPlayerName()
        {
            //Ask player for a name
            Console.WriteLine("Welcome to the Battle Arena! What is your fighters name?");
            Console.Write("> ");

            //Set player neme to what the player has entered
            _playerName = Console.ReadLine();
            Console.WriteLine("");
            Console.Clear();

            //Askes player if they are sure that want to keep the name they have choosen.
            int choice = GetInput("You have entered " + _playerName + ". Are you sure you want to keep this name?", "Yes", "No");

            //If player has choosen to keep their name...
            if (choice == 0)
            {
                //...continue on 
                _currentScene++;
            }

            //Welcomes player to the battle arena
            Console.WriteLine("Welcome " + _playerName + "!");
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int choice = GetInput("Please choose a class", "Titan", "Hunter");

            //If player chooses the class Wizard...
            if (choice == 0)
            {
                //...player stats for Wizard
                _player = new Player(_playerName, 80, 60, 30, "Titan", 100);

                //Updates current scene
                _currentScene++;
            }
            //If player chooses the class Knight
            else if (choice == 1)
            {
                //...player stats for Knight
                _player = new Player(_playerName, 100, 40, 30, "Hunter", 100);

                //Updates current scene
                _currentScene++;
            }
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        public void DisplayStats(Entity character)
        {

            Console.WriteLine("Name: " + character.Name);
            Console.WriteLine("Health: " + character.Health);
            Console.WriteLine("Attack Power: " + character.AttackPower);
            Console.WriteLine("Defense Power: " + character.DefensePower);
            Console.WriteLine("");

        }

        /// <summary>
        /// Function allows player to equip an item that is in their inventory.
        /// </summary>
        public void DisplayEquipItemMenu()
        {
            //Get item index
            int choice = GetInput("Select an item to equip.", _player.GetItemsInInventoryNames());

            //Equip item at given index
            if (!_player.TryEquipItem(choice))
                Console.WriteLine("You couldn't find that item in your bag.");
            
            
            //Print feedback
            Console.WriteLine("You equipped " + _player.CurrentItem.Name + "!");
        }

        /// <summary>
        /// Simulates one turn in the current battle
        /// </summary>
        public void Battle()
        {
            float damageDealt = 0;

            DisplayStats(_player);
            DisplayStats(_currentEnemy);

            int choice = GetInput("A " + _currentEnemy.Name + " approaches you. What do you do?", "Attack", "Equip Item", "Remove current item", "Open Shop", "Save");

            //If player chooses to attack the enemy...
            if (choice == 0)
            {
                //...player attacks and deals damage to enemy
                damageDealt = _player.Attack(_currentEnemy);
                Console.WriteLine("You dealt " + damageDealt + " damage!");

                damageDealt = _currentEnemy.Attack(_player);
                Console.WriteLine("The " + _currentEnemy.Name + " dealt " + damageDealt + "!");

                CheckBattleResults();
            }
            //If player chooses to equip an item from their inventory...
            else if (choice == 1)
            {
                //...display items in inventory to select from.
                DisplayEquipItemMenu();
                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            //If player removes their current item...
            else if (choice == 2)
            {
                //...Trys to remove item
                if (!_player.TryRemoveCurrentItem())
                {
                    Console.WriteLine("You don't have anything equipped.");
                }
                //...Diplays text that item was put away
                else
                {
                    Console.WriteLine("You placed the item in your bag.");
                }

                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            //If player wants to see the shop...
            else if (choice == 3)
            {
                //...shop items are displayed to player
                DisplayShopMenu();
            }
            //If player wants to save game...
            else if (choice == 4)
            {
                //...game is saved
                Save();
                Console.WriteLine("Saved Game");
                Console.ReadKey(true);
                Console.Clear();
                return;
            }

            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Function adds two extra options, save game and exit game, after displaying the shops items
        /// </summary>
        /// <returns>The new array that has save game and exit game options</returns>
        private string[] GetShopMenuOptions()
        {
            //Create new array
            string[] shopMenuOptions = new string[_shop.GetShopItemNames().Length];

            //Copy everthing from old array to the new array
            for (int i = 0; i < _shop.GetShopItemNames().Length; i++)
            {
                shopMenuOptions[i] = _shop.GetShopItemNames()[i];
            }

            //Returns the list of all options in the shop
            return shopMenuOptions;
        }

        /// <summary>
        /// Displays the amount of gold the player has and shows their inventory if player has bought anything.
        /// Displays the shop items player can buy.
        /// </summary>
        private void DisplayShopMenu()
        {
            
            Console.WriteLine("Your Gold: " + _player.Gold);
            Console.WriteLine("Your Inventory: ");
            Console.WriteLine("");

            //Create a new array
            string[] playerInventory = _player.GetItemsInInventoryNames();

            //Copy everyting from old array into the new array
            for (int i = 0; i < _player.GetItemsInInventoryNames().Length; i++)
            {
                //print out everything in the new array
                Console.WriteLine(playerInventory[i]);
            }

            //Asks player what they would like to purchase and displays all items
            int choice = GetInput("What would you like to purchase?", GetShopMenuOptions());

            string[] shopInventory = _shop.GetShopItemNames();

            //If player chooses to back out of shop...
            if (choice == 6)
            {
                //...display text that player is going back to the battle.
                Console.WriteLine("Back to Battle!!");
            }
            //If player buys something from shop...
            else if (_shop.Sell(_player, choice))
            {
                //...display text to player that the item was purchased.
                Console.WriteLine("You purchased the " + shopInventory[choice]);
            }
            else if (choice == shopInventory.Length)
            {
                return;
            }
        }


        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle.
        /// </summary>
        public void CheckBattleResults()
        {
            //If player health drops below 0...
            if (_player.Health <= 0)
            {
                //...display text that player has been defeated.
                Console.WriteLine("You were slain...");
                Console.ReadKey(true);
                Console.Clear();

                //Set current scene to start menu to ask if they would like to play again.
                _currentScene = Scene.STARTMENU;
            }
            //If enemy health drops below 0...
            else if (_currentEnemy.Health <= 0)
            {
                //...display text that the enemy has been defeated
                Console.WriteLine("You slayed the " + _currentEnemy.Name);
                Console.ReadKey(true);
                Console.Clear();

                //Incremend the current enemy index to go on to the next enemy.
                _currentEnemyIndex++;

                //If player has slain all the enemies...
                if (_currentEnemyIndex >= _enemies.Length)
                {
                    //...display text that the player has defeated all enemies.
                    Console.WriteLine("You've slain all the enemies! You are a true warrior.");
                    _currentScene = Scene.RESTARTMENU;
                    return;
                }

                //Set current enemy to the current enemy index
                _currentEnemy = _enemies[_currentEnemyIndex];
            }
        }
    }
}
