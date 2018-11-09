using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Rooms
{
    class RoomExit : Room
    {
        public RoomExit(int x, int y) : base(x, y){
            ene = null;
            for (int i = 0; i < item.Length; i++)
            {
                item[i] = null;
            }
            descr = "En el centro de la habitación hay una escalera de caracol en la que puedes <<bajar>>";
        }
    }
}
