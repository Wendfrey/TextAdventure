using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class ItemGema : ItemEquipable
    {
        public ItemGema(string name, int hp, int att, int def) : base(name, hp, att, def, 0)
        {
            hpM = hp;
            attM = att;
            defM = def;
        }
    }
}
