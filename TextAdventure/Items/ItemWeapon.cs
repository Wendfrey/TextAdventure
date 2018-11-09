using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class ItemWeapon : ItemEquipable
    {

        public ItemWeapon(string name, int hp,int att, float acc) : base(name, hp, att, 0,acc)
        {
        }

    }
}
