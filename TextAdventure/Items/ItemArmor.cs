using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class ItemArmor : ItemEquipable
    {
        readonly int avoidPerc;
        public ItemArmor(string name, int avoidPerc) : base(name: name, def: 3)
        {
            this.avoidPerc = avoidPerc;
        }

        public int GetAvoidPercInt()
        {
            return avoidPerc;
        }

        public float GetAvoidPercFloat()
        {
            return avoidPerc * 0.01f;
        }
    }
}
