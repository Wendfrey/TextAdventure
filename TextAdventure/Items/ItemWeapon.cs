using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class ItemWeapon : ItemEquipable
    {
        protected int hitPerc;
        public ItemWeapon(string name,int att, int hitPerc) : base(name: name, att: att)
        {
            this.hitPerc = hitPerc;
        }

        public int GetHitPercInt()
        {
            return hitPerc;
        }

        public float GetHitPercFloat()
        {
            return hitPerc / 100f;
        }

    }
}
