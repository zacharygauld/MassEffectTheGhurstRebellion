using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassEffectTheGhurstRebellion
{
    class Player : Character
    {
        public int Points { get; set; }
        public int CurrentArea { get; set; }

        public const int maxPoints = 6;

        public Player(string name, int hp, int kineticBarier, int rangedAccuracy, int meleeAccuracy, int strength, int points, int currentArea)
            : base(name, hp, kineticBarier, rangedAccuracy, meleeAccuracy, strength)
        {
            Points = points;
            CurrentArea = currentArea;
        }

        public void AttackEnemy(Character enemy, Weapon weapon)
        {
            Console.WriteLine("You used {0}!", weapon.Name);
            if (!weapon.IsRanged)
            {
                Console.WriteLine("Kinetic barriers don't repel melee attacks! You've damaged {0} for {1} HP!", enemy.Name, weapon.Power);
                enemy.HP -= weapon.Power;
            }
            else if (enemy.KineticBarrier == 0)
            {
                Console.WriteLine("{0}'s kinetic barrier absorbs {1} damage!", enemy.Name, weapon.Power);
                enemy.KineticBarrier -= weapon.Power;
            }
            else if (weapon.Power > enemy.KineticBarrier)
            {
                int difference = weapon.Power - enemy.KineticBarrier;
                Game.WordWrap(String.Format("{0}'s kinetic barrier absorbs {1} damage and goes down! {0} also receives {2} HP damage!", enemy.Name, enemy.KineticBarrier, difference));
                enemy.KineticBarrier = 0;
                enemy.HP -= difference;
            }
            else
            {
                Console.WriteLine("{0}'s kinetic barrier absorbs {1} damage!", enemy.Name, weapon.Power);
                enemy.KineticBarrier -= weapon.Power;
            }
        }
    }
}
