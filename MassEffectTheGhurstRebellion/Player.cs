using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassEffectTheGhurstRebellion
{
    // Player inherits from Character class
    class Player : Character
    {
        public int Points { get; set; }
        public int CurrentArea { get; set; }

        // points to start off with to allocate to strength and/or dexterity
        public const int maxPoints = 6;

        /// <summary>
        /// Player constructor
        /// </summary>
        /// <param name="name">Player's name</param>
        /// <param name="hp">Player's starting HP</param>
        /// <param name="kineticBarier">Player's starting kinetic barrier</param>
        /// <param name="dexterity">Player's starting dexterity. Affects hit and dodge chance.</param>
        /// <param name="strength">Player's starting strength. Affects melee damage and damage taken.</param>
        /// <param name="points">Points to start off with.</param>
        /// <param name="currentArea">Represents an index from the list of Areas in the game.</param>
        public Player(string name, int hp, int kineticBarier, int dexterity, int strength, int points, int currentArea)
            : base(name, hp, kineticBarier, dexterity, strength)
        {
            Points = points;
            CurrentArea = currentArea;
        }

        /// <summary>
        /// Attacks enemy
        /// </summary>
        /// <param name="enemy">The enemy to attack</param>
        /// <param name="weapon">Weapon used to attack enemy</param>
        public void Attack(Character enemy, Weapon weapon)
        {
            Console.WriteLine("You used {0}!", weapon.Name);
            if (Game.HitChance(this.Dexterity, enemy.Dexterity))
            {
                if (!weapon.IsRanged)
                {
                    // attack with a melee weapon; melee weapons aren't affected by kinetic barriers
                    int meleeDamage = Game.DefenseCalculation(enemy.Strength, Game.MeleeOffenseCalculation(Strength, weapon.Power));
                    Console.WriteLine("Kinetic barriers don't repel against melee attacks! You've damaged {0} for {1} HP!", enemy.Name, meleeDamage);
                    enemy.HP -= meleeDamage;
                }
                else if (enemy.KineticBarrier == 0)
                {
                    // ranged attack with no kinetic barrier left
                    int rangedDamage = Game.DefenseCalculation(enemy.Strength, weapon.Power);
                    Console.WriteLine("You've damaged {0} for {1} HP!", enemy.Name, rangedDamage);
                    enemy.HP -= rangedDamage;
                }
                else if (weapon.Power > enemy.KineticBarrier)
                {
                    // ranged attack with kinetic barrier left; kinetic barrier goes down and enemy is damaged
                    int difference = weapon.Power - enemy.KineticBarrier;
                    int rangedDamage = Game.DefenseCalculation(enemy.Strength, difference);
                    Game.WordWrap(String.Format("{0}'s kinetic barrier absorbs {1} damage and goes down! {0} also receives {2} HP damage!", enemy.Name, enemy.KineticBarrier, rangedDamage));
                    enemy.KineticBarrier = 0;
                    enemy.HP -= rangedDamage;
                }
                else
                {
                    // ranged attack that doesn't take down the kinetic barrier
                    Console.WriteLine("{0}'s kinetic barrier absorbs {1} damage!", enemy.Name, weapon.Power);
                    enemy.KineticBarrier -= weapon.Power;
                }
            }
            else
                Console.WriteLine("You missed!");         
        }
    }
}
