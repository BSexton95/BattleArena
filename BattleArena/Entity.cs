using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArena
{
    class Entity
    {
        private string _name;
        private float _health;
        private float _attackPower;
        private float _defensePower;

        public string Name
        {
            get { return _name; }
        }

        public virtual float Health
        {
            get {  return _health; }
        }

        public virtual float AttackPower
        {
            get { return _attackPower; }
        }

        public virtual float DefensePower
        {
            get { return _defensePower; }
        }

        public Entity()
        {
            _name = "Default";
            _health = 0;
            _attackPower = 0;
            _defensePower = 0;
        }

        public Entity(string name, float health, float attackPower, float defensePower)
        {
            _name = name;
            _health = health;
            _attackPower = attackPower;
            _defensePower = defensePower;
        }
        /// <summary>
        /// Function takes the attack power from entity that is attacking and subtacts if from the defense power
        /// of the entity that is being attacked then subtracts the damage the entity took from its health.
        /// </summary>
        /// <param name="damageAmount">Attacking entitys attack power</param>
        /// <returns>The amount of damage taken</returns>
        public float TakeDamage(float damageAmount)
        {
            float damageTaken = damageAmount - DefensePower;

            //If the damage taken is less than 0...
            if ( damageTaken < 0)
            {
                //...then the defending entity took no damage.
                damageTaken = 0;
            }

            //Set health to the amount of damage taken subtracted from the original health amount.
            _health -= damageTaken;

            return damageTaken;
        }

        /// <summary>
        /// Function calls the take damage function
        /// </summary>
        /// <param name="defender">The entity that is defending</param>
        /// <returns>The damage taken from defender</returns>
        public float Attack(Entity defender)
        {
            return defender.TakeDamage(AttackPower);
        }

        /// <summary>
        /// Saves entity stats
        /// </summary>
        /// <param name="writer">Writes all the current stats in save file</param>
        public virtual void Save(StreamWriter writer)
        {
            writer.WriteLine(_name);
            writer.WriteLine(_health);
            writer.WriteLine(_attackPower);
            writer.WriteLine(_defensePower);
        }

        /// <summary>
        /// Loads the entity stats that were previously saved
        /// </summary>
        /// <param name="reader">reads all the stats that were put in the save file</param>
        /// <returns>True if load is successful</returns>
        public virtual bool Load(StreamReader reader)
        {
            _name = reader.ReadLine();
            if(!float.TryParse(reader.ReadLine(), out _health))
            {
                return false;
            }

            if (!float.TryParse(reader.ReadLine(), out _attackPower))
            {
                return false;
            }

            if (!float.TryParse(reader.ReadLine(), out _defensePower))
            {
                return false;
            }

            return true;
        }
    }
}
