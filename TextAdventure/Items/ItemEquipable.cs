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
        protected int attMM;
        protected int manaM;
        protected int speedM;

        public ItemEquipable(string name, int hp = 0, int att = 0, int def = 0, int attMag = 0, int mana = 0, int speed = 0) : base(name)
        {
            hpM = hp;
            attM = att;
            defM = def;
            attMM = attMag;
            manaM = mana;
            speedM = speed;
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
    }
}
