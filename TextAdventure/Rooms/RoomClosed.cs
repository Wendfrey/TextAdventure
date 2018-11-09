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
                ene = new Enemigo(Enemigo.eneList[CustomMath.RandomIntNumber(Enemigo.eneList.Length - 1)], ((int)Math.Pow(Program.GetLevel() + 1, 1.4) < Program.pl.GetLevel()) ? Program.pl.GetLevel() : (int) Math.Pow(Program.GetLevel() + 1, 1.4));
                ene.SetName("Super " + ene.GetName());
                if(random < 0.5)
                {
                    item[0] = new ItemWeapon("Espada legendaria", 0, CustomMath.RandomIntNumber(5, 3) + Program.level,(Program.level < 15)?CustomMath.RandomIntNumber(25 + Program.level, 25) * 0.01f : CustomMath.RandomIntNumber(40, 25) * 0.01f);
                }
                else
                {
                    item[0] = new ItemWeapon("Espada de espadas", 0, CustomMath.RandomIntNumber(3, 0) + Program.level*2, CustomMath.RandomIntNumber(25, 0) * 0.01f);
                }
            }
            else
            {
                if (random < 0.05)
                {
                    item[0] = new ItemWeapon("Espada legendaria", 0, CustomMath.RandomIntNumber(5, 3) + Program.level, (Program.level < 15) ? CustomMath.RandomIntNumber(25 + Program.level, 25) * 0.01f : CustomMath.RandomIntNumber(40, 25) * 0.01f);
                }
                else if (random < 0.3)
                {
                    item[0] = new ItemWeapon("Espada buena", 0, CustomMath.RandomIntNumber(3, 0) + Program.level, CustomMath.RandomIntNumber(25, 0) * 0.01f);
                }
                else if (random < 0.95)
                {
                    item[0] = new ItemWeapon("Espada normal", 0, (CustomMath.RandomIntNumber(3, 0) + Program.level) / 2, CustomMath.RandomIntNumber(25, 0) * 0.01f);
                }
                else
                {
                    item[0] = new ItemWeapon("Espada podrida", 0, 1, CustomMath.RandomIntNumber(0, -45) * 0.01f);
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
