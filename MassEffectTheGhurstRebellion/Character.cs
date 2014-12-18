using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassEffectTheGhurstRebellion
{
    class Character
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int KineticBarrier { get; set; }
        public int Dexterity { get; set; }
        public int Strength { get; set; }
        public List<Weapon> WeaponInventory { get; set; }
        public List<Item> ItemInventory { get; set; }

        /// <summary>
        /// Character constructor
        /// </summary>
        /// <param name="name">Character's name.</param>
        /// <param name="hp">Character's starting HP.</param>
        /// <param name="kineticBarier">Character's starting kinetic barrier.</param>
        /// <param name="dexterity">Character's dexterity rating. Affects dodge and hit chance.</param>
        /// <param name="strength">Character's strength rating. Affects melee damage and damage taken.</param>
        public Character(string name, int hp, int kineticBarier, int dexterity, int strength)
        {
            Name = name;
            HP = hp;
            KineticBarrier = kineticBarier;
            Dexterity = dexterity;
            Strength = strength;
            WeaponInventory = new List<Weapon>();
            ItemInventory = new List<Item>();
        }

        /// <summary>
        /// Attacks the player
        /// </summary>
        /// <param name="player">Player to attack</param>
        /// <param name="weapon">Weapon used to attack player</param>
        public void Attack(Player player, Weapon weapon)
        {
            Console.WriteLine("{0} used {1}!", Name, weapon.Name);
            if (Game.HitChance(this.Dexterity, player.Dexterity))
            {
                if (!weapon.IsRanged)
                {
                    // attack with a melee weapon; melee weapons aren't affected by kinetic barriers
                    int meleeDamage = Game.DefenseCalculation(player.Strength, Game.MeleeOffenseCalculation(Strength, weapon.Power));
                    Console.WriteLine("Kinetic barriers don't repel against melee attacks! You've been damaged for {0} HP!", meleeDamage);
                    player.HP -= meleeDamage;
                }
                else if (player.KineticBarrier == 0)
                {
                    // ranged attack with no kinetic barrier left
                    int rangedDamage = Game.DefenseCalculation(player.Strength, weapon.Power);
                    Console.WriteLine("You've been damaged for {0} HP!", rangedDamage);
                    player.HP -= rangedDamage;
                }
                else if (weapon.Power > player.KineticBarrier)
                {
                    // ranged attack with kinetic barrier left; kinetic barrier goes down and player is damaged
                    int difference = weapon.Power - player.KineticBarrier;
                    int rangedDamage = Game.DefenseCalculation(player.Strength, difference);
                    Game.WordWrap(String.Format("You kinetic barrier absorbs {0} damage and goes down! You also receive {1} HP damage!", player.KineticBarrier, rangedDamage));
                    player.KineticBarrier = 0;
                    player.HP -= rangedDamage;
                }
                else
                {
                    // ranged attack that doesn't take down the kinetic barrier
                    Console.WriteLine("Your kinetic barrier absorbs {0} damage!", weapon.Power);
                    player.KineticBarrier -= weapon.Power;
                }
            }
            else
                Console.WriteLine("{0} missed!", Name);
        }
    }
}
