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
        public float StatBoost;
        public ItemType Type;
    }

    public struct ShopItems
    {
        public string Name;
        public int Cost;
        public ItemType Type;
        public float StatBoost;
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
        private Item[] _wizardItems;
        private Item[] _knightItems;
        private Shop _shop;

        /// <summary>
        /// Function takes in an array of integers and adds on more integers to the array.
        /// </summary>
        /// <param name="arr">Array of any size</param>
        /// <param name="value">numbers to be added to the array</param>
        /// <returns>The new array that has all of the values in the old array and the second argument</returns>
        int[] AppendToArray(int[] arr, int value)
        {
            //Create a new array with one more slot than the old array
            int[] newArray = new int[arr.Length + 1];

            //Copy the values from the old array into the new array
            for (int i = 0; i < arr.Length; i++)
            {
                newArray[i] = arr[i];
            }

            newArray[newArray.Length - 1] = value;

            return newArray;
        }

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            int[] numbers = new int[] { 1, 2, 3, 4 };

            numbers = AppendToArray(numbers, 5);

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

        public void InitializeItems()
        {
            //Wizard items
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 5, Type = ItemType.ATTACK };
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 15, Type = ItemType.DEFENSE };

            //Knight items
            Item wand = new Item { Name = "Wand", StatBoost = 1025, Type = ItemType.ATTACK };
            Item shoes = new Item { Name = "Shoes", StatBoost = 9000.05f, Type = ItemType.DEFENSE };

            //Initialize items in shop
            ShopItems strongSword = new ShopItems { Name = "Strong Sword - 500", Cost = 500, Type = ItemType.ATTACK, StatBoost = 20 };
            ShopItems lightShield = new ShopItems { Name = "Light Shield - 20", Cost = 20, Type = ItemType.DEFENSE, StatBoost = 10 };
            ShopItems healthPotion = new ShopItems { Name = "Health Potion - 30", Cost = 30, Type = ItemType.HEALTH, StatBoost = 30 };

            //Initialize arrays
            _wizardItems = new Item[] { bigWand, bigShield };
            _knightItems = new Item[] { wand, shoes };

            //Initialize shop array
            ShopItems[] _inventory = new ShopItems[] { strongSword, lightShield, healthPotion };
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
            Entity fraawg = new Entity("Fraawg", 42, 15, 10);

            //Enemy 2 stats
            Entity sassafrazz = new Entity("Sassafrazzz", 74, 17, 10);

            //Enemy 2 stats
            Entity wompus = new Entity("Wompus with Gun", 99, 55, 10);

            //Enemies are put into an array
            _enemies = new Entity[] { fraawg, sassafrazz, wompus };

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

            //Assign items based on player job
            if (job == "Wizard")
            {
                _player = new Player(_wizardItems);
            }
            else if (job == "Knight")
            {
                _player = new Player(_knightItems);
            }
            else
            {
                loadSuccessful = false;
            }

            _player.Job = job;

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
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, params string[] options)
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
        void DisplayCurrentScene()
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
        void DisplayMainMenu()
        {
            int choice = GetInput("Play Again?", "Yes", "No");

            //If player chooses to play again...
            if (choice == 0)
            {
                //...set scene back to 0, which asks for player name
                _currentScene = 0;
                InitializeEnemies();
            }
            //If player chooses to quit the game...
            else if (choice == 1)
            {
                //...set gameOver to true end the game.
                _gameOver = true;
            }

        }

        public void DisplayStartMenu()
        {
            int choice = GetInput("Welcome to Battle Arena!", "Start New Game", "Load Game");

            if (choice == 0)
            {
                _currentScene = Scene.NAMECREATION;
            }
            else if (choice == 1)
            {
                if (Load())
                {
                    Console.WriteLine("Load Successful!");
                    Console.ReadKey(true);
                    Console.Clear();
                    _currentScene = Scene.BATTLE;
                }
                else
                {
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
        void GetPlayerName()
        {
            //Ask player for a name
            Console.WriteLine("Welcome to the Battle Arena! What is your fighters name?");
            Console.Write("> ");
            _playerName = Console.ReadLine();
            Console.WriteLine("");
            Console.Clear();

            //Askes player if they are sure that want to keep the name they have choosen.
            int choice = GetInput("You have entered " + _playerName + " . Are you sure you want to keep this name?", "Yes", "No");

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
            int choice = GetInput("Please choose a class", "Wizard", "Knight");

            //If player chooses the class Wizard...
            if (choice == 0)
            {
                //...player stats for Wizard
                _player = new Player(_playerName, 50, 25, 50, _wizardItems, "Wizard");

                //Updates current scene
                _currentScene++;
            }
            //If player chooses the class Knight
            else if (choice == 1)
            {
                //...player stats for Knight
                _player = new Player(_playerName, 75, 15, 20, _knightItems, "Knight");

                //Updates current scene
                _currentScene++;
            }
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Entity character)
        {

            Console.WriteLine("Name " + character.Name);
            Console.WriteLine("Health " + character.Health);
            Console.WriteLine("Attack Power " + character.AttackPower);
            Console.WriteLine("Defense Power " + character.DefensePower);
            Console.WriteLine("");

        }


        public void DisplayEquipItemMenu()
        {
            //Get item index
            int choice = GetInput("Select an item to equip.", _player.GetItemNames());

            //Equip item at given index
            if (!_player.TryEquipItem(choice))
                Console.WriteLine("You couldn't fine that item in your bag.");

            //Print feedback
            Console.WriteLine("You equipped " + _player.CurrentItem.Name + "!");
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
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

            }
            //Otherwise if player dicides to dodge the enemy...
            else if (choice == 1)
            {
                DisplayEquipItemMenu();
                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            else if (choice == 2)
            {
                if (!_player.TryRemoveCurrentItem())
                {
                    Console.WriteLine("You don't have anything equipped.");
                }
                else
                {
                    Console.WriteLine("You placed the item in your bag.");
                }

                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            else if (choice == 3)
            {
                DisplayShopMenu();
            }
            else if (choice == 4)
            {
                Save();
                Console.WriteLine("Saved Game");
                Console.ReadKey(true);
                Console.Clear();
                return;
            }

            damageDealt = _currentEnemy.Attack(_player);
            Console.WriteLine("The " + _currentEnemy.Name + " dealt " + damageDealt);

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
            string[] shopMenuOptions = new string[_shop.GetItemNames().Length];

            //Copy everthing from old array to the new array
            for (int i = 0; i < _shop.GetItemNames().Length; i++)
            {
                shopMenuOptions[i] = _shop.GetItemNames()[i];
            }

            //Create another array with two extra slots
            string[] addedOptions = new string[shopMenuOptions.Length + 1];

            //Again copy everthing from the previous array the a new array
            for (int i = 0; i < shopMenuOptions.Length; i++)
            {
                addedOptions[i] = shopMenuOptions[i];
            }

            //Add an exit game to the array
            addedOptions[shopMenuOptions.Length] = "Go Back to Battle";

            //Set the old array equal to the new array
            shopMenuOptions = addedOptions;

            //Returns the list of all options in the shop
            return shopMenuOptions;
        }

        /// <summary>
        /// Displays the players 
        /// </summary>
        private void DisplayShopMenu()
        {
            Console.WriteLine("Your Gold: " + _player.Gold);
            Console.WriteLine("Your Inventory: ");
            Console.WriteLine("");

            string[] playerInventory = _player.GetItemNames();

            for (int i = 0; i < _player.GetItemNames().Length; i++)
            {
                Console.WriteLine(playerInventory[i]);
            }

            //Asks player what they would like to purchase and displays all items
            int choice = GetInput("What would you like to purchase?", GetShopMenuOptions());

            //If player buys a sword...
            if (choice == 0)
            {
                //...shop sells player the sword
                Console.WriteLine("You have purchased a sword!");
                _shop.Sell(_player, 0);
            }
            //If player buys shield...
            else if (choice == 1)
            {
                //...shop sells player the sword
                Console.WriteLine("You have purchased a shield!");
                _shop.Sell(_player, 1);
            }
            //If player buys a health potion...
            else if (choice == 2)
            {
                //...shop sells the player the health potion
                Console.WriteLine("You have purchased a health potion!");
                _shop.Sell(_player, 2);
            }
            //If player saves game...
            else if (choice == 3)
            {
               
                return;
            }
        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            if (_player.Health <= 0)
            {
                Console.WriteLine("You were slain...");
                Console.ReadKey(true);
                Console.Clear();

                _currentScene = Scene.RESTARTMENU;
            }
            else if (_currentEnemy.Health <= 0)
            {
                Console.WriteLine("You slayed the " + _currentEnemy.Name);
                Console.ReadKey(true);
                Console.Clear();
                _currentEnemyIndex++;

                if (_currentEnemyIndex >= _enemies.Length)
                {
                    _currentScene = Scene.RESTARTMENU;
                    Console.WriteLine("You've slain all the enemies! You are a true warrior.");
                    return;
                }
                _currentEnemy = _enemies[_currentEnemyIndex];
            }

        }
    }
}
