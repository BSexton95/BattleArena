using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    //Test Commit

    /// <summary>
    /// Represents any entity that exists in game
    /// </summary>
    struct Character
    {
        public string name;
        public float health;
        public float attackPower;
        public float defensePower;
    }

    class Game
    {
        bool gameOver;
        int currentScene;
        Character player;
        Character[] enemies;
        private int currentEnemyIndex = 0;
        private Character currentEnemy;

        //Enemy 1
        Character troll;
        //Enemy 2
        Character aHead;
        //Enemy 3
        Character creepyMan;
        //Enemy 4
        Character jakeFromStateFarm;

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();

            while(!gameOver)
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
            gameOver = false;
            currentScene = 0;
            currentEnemyIndex = 0;

            //Enemy 1 stats
            troll.name = "Troll";
            troll.health = 10;
            troll.attackPower = 20;
            troll.defensePower = 5;

            //Enemy 2 stats
            aHead.name = "Head";
            aHead.health = 10;
            aHead.attackPower = 20;
            aHead.defensePower = 5;

            //Enemy 3 stats
            creepyMan.name = "Creepy Man";
            creepyMan.health = 10;
            creepyMan.attackPower = 20;
            creepyMan.defensePower = 5;

            //Enemy 4 stats
            jakeFromStateFarm.name = "Jake From State Farm";
            jakeFromStateFarm.health = 20;
            jakeFromStateFarm.attackPower = 20;
            jakeFromStateFarm.defensePower = 10;

            currentEnemy = enemies[currentEnemyIndex];

            //Enemies are put into an array
            enemies = new Character[] { troll, aHead, creepyMan, jakeFromStateFarm };
            
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
            switch(currentScene)
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

            if(choice == 1)
            {
                currentScene = 0;
                currentEnemyIndex = 0;
                currentEnemy = enemies[currentEnemyIndex];
            }
            else
            {
                gameOver = true;
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
            player.name = Console.ReadLine();
            Console.WriteLine("");
            Console.Clear();

            //Askes player if they are sure that want to keep the name they have choosen.
            int choice = GetInput("You have entered " + player.name + " . Are you sure you want to keep this name?", "Yes", "No");

            //If player has choosen to keep their name...
            if (choice == 1)
            {
                //...continue on 
                currentScene++;
            }

            Console.WriteLine("Welcome " + player.name + "!");
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int choice = GetInput("Please choose a class", "Wizard", "Knight");

            if (choice == 1)
            {
                player.health = 50;
                player.attackPower = 30000000000;
                player.defensePower = 500000;
                currentScene++;
            }
            else if (choice == 2)
            {
                player.health = 75;
                player.attackPower = 15;
                player.defensePower = 5;
                currentScene++;
            }
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Character character)
        {

            Console.WriteLine("Name " + character.name);
            Console.WriteLine("Health " + character.health);
            Console.WriteLine("Attack Power " + character.attackPower);
            Console.WriteLine("Defense Power " + character.defensePower);
            Console.WriteLine("");

        }

        /// <summary>
        /// Calculates the amount of damage that will be done to a character
        /// </summary>
        /// <param name="attackPower">The attacking character's attack power</param>
        /// <param name="defensePower">The defending character's defense power</param>
        /// <returns>The amount of damage done to the defender</returns>
        float CalculateDamage(float attackPower, float defensePower)
        {
            float damageTaken = attackPower - defensePower;

            if(damageTaken < 0)
            {
                damageTaken = 0;
            }

            return damageTaken;
        }

        /// <summary>
        /// Deals damage to a character based on an attacker's attack power
        /// </summary>
        /// <param name="attacker">The character that initiated the attack</param>
        /// <param name="defender">The character that is being attacked</param>
        /// <returns>The amount of damage done to the defender</returns>
        public float Attack(ref Character attacker, ref Character defender)
        {
            float damageTaken = CalculateDamage(attacker.attackPower, defender.defensePower);
            defender.health -= damageTaken;
            
            if(defender.health < 0)
            {
                defender.health = 0;
            }

            return damageTaken;
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            float damageDealt = 0;

            DisplayStats(player);
            DisplayStats(currentEnemy);

            int choice = GetInput("A " + currentEnemy.name + " approaches you. What do you do?", "Attack", "Dodge");

            if (choice == 1)
            {
                damageDealt = Attack(ref player, ref currentEnemy);
                Console.WriteLine("You dealt " + damageDealt + " damage!");

            }
            else if (choice == 2)
            {
                Console.WriteLine("You dodged the trolls attack");
                Console.ReadKey(true);
                Console.Clear();
                return;
            }

            damageDealt = Attack(ref currentEnemy, ref player);
            Console.WriteLine("The " + currentEnemy.name + " dealt" + damageDealt);

            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            if(player.health <= 0)
            {
                Console.WriteLine("You were slain...");
                Console.ReadKey(true);
                Console.Clear();

                currentScene = 3;
            }
            else if(currentEnemy.health <= 0)
            {
                Console.WriteLine("You slayed the " + currentEnemy.name);
                Console.ReadKey(true);
                Console.Clear();
                currentEnemyIndex++;

                if (currentEnemyIndex >= enemies.Length)
                {
                    currentScene = 3;
                    Console.WriteLine("You've slain all the enemies! You are a true warrior.");
                    return;
                }
                currentEnemy = enemies[currentEnemyIndex];
            }
        }
    }
}
