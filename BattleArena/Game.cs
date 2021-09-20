﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{

    public enum ItemType
    { 
        DEFENSE,
        ATTACK,
        NONE
    }

    public struct Item
    {
        public string Name;
        public float StatBoost;
        public ItemType Type;
    }

    class Game
    {
        private bool _gameOver;
        private int _currentScene;
        private Player _player;
        private Entity[] _enemies;
        private int _currentEnemyIndex;
        private Entity _currentEnemy;
        private string _playerName;
        private Item[] _wizardItems;
        private Item[] _knightItems;

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

            while(!_gameOver)
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
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 5, ItemType = 1 };
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 15, ItemType = 0 };

            //Knight items
            Item wand = new Item { Name = "Wand", StatBoost = 1025, ItemType = 1 };
            Item shoes = new Item { Name = "Shoes", StatBoost = 9000.05f, ItemType = 0 };

            //Initialize arrays
            _wizardItems = new Item[] { bigWand, bigShield };
            _knightItems = new Item[] { wand, shoes };
        }

        /// <summary>
        /// Function initializes all enemies and their starting values
        /// </summary>
        public void InitializeEnemies()
        {
            //reset the current enemy to the first enemy in the array
            _currentEnemyIndex = 0;

            //Enemy 1 stats
            Entity fraawg = new Entity("Fraawg", 42, 15, 57);

            //Enemy 2 stats
            Entity sassafrazz = new Entity("Sassafrazzz", 74, 17, 59);

            //Enemy 2 stats
            Entity wompus = new Entity("Wompus with Gun", 99, 55, 30);

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

                for(int i = 0; i < options.Length; i++)
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
                    if(inputReceived < 0 || inputReceived >= options.Length)
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
            switch(_currentScene)
            {
                case 0:
                    GetPlayerName();
                    break;

                case 1:
                    CharacterSelection();
                    break;

                case 2:
                    Battle();
                    CheckBattleResults();
                    Console.ReadKey(true);
                    break;

                case 3:
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
            if(choice == 0)
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
                _player = new Player(_playerName, 50, 25, 50, _wizardItems);

                //Updates current scene
                _currentScene++;
            }
            //If player chooses the class Knight
            else if (choice == 1)
            {
                //...player stats for Knight
                _player = new Player(_playerName, 75, 15, 10, _knightItems);

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

            int choice = GetInput("A " + _currentEnemy.Name + " approaches you. What do you do?", "Attack", "Equip Item", "Remove current item");

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

            damageDealt = _currentEnemy.Attack(_player);
            Console.WriteLine("The " + _currentEnemy.Name + " dealt" + damageDealt);

            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            if(_player.Health <= 0)
            {
                Console.WriteLine("You were slain...");
                Console.ReadKey(true);
                Console.Clear();

                _currentScene = 3;
            }
            else if(_currentEnemy.Health <= 0)
            {
                Console.WriteLine("You slayed the " + _currentEnemy.Name);
                Console.ReadKey(true);
                Console.Clear();
                _currentEnemyIndex++;

                if (_currentEnemyIndex >= _enemies.Length)
                {
                    _currentScene = 3;
                    Console.WriteLine("You've slain all the enemies! You are a true warrior.");
                    return;
                }
                _currentEnemy = _enemies[_currentEnemyIndex];
            }
        }
    }
}
