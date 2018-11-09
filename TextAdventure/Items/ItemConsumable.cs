using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    abstract class ItemConsumable : Item
    {
        public ItemConsumable(string name) : base(name)
        {
        }

        public abstract void Consumir();
    }
}
