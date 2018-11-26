using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Rooms
{
    class RoomClosed : Room
    {
        bool isClosed = true;
        public RoomClosed(int x, int y) : base(x, y)
        {
            double random = CustomMath.RandomUnit();
            if (CustomMath.RandomUnit() < 1/3d)
            {
                ene = new Enemigo(Enemigo.eneList[CustomMath.RandomIntNumber(Enemigo.eneList.Length - 1)], (int) Math.Pow(Program.GetLevel() + 1, 1.4));
                ene.SetName("Super " + ene.GetName());
                if(random < 0.5)
                {
                    item[0] = new ItemWeapon("Espada legendaria", CustomMath.RandomIntNumber(5, 3) + Program.level, 20, CustomMath.RandomIntNumber(5, 3) + Program.level);
                }
                else
                {
                    item[0] = new ItemWeapon("Espada de espadas", (CustomMath.RandomIntNumber(3, 0) + Program.level)*2, -10);
                }
            }
            else
            {
                if (random < 0.05)
                {
                    item[0] = new ItemWeapon("Espada legendaria", CustomMath.RandomIntNumber(5, 3) + Program.level, 20, CustomMath.RandomIntNumber(5, 3) + Program.level);
                }
                else if (random < 0.3)
                {
                    item[0] = new ItemWeapon("Espada buena", CustomMath.RandomIntNumber(3, 0) + Program.level, 20);
                }
                else if (random < 0.675)
                {
                    item[0] = new ItemWeapon("Espada normal", (CustomMath.RandomIntNumber(3, 0) + Program.level) / 2, 10);
                }
                else if (random < 0.95)
                {
                    item[0] = new ItemWeapon("Baston Mágico", (CustomMath.RandomIntNumber(2, 0) + Program.level) / 3, 10, CustomMath.RandomIntNumber(3,1)+Program.level/2);
                }
                else
                {
                    item[0] = new ItemWeapon("Espada podrida", 1, 0);
                }
            }
        }

        public bool IsClosed()
        {
            return isClosed;
        }

        public void OpenRoom()
        {
            isClosed = false;
        }

        public static bool UseKey()
        {
            Program.buffer.InsertText("La habitación esta cerrada con llave");
            Program.buffer.InsertText("¿Que objeto quieres usar para abrir la habitación?");
            Program.pl.ListOfItems();
            Program.buffer.PrintText(Program.buffer.height-3);
            Program.buffer.PrintBackground();
            Program.buffer.Print(1, Program.buffer.height - 2,">");
            Program.SmallMap();
            Program.buffer.PrintScreen();
            Console.SetCursorPosition(2, Program.buffer.height - 2);
            Item[] bag = Program.pl.GetBag();
            bool obj = int.TryParse(Console.ReadLine(), out int num);
            if (obj && Program.pl.GetBag().Length > num && num >= 0 && bag[num] != null)
            {
                if (bag[num].GetName().Equals("Llave vieja"))
                {
                    Program.buffer.InsertText("Has usado "+bag[num].GetName());
                    bag[num] = null;
                    Item.Ordenar(bag);
                    return true;
                }
                else
                {
                    Program.buffer.InsertText("No parece que "+bag[num].GetName()+" encaje");
                }
            }
            else if(obj)
            {
                Program.buffer.InsertText("Ese numero no es válido");
            }
            else
            {
                Program.buffer.InsertText("Tiene que ser un numero");
            }
            return false;
        }
    }
}
