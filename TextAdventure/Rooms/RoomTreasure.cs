using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Rooms
{
    class RoomTreasure : Room
    {
        bool activated = false;

        public RoomTreasure(int x, int y) : base(x, y)
        {
            this.descr = "En medio de la sala hay un cofre con objetos";
            int value = 0;
            List<Item> list = new List<Item>{ new ItemScroll("Pergamino de salida", 1), new ItemScroll("Pergamino de visión", 0), new ItemPocion("Minipoción de maná", CustomMath.RandomIntNumber(50, 25), ItemPocion.PocionType.mana), new ItemPocion("Poción de maná", CustomMath.RandomIntNumber(75, 50), ItemPocion.PocionType.mana), new ItemPocion("Gran poción de maná", 100, ItemPocion.PocionType.mana), new ItemPocion("Minipoción de vida", CustomMath.RandomIntNumber(50, 25), ItemPocion.PocionType.hp), new ItemPocion("Poción de vida", CustomMath.RandomIntNumber(75, 50), ItemPocion.PocionType.hp), new ItemPocion("Gran poción de vida", 100, ItemPocion.PocionType.hp), new ItemArmor("Armadura tapizada", 5+(5*Program.level), Program.level, 45)};
            List<int> listValue = new List<int>{ 2, 2, 1, 2, 3, 1, 2, 3, 3};

            while (value < 3)
            {
                int rand = CustomMath.RandomIntNumber(list.Count-1);
                GetItem(list[rand]);
                value += listValue[rand];
                if (value < 3)
                {
                    int xx = 3 - value;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (listValue[i] > xx)
                        {
                            listValue.RemoveAt(i);
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }

        public override bool GetItem(Item item)
        {
            if (!RoomHasItem())
                descr = "Una sala con un cofre con objetos";
            return base.GetItem(item);
        }

        public override Item DropItem(int i)
        {
            Item item = base.DropItem(i);
            if (!RoomHasItem())
            {
                descr = "Una sala con un cofre vacio";
            }
            return item;
        }
    }
}
