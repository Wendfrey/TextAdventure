using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventure.Rooms;

namespace TextAdventure
{
    class ItemScroll : ItemConsumable
    {
        readonly int id;
        public ItemScroll(string name, int id) : base(name)
        {

            this.id = id;
        }

        public int GetId()
        {
            return id;
        }

        public override void Consumir()
        {
            Effect();
        }

        private void Effect()
        {
            Player pl = Program.pl;
            ConsoleBuffer buffer = Program.buffer;
            switch (id)
            {
                case 0:
                    if (!pl.GetMaldicion(4))
                    {
                        List<Room> r0 = Program.lvlLayout;
                        for (int i = 0; i < r0.Count; i++)
                        {
                            if(r0[i].IsVisible() == 0)
                                r0[i].SetVisible(3);
                        }
                        buffer.InsertText("¡El piso se ha revelado!");
                    }
                    else
                    {
                        buffer.InsertText("¡La maldición del ciego ha anulado el efecto!");
                    }
                    break;

                case 1:
                    List<Room> r1 = Program.lvlLayout;
                    for (int i = 0; i < r1.Count; i++)
                    {
                        if (r1[i].GetType() == typeof(RoomExit))
                        {
                            if (pl.GetMaldicion(4))
                                pl.currentRoom.SetVisible(0);
                            pl.currentRoom = r1[i];
                            r1[i].SetVisible(2);
                            buffer.InsertText("¡Te has teletransportado!");
                            if (Program.inCombat == false)
                                buffer.InsertText(pl.currentRoom.GetDescriptionTotal());
                            i = r1.Count;
                        }
                    }
                    break;
            }
        }
    }
}
