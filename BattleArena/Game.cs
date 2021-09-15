using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    class Game
    {
        private bool _gameOver;
        private int _currentScene;
        private Entity _player;
        private Entity[] _enemies;
        private int _currentEnemyIndex;
        private Entity _currentEnemy;
        private string _playerName;

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
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
        }

        /// <summary>
        /// Function initializes all enemies and their starting values
        /// </summary>
        public void InitializeEnemies()
        {
            //reset the current enemy to the first enemy in the array
            _currentEnemyIndex = 0;

            //Enemy 1 stats
            Entity fraawg = new Entity("Fraawg", 42590, 15, 573);

            //Enemy 2 stats
            Entity sassafrazz = new Entity("Sassafrazzz", 745, 1738, 5907);

            //Enemy 2 stats
            Entity wompus = new Entity("Wompus with Gun", 999, 55, 3000);

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
        int GetInput(string description, string option1, string option2)
        {
            string input = "";
            int inputReceived = 0;

            while (inputReceived != 1 && inputReceived != 2)
            {//Print options
                Console.WriteLine(description);
                Console.WriteLine("1. " + option1);
                Console.WriteLine("2. " + option2);
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If player selected the first option...
                if (input == "1" || input == option1)
                {
                    //Set input received to be the first option
                    inputReceived = 1;
                }
                //Otherwise if the player selected the second option...
                else if (input == "2" || input == option2)
                {
                    //Set input received to be the second option
                    inputReceived = 2;
                }
                //If neither are true...
                else
                {
                    //...display error message
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey();
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
            if(choice == 1)
            {
                //...set scene back to 0, which asks for player name
                _currentScene = 0;
                InitializeEnemies();
            }
            //If player chooses to quit the game...
            else
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
            if (choice == 1)
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
            if (choice == 1)
            {
                //...player stats for Wizard
                _player = new Entity(_playerName, 50, 25000000, 50000000);

                //Updates current scene
                _currentScene++;
            }
            //If player chooses the class Knight
            else if (choice == 2)
            {
                //...player stats for Knight
                _player = new Entity(_playerName, 75, 15, 10);

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

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            float damageDealt = 0;

            DisplayStats(_player);
            DisplayStats(_currentEnemy);

            int choice = GetInput("A " + _currentEnemy.Name + " approaches you. What do you do?", "Attack", "Dodge");

            //If player chooses to attack the enemy...
            if (choice == 1)
            {
                //...player attacks and deals damage to enemy
                damageDealt = _player.Attack(_currentEnemy);
                Console.WriteLine("You dealt " + damageDealt + " damage!");

            }
            //Otherwise if player dicides to dodge the enemy...
            else if (choice == 2)
            {
                Console.WriteLine("You dodged the trolls attack");
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
