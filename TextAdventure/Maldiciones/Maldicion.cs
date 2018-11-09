using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Maldiciones
{
    class Maldicion
    {
        public static int cantidadMaldiciones = 5;

        readonly string name;
        readonly int id;
        public Maldicion(int id)
        {
            this.id = id;
            switch (this.id) {
                case 0:
                    name = "Maldición de la dislexia";
                    break;
                case 1:
                    name = "Maldición del torpe";
                    break;
                case 2:
                    name = "Maldición del confuso";
                    break;
                case 3:
                    name = "Maldición del inválido";
                    break;
                case 4:
                    name = "Maldición del ciego";
                    for (int i = 0; i < Program.lvlLayout.Count; i++)
                        Program.lvlLayout[i].SetVisible(0);
                    Program.pl.currentRoom.SetVisible(2);
                    break;
            }
        }

        public string GetName()
        {
            return name;
        }

        public int GetId()
        {
            return id;
        }
    }
}
