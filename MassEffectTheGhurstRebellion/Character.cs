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
        public bool ShieldsUp { get; set; }
        public int RangedAccuracy { get; set; }
        public int MeleeAccuracy { get; set; }
        public int Strength { get; set; }
        public List<Weapon> WeaponInventory { get; set; }
        public List<Item> ItemInventory { get; set; }

        public Character(string name, int hp, int kineticBarier, int rangedAccuracy, int meleeAccuracy, int strength)
        {
            Name = name;
            HP = hp;
            KineticBarrier = kineticBarier;
            ShieldsUp = true;
            RangedAccuracy = rangedAccuracy;
            MeleeAccuracy = meleeAccuracy;
            Strength = strength;
            WeaponInventory = new List<Weapon>();
            ItemInventory = new List<Item>();
        }

        public void AttackPlayer(Player player, Weapon weapon)
        {
            Console.WriteLine("{0} used {1}!", Name, weapon.Name);
            if (!weapon.IsRanged)
            {
                Console.WriteLine("Kinetic barriers don't repel melee attacks! You've been damaged for {0} HP!", weapon.Power);
                player.HP -= weapon.Power;
            }
            else
            {
                Console.WriteLine("Your kinetic barrier absorbs {0} damage!", weapon.Power);
                player.KineticBarrier -= weapon.Power;
            }
        }
    }
}
