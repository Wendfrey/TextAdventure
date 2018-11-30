using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Rooms
{
    class RoomGema : Room
    {
        bool isCurse;

        public RoomGema(int x, int y) : base(x, y)
        {
            this.x = x;
            this.y = y;
            item[0] = new ItemGema("Gema", 0, CustomMath.RandomIntNumber(3+Program.level,-1 + Program.level), CustomMath.RandomIntNumber(3 + Program.level, -1 + Program.level));
            descr = "Al entrar, sientes una gran presencia. Dentro de la sala, en un pedestal, hay una gema con una tenue luz que ilumina a duras penas las paredes de la sala.";
            ene = null;
            isCurse = (CustomMath.RandomUnit() < 0.6) ? true : false;
        }

        override public Item DropItem(int i)
        {
            Item value = base.DropItem(i);
            if(value != null && i == 0)
            {
                descr = "Una sala con un pedestal en el centro";
                if (isCurse)
                {
                    isCurse = false;
                    Program.buffer.InsertText("Al coger '"+value.GetName()+"' sientes como tu cuerpo se vuelve mas pesado");
                    Program.pl.ObtenMaldicion();
                }
            }

            return value;
        }
    }
}
