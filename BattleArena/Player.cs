﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArena
{
    class Player : Entity
    {
        private Item _currentItem;
        private Item _health;
        private int _currentItemIndex;
        private string _job;
        private int _gold;
        private Item[] _inventory;

        public override float Health
        {
            get
            {
                if(_currentItem.Type == ItemType.HEALTH)
                {
                    return base.Health + CurrentItem.StatBoost;
                }
                return base.Health;
            }
        }
        public override float AttackPower
        {
            get
            {
                if(_currentItem.Type == ItemType.ATTACK)
                {
                    return base.AttackPower + CurrentItem.StatBoost;
                }

                return base.AttackPower;
            }
        }

        public override float DefensePower
        {
            get
            {
                if (_currentItem.Type == ItemType.DEFENSE)
                {
                    return base.DefensePower + CurrentItem.StatBoost;
                }

                return base.DefensePower;
            }
        }

        public Item CurrentItem
        {
            get { return _currentItem; }
        }

        public string Job
        {
            get
            {
                return _job;
            }
            set
            {
                _job = value;
            }
        }

        public int Gold
        {
            get { return _gold; }
        }

        public Player()
        {
            _currentItem.Name = "Nothing";
            _currentItemIndex = -1;
            _inventory = new Item[0];
           
        }

        public Player(Item[] items) : base()
        {
            _inventory = new Item[0];
            _currentItem.Name = "Nothing";
            _currentItemIndex = -1;
        }

        public Player(string name, float health, float attackPower, float defensePower, string jobs, int gold) : base(name, health, attackPower, defensePower)
        {
            _currentItem.Name = "Nothing";
            _job = jobs;
            _currentItemIndex = -1;
            _inventory = new Item[0];
            _gold = gold;
        }
        
        public Player(int gold)
        {
            _gold = gold;
        }
        
        /// <summary>
        /// Allows player to by an item from shop and adds the item to the players inventory
        /// </summary>
        /// <param name="item"></param>
        public void Buy(Item item)
        {
            Item[] inventory = new Item[_inventory.Length + 1];

            for (int i = 0; i < _inventory.Length; i++)
            {
                inventory[i] = _inventory[i];
            }

            inventory[_inventory.Length] = item;

            _inventory = inventory;

            _gold -= item.Cost;
        }
        
        /// <returns>List of items in the players inventory</returns>
        public string[] GetItemsInInventoryNames()
        {
            string[] itemNames = new string[_inventory.Length];

            for (int i = 0; i < _inventory.Length; i++)
            {
                itemNames[i] = _inventory[i].Name;
            }

            return itemNames;
        }
        
        /// <summary>
        /// Sets the item at the given index to be the current item
        /// </summary>
        /// <param name="index">The index of the item in the array</param>
        /// <returns>False if the index is outside the bounds of the array</returns>
        public bool TryEquipItem(int index)
        {
            //If the index is out of bounds...
            if (index >= _inventory.Length || index < 0)
            {
                //...return false
                return false;
            }


            _currentItemIndex = index;

            //Set the current item to be the array at the given index
            _currentItem = _inventory[_currentItemIndex];

            Console.WriteLine("Be sure to unequip this item if you would like to use another item");
            Console.ReadKey(true);
            Console.Clear();

            return true;
        }

        /// <summary>
        /// Set the current item to be nothing
        /// </summary>
        /// <returns>False if there is no item equipped</returns>
        public bool TryRemoveCurrentItem()
        {
            //IF the current item is set to nothing...
            if (CurrentItem.Name == "Nothing")
            {
                //...return false
                return false;
            }

            _currentItemIndex = -1;

            //Set item to be nothing
            _currentItem = new Item();
            _currentItem.Name = "Nothing";

            return true;
        }

        public override void Save(StreamWriter writer)
        {
            writer.WriteLine(_job);
            writer.WriteLine(_gold);
            base.Save(writer);
            writer.WriteLine(_currentItemIndex);
            writer.WriteLine(_inventory.Length);

            for(int i = 0; i < _inventory.Length; i++)
            {
                writer.WriteLine(_inventory[i].Name);
                writer.WriteLine(_inventory[i].Cost);
            }
        }

        public override bool Load(StreamReader reader)
        {
            //If the base loading function fails...
            if (!base.Load(reader))
            {
                //...return false
                return false;
            }

            //If the current line can't be converted into an int...
            if(!int.TryParse(reader.ReadLine(), out _currentItemIndex))
            {
                //...return false
                return false;
            }

            //Return whether or not the item was equipped succesfully
            //return TryEquipItem(_currentItemIndex);

            int inventoryLength = 0;

            if (!int.TryParse(reader.ReadLine(), out inventoryLength))
            {
                return false;
            }

            _inventory = new Item[inventoryLength];

            

            for (int i = 0; i < _inventory.Length; i++)
            {
                _inventory[i].Name = reader.ReadLine();

                if (!int.TryParse(reader.ReadLine(), out _inventory[i].Cost))
                {
                    return false;
                }


            }


            return true;


        }
    }
}
