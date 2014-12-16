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

        public Weapon(string name, string description, bool isRanged, int power)
        {
            Name = name;
            Description = description;
            IsRanged = isRanged;
            Power = power;
        }
    }
}
