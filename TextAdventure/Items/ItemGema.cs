using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class ItemGema : ItemEquipable
    {
        public ItemGema(string name, int hp, int attM, int manaM) : base(name: name, attMag: attM, mana: manaM)
        {
        }
    }
}
