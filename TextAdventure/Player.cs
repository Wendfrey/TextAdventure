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
        int excesoMaldito = 0;
        int money;

        protected override float hitPerc
        {
            get
            {
                if (arma == null)
                    return (GetAtt() / 5) * 0.01f;
                else
                    return arma.GetHitPercFloat()+(GetAtt()/5)*0.01f;
            }
        }

        protected override float avoidPerc
        {
            get
            {
                if (armadura == null)
                    return 0;
                else
                    return armadura.GetAvoidPercFloat();
            }
        }

        public int ExcesoMaldito
        {
            get
            {
                return excesoMaldito;
            }
            set
            {
                excesoMaldito = value;
                if (hp > GetMHealth())
                    hp = GetMHealth();
            }
        }

        public Player()
        {
            for (int i = 0; i < 5; i++)
                bag[i] = new ItemPocion("Poción de vida", 50, ItemPocion.PocionType.hp);
            armadura = new ItemArmor("Armadura Simple", 5,3,45);

            hpM = 24;
            hp = GetMHealth();
            att = 7;
            def = 5;
            speed = 5;
            mana = 10;
            manaM = 10;
            money = 0;
        }

        public void GainMoney(int gain)
        {
            money += gain;
        }

        public void SpendMoney(int spent)
        {
            money -= spent;
        }

        public bool EnoughMoney(int cost)
        {
            return !(cost > money);
        }

        public int CurrentMoney()
        {
            return money;
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
            int suma = 0;
            for (int i = 0; i < gemas.Length; i++)
            {
                if (gemas[i] != null)
                    suma += gemas[i].ModifierAtt();
            }
            return suma;
        }

        public int GetGemsHp()
        {
            int suma = 0;
            for (int i = 0; i < gemas.Length; i++)
            {
                if (gemas[i] != null)
                    suma += gemas[i].ModifierHp();
            }
            return suma;
        }

        public int GetGemsDef()
        {
            int suma = 0;
            for (int i = 0; i < gemas.Length; i++)
            {
                if (gemas[i] != null)
                    suma += gemas[i].ModifierDef();
            }
            return suma;
        }
        public int GetGemsMana()
        {
            int suma = 0;
            for (int i = 0; i < gemas.Length; i++)
            {
                if (gemas[i] != null)
                    suma += gemas[i].ModifierManaM();
            }
            return suma;
        }

        public int GetGemsAttM()
        {
            int suma = 0;
            for (int i = 0; i < gemas.Length; i++)
            {
                if (gemas[i] != null)
                    suma += gemas[i].ModifierAttM();
            }
            return suma;
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
                ExcesoMaldito++;
                Program.buffer.InsertText("Has perdido parte de tu vida maxima");
                if (GetMHealth() <= 0)
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

        public override int GetMHealth()
        {
            int suma = 0;
            if(arma != null)
            {
                suma += arma.ModifierHp();
            }
            if(armadura != null)
            {
                suma += armadura.ModifierHp();
            }
            suma += GetGemsHp();
            return (base.GetMHealth() + suma) * (10 - excesoMaldito) / 10;
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

        public void SetAtt(int attn)
        {
            att = attn;
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

        public void SetDef(int defn)
        {
            def = defn;
        }

        public override int GetAttMa()
        {
            int suma = 0;
            if (arma != null)
                suma += arma.ModifierAttM();
            if (armadura != null)
                suma += armadura.ModifierAttM();
            suma += GetGemsAttM();
            return base.GetAttMa()+suma;
        }

        public void RestoreHealth()
        {
            hp = GetMHealth();
        }

        public void RestoreHealth(int cantidad)
        {
            hp = (hp + cantidad > GetMHealth()) ? GetMHealth() : hp + cantidad;
        }

        public void SetMaxHealth(int mHealth)
        {
            hp -= hpM;
            hpM = mHealth;
            hp += hpM;
            if (hp <= 0)
            {
                hp = 1;
            }
        }

        public int GetTotalHealth()
        {
            return hpM;
        }

        public override int GetManaM()
        {
            int suma = 0;
            if (arma != null)
                suma += arma.ModifierManaM();
            if (armadura != null)
                suma += armadura.ModifierManaM();
            suma += GetGemsMana();

            return base.GetManaM()+suma;
        }

        public void RestoreMana()
        {
            mana = GetManaM();
        }

        public void RestoreMana(int cantidad)
        {
            mana = (mana + cantidad > GetManaM()) ? GetManaM() : mana + cantidad;
        }

        public void SetSpeed(int spN)
        {
            speed = spN;
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
            if (bag[num].GetType().BaseType == typeof(ItemConsumable))
            {
                if (bag[num].GetType() == typeof(ItemPocion))
                {
                    ItemPocion item = (ItemPocion)bag[num];
                    item.Consumir();
                    if (item.GetPocionType() == ItemPocion.PocionType.hp)
                    {
                        if (hp < GetMHealth())
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
                if (!control)
                    throw new NotImplementedException("Error con el objeto de tipo " + bag[num].GetType().Name);
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
