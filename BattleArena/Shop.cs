using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    class Shop
    {
        private int _gold;
        private ShopItems[] _inventory;

        public Shop(ShopItems[] inventory)
        {
            _inventory = inventory;
        }

        /// <summary>
        /// Sells item to player if player has enough gold
        /// </summary>
        /// <param name="player">Read player gold to see if player has enough to by the item</param>
        /// <param name="option">Finds which item the player choose to buy</param>
        /// <returns>If player bought something</returns>
        public bool Sell(Player player, int option)
        {
            //If player doesn't have enough gold to buy item...
            if (player.Gold < _inventory[option].Cost)
            {
                //...Display text to tell player they don't have enough gold
                Console.WriteLine("You don't have enough gold!");
            }
            //Otherwise shop sells item to player
            else
            {
                player.Buy(_inventory[option]);
            }

            Console.ReadKey(true);
            Console.Clear();

            return true;

        }

        /// <returns>List of items in the shop</returns>
        public string[] GetShopItemNames()
        {
            //Create a new array
            string[] itemNames = new string[_inventory.Length];

            //Copy all all items in the shop into new array
            for (int i = 0; i < _inventory.Length; i++)
            {
                itemNames[i] = _inventory[i].Name;
            }

            //Returns the list of all the items in the shop
            return itemNames;
        }
    }
}
