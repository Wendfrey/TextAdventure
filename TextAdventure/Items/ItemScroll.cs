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
                        List<Room> r = Program.lvlLayout;
                        for (int i = 0; i < r.Count; i++)
                        {
                            r[i].SetVisible(2);
                        }
                        buffer.InsertText("¡El piso se ha revelado!");
                    }
                    else
                    {
                        buffer.InsertText("¡La maldición del ciego ha anulado el efecto!");
                    }
                    break;

                case 1:
                    
                    break;
            }
        }
    }
}
