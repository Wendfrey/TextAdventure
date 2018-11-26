using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Rooms
{
    class Room
    {
        protected Item[]  item = { null, null, null };
        protected int x, y;
        protected Room north = null, west = null, south = null, east = null;
        public Enemigo ene = null;
        protected string descr;
        int visibility = 0;

        public Room(int x, int y)
        {
            int level = Program.GetLevel();
            this.x = x;
            this.y = y;
            descr = FileReader.RandomDescr("Text/roomDescr.txt");
            if (!(x == 0 && y == 0))
            {
                double prob = (level < 4) ? 0.1 + 0.3 * level / 4d : 0.4;
                if (CustomMath.RandomUnit() < prob)
                    ene = new Enemigo(Enemigo.eneList[CustomMath.RandomIntNumber(Enemigo.eneList.Length - 1)], level);

                if (CustomMath.RandomUnit() < 0.02)
                {
                    if (CustomMath.RandomUnit() < 0.5)
                    {
                        item[0] = new ItemPocion("Poción de vida", CustomMath.RandomIntNumber(75, 50), ItemPocion.PocionType.hp);
                    }
                    else
                    {
                        item[0] = new ItemPocion("Poción de mana", CustomMath.RandomIntNumber(75, 50), ItemPocion.PocionType.mana);
                    }
                }
            }
        }

        public int GetPosX()
        {
            return x;
        }

        public int GetPosY()
        {
            return y;
        }

        public void SetPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public string[] GetDescriptionTotal()
        {
            int lines = 0;
            string ddd = "\""+ descr+"\"";
            lines++;
            ddd += "\nSe puede ir al:\n";
            ddd += (GetNorthRoom() != null) ? " -norte" : null;
            ddd += (GetWestRoom() != null) ? " -oeste" : null;
            ddd += (GetSouthRoom() != null) ? " -sur" : null;
            ddd += (GetEastRoom() != null) ? " -este" : null;
            lines++;
            lines++;
            if (RoomHasItem())
            {
                ddd += "\nSe puede <<coger>> ";
                bool temp = true;
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i] != null)
                    {
                        if (temp)
                            temp = false;
                        else
                            ddd += ", ";
                        ddd += item[i].GetName();
                    }
                }
                lines++;
            }
            string[] finalDescr = ddd.Split('\n');
            return finalDescr;
        }

        public string GetDescription()
        {
            return descr;
        }
        public bool RoomHasItem()
        {
            for (int i = 0; i < item.Length; i++)
                if (item[i] != null)
                    return true;
            return false;
        }

        virtual public Item DropItem(int i)
        {
            Item ret = item[i];
            item[i] = null;
            Item.Ordenar(item);
            return ret;
        }

        public bool GetItem(Item item)
        {
            if (!HasItemSpace())
                return false;
            for(int i = 0; i < this.item.Length; i++)
            {
                if (this.item[i] == null)
                {
                    this.item[i] = item;
                    i = this.item.Length;
                }
            }
            return true;
        }

        public bool HasItemSpace()
        {
            for(int i = 0; i<item.Length; i++)
            {
                if (item[i] == null)
                    return true;
            }
            return false;
        }

        public Item[] GetRoomItems()
        {
            return item;
        }

        public void ListOfItems()
        {
            string text = "    ";
            int acc = 0;
            for (int j = 0; j < item.Length; j++)
            {
                if (item[j] != null)
                {
                    string temp = "[" + j + "]->" + item[j].GetName();
                    if (text.Length+temp.Length > 100)
                    {
                        Program.buffer.InsertText(text);
                        text = "    ";
                    }
                    text +=  temp+ "  ";
                    acc++;
                }
            }
            if (!text.Equals("    "))
            {
                Program.buffer.InsertText(text);
            }
        }

        public int IsVisible()
        {
            return visibility;
        }

        public void SetVisible(int visible)
        {
            visibility = visible;
        }
        ////////////////////////////////
        public Room GetNorthRoom()
        {
            return north;
        }

        public Room GetWestRoom()
        {
            return west;
        }

        public Room GetSouthRoom()
        {
            return south;
        }

        public Room GetEastRoom()
        {
            return east;
        }

        ////////////////////////////////
        public void SetNorth(Room room)
        {
            this.north = room;
        }
        public void SetWest(Room room)
        {
            this.west = room;
        }
        public void SetSouth(Room room)
        {
            this.south = room;
        }
        public void SetEast(Room room)
        {
            this.east = room;
        }
    }
}
