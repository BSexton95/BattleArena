using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
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
        string playerName;

        Character troll;
        Character aHead;
        Character creepyMan;
        Character jakeFromStateFarm;

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            troll.name = "Troll";
            troll.health = 20;
            troll.attackPower = 30;
            troll.defensePower = 5;

            aHead.name = "A Head";
            aHead.health = 20;
            aHead.attackPower = 30;
            aHead.defensePower = 5;

            creepyMan.name = "Creepy Man";
            creepyMan.health = 30;
            creepyMan.attackPower = 30;
            creepyMan.defensePower = 5;

            jakeFromStateFarm.name = "Jake From State Farm";
            jakeFromStateFarm.health = 50;
            jakeFromStateFarm.attackPower = 30;
            jakeFromStateFarm.defensePower = 10;

            enemies = new Character[] { troll, aHead, creepyMan, jakeFromStateFarm };
            
        }

        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        public void Update()
        {
        }

        /// <summary>
        /// This function is called before the applications closes
        /// </summary>
        public void End()
        {
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
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game
        /// </summary>
        void DisplayMainMenu()
        {

            GetPlayerName();
            CharacterSelection();

        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            Console.WriteLine("Welcome to the Battle Arena! What is your fighters name?");
            Console.ReadLine();

            playerName = Console.ReadLine();
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int choice = GetInput("Welcome " + playerName + "! Please Select a class.", "Brawler", "Gunman");
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

        }

        /// <summary>
        /// Calculates the amount of damage that will be done to a character
        /// </summary>
        /// <param name="attackPower">The attacking character's attack power</param>
        /// <param name="defensePower">The defending character's defense power</param>
        /// <returns>The amount of damage done to the defender</returns>
        float CalculateDamage(float attackPower, float defensePower)
        {
            return attackPower - defensePower;
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
            return damageTaken;
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            //Displays player stats
            DisplayStats(player);
            //Displays enemies stats
            DisplayStats(currentEnemy);

            float damageDealt = Attack(ref player, ref currentEnemy);
            Console.WriteLine("You dealt " + player.attackPower + " damage!" +
                "\n The " + currentEnemy + "dealt " + currentEnemy.attackPower);

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
                Console.WriteLine("You died");
            }
            else if(currentEnemy.health <= 0)
            {
                currentEnemyIndex++;
                currentEnemy = enemies[currentEnemyIndex];
            }
        }

    }
}
