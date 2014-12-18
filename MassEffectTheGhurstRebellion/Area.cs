using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassEffectTheGhurstRebellion
{
    class Area
    {
        public string Name { get; set; }
        public List<Weapon> WeaponsInArea { get; set; }
        public List<Item> ItemsInArea { get; set; }
        public string AreaDescription { get; set; }
        public string InvestigateDescription { get; set; }

        /// <summary>
        /// Area constructor
        /// </summary>
        /// <param name="name">Name of the area</param>
        /// <param name="areaDescription">Description of the area</param>
        /// <param name="investigateDescription">Additional description of the area shown in the Investigate menu</param>
        public Area(string name, string areaDescription, string investigateDescription)
        {
            Name = name;
            AreaDescription = areaDescription;
            InvestigateDescription = investigateDescription;
            WeaponsInArea = new List<Weapon>();
            ItemsInArea = new List<Item>();
        }
    }
}
