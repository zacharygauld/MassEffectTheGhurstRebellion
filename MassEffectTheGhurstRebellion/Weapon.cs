using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassEffectTheGhurstRebellion
{
    class Weapon
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRanged { get; set; }
        public int Power { get; set; }
        
        /// <summary>
        /// Weapon constructor
        /// </summary>
        /// <param name="name">Name of weapon</param>
        /// <param name="description">Description of weapon</param>
        /// <param name="isRanged">Is the weapon ranged or not?</param>
        /// <param name="power">The weapon's power rating</param>
        public Weapon(string name, string description, bool isRanged, int power)
        {
            Name = name;
            Description = description;
            IsRanged = isRanged;
            Power = power;
        }
    }
}
