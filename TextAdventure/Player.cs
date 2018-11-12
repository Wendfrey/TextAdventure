using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TextAdventure.Rooms;
using TextAdventure.Maldiciones;

namespace TextAdventure
{
    class Player : CombatClass
    {
        public Room currentRoom = null;
        public int lastRoom = -1;
        Item[] bag = new Item[10];
        ItemGema[] gemas = { null, null, null };
        ItemWeapon arma = null;
        ItemArmor armadura = null;
        Maldicion[] mal = { null, null, null };
        public bool levelUp = false;
        int level;
        int exp;
        int excesoMaldito = 0;
        public int ExcesoMaldito
        {
            get
            {
                return excesoMaldito;
            }
            set
            {
                excesoMaldito = value;
                hpM = ((400 * level / 100 + 20) * (10 - excesoMaldito)) / 10;
                if (hp > hpM)
                    hp = hpM;
            }
        }

        public int Experiencia
        {
            get
            {
                return exp;
            }
            set
            {
                exp = value;
                while (CustomMath.ExpNeeded(level + 1) <= exp)
                {
                    level++;
                    levelUp = true;

                    int temp = hpM;
                    hpM = ((400 * level / 100 + 20) * (10 - excesoMaldito)) / 10;
                    hp += hpM - temp;
                    att = 200 * level / 100 + 5;
                    def = 200 * level / 100 + 5;
                    speed = 200 * level / 100 + 5;
                }
            }
        }
        public Player()
        {
            level = 1;
            Experiencia = 0;
            for (int i = 0; i < 5; i++)
                bag[i] = new ItemPocion("Poción de vida", 50, ItemPocion.PocionType.hp);

            armadura = new ItemArmor("Armadura Simple", 45);

            hpM = ((400 * level / 100 + 20) * (10 - excesoMaldito)) / 10;
            hp = hpM;
            att = 200 * level / 100 + 5;
            def = 200 * level / 100 + 5;
            speed = 200 * level / 100 + 5;
            mana = 10;
            manaM = 10;
            hitPerc = 0.1f;
            avoidPerc = armadura.GetAvoidPercFloat();
        }

        public void SetCurrentRoom(Room room)
        {
            currentRoom = room;
        }

        /*_______________Arma______________*/
        public ItemWeapon GetWeapon()
        {
            return arma;
        }

        public ItemWeapon DropWeapon()
        {
            ItemWeapon temp = arma;
            arma = null;
            return temp;
        }

        /*_____________Armadura___________*/
        public ItemArmor GetArmor()
        {
            return armadura;
        }

        public ItemArmor DropArmor()
        {
            ItemArmor temp = armadura;
            armadura = null;
            return temp;
        }

        /*_________________Mochila________________*/
        public void DropItem(int pos)
        {
            if (bag[pos] != null)
            {
                if (currentRoom.GetItem(bag[pos]))
                {
                    Program.buffer.InsertText("Has soltado " + bag[pos].GetName());
                    bag[pos] = null;
                }
            }
        }

        public void ListOfItems()
        {
            string text = "    ";
            int acc = 0;
            for (int j = 0; j < bag.Length; j++)
            {
                if (bag[j] != null)
                {
                    string temp = "[" + j + "]->" + bag[j].GetName();
                    if (text.Length + temp.Length > 100)
                    {
                        Program.buffer.InsertText(text);
                        text = "    ";
                    }
                    text += temp + "  ";
                    acc++;
                }
            }
            if (!text.Equals("    "))
            {
                Program.buffer.InsertText(text);
            }
        }

        public void PickItem(int num)
        {
            Item item = currentRoom.GetRoomItems()[num];
            if (item.GetType().BaseType == typeof(ItemEquipable))
            {
                if (!FilledGemas() && item.GetType() == typeof(ItemGema))
                {
                    for (int i = 0; i < gemas.Length; i++)
                    {
                        if (gemas[i] == null)
                        {
                            gemas[i] = (ItemGema)currentRoom.DropItem(num);
                            Program.buffer.InsertText("Te has equipado " + item.GetName());
                            i = bag.Length;
                        }
                    }
                }
                else if (GetWeapon() == null && item.GetType() == typeof(ItemWeapon))
                {
                    arma = (ItemWeapon)currentRoom.DropItem(num);
                    Program.buffer.InsertText("Te has equipado " + item.GetName());
                }
                else if (GetArmor() == null && item.GetType() == typeof(ItemArmor))
                {
                    armadura = (ItemArmor)currentRoom.DropItem(num);
                    Program.buffer.InsertText("Te has equipado " + item.GetName());
                }
                else if (!FilledBag())
                {
                    for (int i = 0; i < bag.Length; i++)
                    {
                        if (bag[i] == null)
                        {
                            bag[i] = currentRoom.DropItem(num);
                            Program.buffer.InsertText("Has guardado en la mochila " + item.GetName());
                            i = bag.Length;
                        }
                    }
                }
                else
                {
                    Program.buffer.InsertText("Tienes la mochila llena");
                }
            }
            else
            {
                if (!FilledBag())
                {
                    for (int i = 0; i < bag.Length; i++)
                    {
                        if (bag[i] == null)
                        {
                            bag[i] = currentRoom.DropItem(num);
                            Program.buffer.InsertText("Has guardado en la mochila " + item.GetName());
                            i = bag.Length;
                        }
                    }
                }
                else
                {
                    Program.buffer.InsertText("Tienes la mochila llena");
                }
            }
        }

        public bool FilledBag()
        {
            bool check = true;
            for (int i = 0; i < bag.Length; i++)
            {
                if (bag[i] == null)
                {
                    check = false;
                    i = bag.Length;
                }
            }
            return check;
        }

        public Item[] GetBag()
        {
            return bag;
        }

        public void EquipItem(int num)
        {
            Item item = bag[num];
            if (item.GetType().BaseType == typeof(ItemEquipable))
            {
                if (item.GetType() == typeof(ItemWeapon))
                {
                    if (arma == null)
                    {
                        Program.buffer.InsertText("Te has equipado " + item.GetName());
                        arma = (ItemWeapon)item;
                        bag[num] = null;
                    }
                    else
                    {
                        Program.buffer.InsertText("Te has equipado " + item.GetName());
                        Program.buffer.InsertText("Te has desequipado " + arma.GetName());
                        ItemWeapon temp = arma;
                        arma = (ItemWeapon)item;
                        bag[num] = temp;
                    }
                }
                else if (item.GetType() == typeof(ItemGema))
                {
                    bool check = true;
                    for (int i = 0; i < gemas.Length; i++)
                    {
                        if (gemas[i] == null)
                        {
                            Program.buffer.InsertText("Te has equipado " + item.GetName());
                            gemas[i] = (ItemGema)item;
                            bag[num] = null;
                            i = bag.Length;
                            check = false;
                        }
                    }
                    if (check)
                    {
                        Program.buffer.InsertText("¿Que gema quieres desequiparte?");
                        ListOfGems();
                        Program.buffer.PrintBackground();
                        Program.buffer.PrintText(Program.buffer.height - 3);
                        Program.buffer.Print(1, 0, "PRINCIPAL");
                        Program.buffer.Print(1, Program.buffer.height - 2, ">");
                        Program.SmallMap();
                        Program.buffer.PrintScreen();
                        Console.SetCursorPosition(2, Program.buffer.height - 2);

                        bool obj = int.TryParse(Console.ReadLine(), out int num1);
                        if (obj && num1 >= 0 && num1 < gemas.Length && gemas[num1] != null)
                        {
                            Program.buffer.InsertText("Te has equipado " + item.GetName());
                            Program.buffer.InsertText("Te has desequipado " + item.GetName());
                            ItemGema temp = gemas[num1];
                            gemas[num1] = (ItemGema)item;
                            bag[num] = temp;
                        }
                        else if (!obj)
                        {
                            Program.buffer.InsertText("Tiene que ser un numero");
                        }
                        else
                        {
                            Program.buffer.InsertText("Esa posicion no es válida");
                        }
                    }
                }
                else if (item.GetType() == typeof(ItemArmor))
                {
                    if (armadura == null)
                    {
                        Program.buffer.InsertText("Te has equipado " + item.GetName());
                        armadura = (ItemArmor)bag[num];
                        bag[num] = null;
                    }
                    else
                    {
                        Program.buffer.InsertText("Te has equipado " + item.GetName());
                        Program.buffer.InsertText("Te has desequipado " + armadura.GetName());
                        ItemArmor temp = armadura;
                        armadura = (ItemArmor)item;
                        bag[num] = temp;
                    }

                }
            }
            else
            {
                Program.buffer.InsertText("Este objeto no es equipable");
            }
        }

        public void GetItem(Item item)
        {
            for (int i = 0; i < bag.Length; i++)
            {
                if (bag[i] == null)
                {
                    bag[i] = item;
                    i = bag.Length;
                }
            }
        }
        /*_______________Gemas_________________*/
        public bool FilledGemas()
        {
            bool ret = true;
            for (int i = 0; i < gemas.Length; i++)
            {
                if (gemas[i] == null)
                {
                    ret = false;
                    i = gemas.Length;
                }
            }
            return ret;
        }

        public bool EmptyGemas()
        {
            bool ret = true;
            for (int i = 0; i < gemas.Length; i++)
            {
                if (gemas[i] != null)
                {
                    ret = false;
                    i = bag.Length;
                }
            }
            return ret;
        }
        public int GetGemsAtt()
        {
            int att = 0;
            for (int i = 0; i < gemas.Length; i++)
            {
                if (gemas[i] != null)
                    att += gemas[i].ModifierAtt();
            }
            return att;
        }

        public int GetGemsHp()
        {
            int hp = 0;
            for (int i = 0; i < gemas.Length; i++)
            {
                if (gemas[i] != null)
                    hp += gemas[i].ModifierHp();
            }
            return hp;
        }

        public int GetGemsDef()
        {
            int def = 0;
            for (int i = 0; i < gemas.Length; i++)
            {
                if (gemas[i] != null)
                    def += gemas[i].ModifierDef();
            }
            return def;
        }


        public ItemGema[] GetGemas()
        {
            return gemas;
        }

        public void ListOfGems()
        {
            string text = "    ";
            int acc = 0;
            for (int j = 0; j < gemas.Length; j++)
            {
                if (gemas[j] != null)
                {
                    string temp = "[" + j + "]->" + gemas[j].GetName();
                    if (text.Length + temp.Length > 100)
                    {
                        Program.buffer.InsertText(text);
                        text = "    ";
                    }
                    text += temp + "  ";
                    acc++;
                }
            }
            if (!text.Equals("    "))
            {
                Program.buffer.InsertText(text);
            }
        }
        /*_____________Maldiciones_______________*/
        public void ObtenMaldicion()
        {
            bool maldito = true;
            for (int i = 0; i < mal.Length; i++)
            {
                if (mal[i] == null)
                    maldito = false;
            }
            if (maldito == false)
            {
                Maldicion temp = new Maldicion(CustomMath.RandomIntNumber(Maldicion.cantidadMaldiciones - 1));
                for (int i = 0; i < mal.Length; i++)
                {
                    if (mal[i] != null && temp.GetId() == mal[i].GetId())
                    {
                        temp = new Maldicion(CustomMath.RandomIntNumber(Maldicion.cantidadMaldiciones - 1));
                        i = -1;
                    }
                }
                Program.buffer.InsertText("Has obtenido la " + temp.GetName());
                for (int i = 0; i < mal.Length; i++)
                {
                    if (mal[i] == null)
                    {
                        mal[i] = temp;
                        i = mal.Length;
                    }
                }
            }
            else
            {
                Program.buffer.InsertText("Tienes tantas maldiciones que tu cuerpo no es capaz de aguantar más");
                int temp = ((400 * level / 100 + 20) / 10);
                ExcesoMaldito++;
                Program.buffer.InsertText("Has perdido " + temp + " de vida maxima");
                if (hpM <= 0)
                {
                    Program.buffer.InsertText("El peso de las maldiciones exprime tu ultima gota de tu alma");
                    Program.buffer.InsertText("Caes muerto en el suelo");
                    Program.buffer.InsertText("Pulsa cualquier boton para continuar");
                    Console.ReadKey();
                }
            }
        }

        public bool GetMaldicion(int id)
        {
            for (int i = 0; i < mal.Length; i++)
            {
                if (mal[i] != null && id == mal[i].GetId())
                    return true;
            }
            return false;
        }
        public Maldicion[] GetArrMal()
        {
            return mal;
        }
        /*_________________Estadisticas____________*/
        public int GetLevel()
        {
            return level;
        }

        override public int GetAtt()
        {
            int suma = 0;
            if (arma != null)
                suma += arma.ModifierAtt();
            if (armadura != null)
                suma += armadura.ModifierAtt();
            
                suma += GetGemsAtt();

            return base.GetAtt() + suma;
        }

        public override int GetDef()
        {
            int suma = 0;
            if (arma != null)
                suma += arma.ModifierDef();
            if (armadura != null)
                suma += armadura.ModifierDef();

            suma += GetGemsDef();

            return base.GetDef() + suma;
        }

        public void RestoreHealth()
        {
            hp = hpM;
        }

        public void RestoreHealth(int cantidad)
        {
            hp = (hp + cantidad > hpM) ? hpM : hp + cantidad;
        }

        public void RestoreMana()
        {
            mana = manaM;
        }

        public void RestoreMana(int cantidad)
        {
            mana = (mana + cantidad > manaM) ? manaM : mana + cantidad;
        }

        public override float GetHitPerc()
        {
            float fff = base.GetHitPerc();
            if (arma != null)
                fff += arma.GetHitPercFloat();
            return fff;
        }

        /*Sin añadir ni gemas ni arma*/
        public int GetFlatAtt()
        {
            return base.GetAtt();
        }

        public int GetFlatDef()
        {
            return base.GetDef();
        }

        /*_______________Consumicion______________*/
        public bool ConsumeItem(int num)
        {
            bool control = false;
            if (bag[num].GetType() == typeof(ItemPocion))
            {
                ItemPocion item = (ItemPocion)bag[num];
                item.Consumir();
                if (item.GetPocionType() == ItemPocion.PocionType.hp)
                {
                    if (hp < hpM)
                        Program.buffer.InsertText("Has tomado " + bag[num].GetName() + " y has recuperado " + item.GetRecoveryStat() + " de vida");
                    else
                        Program.buffer.InsertText("Has tomado " + bag[num].GetName() + " y te has recuperado el máximo de vida");
                }
                else
                {
                    if (mana < manaM)
                        Program.buffer.InsertText("Has tomado " + bag[num].GetName() + " y has recuperado " + item.GetRecoveryStat() + " de maná");
                    else
                        Program.buffer.InsertText("Has tomado " + bag[num].GetName() + " y te has recuperado el máximo de maná");
                }
                bag[num] = null;
                control = true;
            }
            else if (bag[num].GetType() == typeof(ItemScroll))
            {
                ItemScroll item = (ItemScroll)bag[num];
                item.Consumir();
                bag[num] = null;
                control = true;
            }
            else
            {
                Program.buffer.InsertText(bag[num].GetName() + " no se puede consumir");
            }
            Item.Ordenar(bag);
            return control;
        }
    }
}
