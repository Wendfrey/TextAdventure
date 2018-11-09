using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class ItemEquipable : Item
    {
        protected int hpM;
        protected int attM;
        protected int defM;
        protected float accM;

        public ItemEquipable(string name, int hp, int att, int def, float acc) : base(name)
        {
            this.hpM = hp;
            attM = att;
            defM = def;
            accM = acc;
        }

        public int ModifierHp()
        {
            return hpM;
        }

        public int ModifierAtt()
        {
            return attM;
        }

        public int ModifierDef()
        {
            return defM;
        }

        public float ModifierAcc()
        {
            return accM;
        }
    }
}
