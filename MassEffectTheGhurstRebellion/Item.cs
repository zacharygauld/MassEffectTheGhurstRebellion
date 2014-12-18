using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassEffectTheGhurstRebellion
{
    class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Item constructor
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <param name="description">Description of item</param>
        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
