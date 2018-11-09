using System;
using System.Collections.Generic;
using TextAdventure.Rooms;

namespace TextAdventure.Comandos
{
    static class Comando
    {
        private static readonly string[] validCommands = { "oeste", "este", "norte", "sur", "map", "exit", "help", "stats", "sala", "coger", "soltar", "clear", "bajar", "mochila", "consumir", "equipo", "equipar", "desequipar", "rezar" };
        

        public static bool CheckCommand(string com)
        {
            foreach (string s in validCommands)
            {
                if (s.Equals(com.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public static Program.CommandMethod ReturnCommand(string command)
        {
            Program.CommandMethod dd = null;
            switch (command.ToLower())
            {
                case "oeste":
                    dd = MoveWest;
                    if (Program.pl.GetMaldicion(2) && CustomMath.RandomUnit() <0.05)
                        dd = MoveEast;
                    break;
                case "este":
                    dd = MoveEast;
                    if (Program.pl.GetMaldicion(2) && CustomMath.RandomUnit() < 0.05)
                        dd = MoveWest;
                    break;
                case "norte":
                    dd =  MoveNorth;
                    if (Program.pl.GetMaldicion(2) && CustomMath.RandomUnit() < 0.05)
                        dd = MoveSouth;
                    break;
                case "sur":
                    dd =  MoveSouth;
                    if (Program.pl.GetMaldicion(2) && CustomMath.RandomUnit() < 0.05)
                        dd = MoveNorth;
                    break;
                case "map":
                    dd = Program.DrawMap;
                    break;
                case "help":
                    dd = GetHelp;
                    break;
                case "stats":
                    dd = GetStats;
                    break;
                case "clear":
                    dd = ClearConsole;
                    break;
                case "sala":
                    dd = GetRoomDescr;
                    break;
                case "coger":
                    dd = PickItemFromRoom;
                    break;
                case "soltar":
                    dd = PlayerDropItem;
                    break;
                case "bajar":
                    dd = NextLevel;
                    break;
                case "mochila":
                    dd = LookAtBag;
                    break;
                case "consumir":
                    dd = ConsumeItem;
                    break;
                case "equipo":
                    dd = VerEquipo;
                    break;
                case "equipar":
                    dd = EquiparItem;
                    break;
                case "rezar":
                    dd = Rezar;
                    break;
                case "desequipar":
                    dd = Desequipar;
                    break;
            }
            return dd;
        }

        public static bool GetHelp()
        {
            Program.buffer.InsertText("Lista de comandos:");
            Program.buffer.InsertText("'help'           'mochila'");
            Program.buffer.InsertText("'oeste'          'este'");
            Program.buffer.InsertText("'norte'          'sur'");
            Program.buffer.InsertText("'map'            'stats'");
            Program.buffer.InsertText("'clear'          'sala'");
            Program.buffer.InsertText("'coger'          'soltar'");
            Program.buffer.InsertText("'equipar'        'equipo'");
            Program.buffer.InsertText("'desequipar'     'consumir'");
            return true;
        }

        public static bool MoveNorth()
        {
            Room temp = Program.pl.currentRoom.GetNorthRoom();
            if (temp != null)
            {
                if (temp.GetType() == typeof(RoomClosed) && ((RoomClosed)temp).IsClosed())
                {
                    if (RoomClosed.UseKey())
                    {
                        ((RoomClosed)temp).OpenRoom();
                        if (Program.pl.GetMaldicion(4))
                            Program.pl.currentRoom.SetVisible(0);
                        Program.pl.currentRoom = temp;
                        Program.buffer.InsertText("Vas al norte");
                        Program.pl.lastRoom = 2;
                        return true;
                    }
                    else
                    {
                        if(!Program.pl.GetMaldicion(4))
                            temp.SetVisible(1);
                        return false;
                    }
                }
                else
                {
                    if (Program.pl.GetMaldicion(4))
                        Program.pl.currentRoom.SetVisible(0);
                    Program.pl.currentRoom = temp;
                    Program.buffer.InsertText("Vas al norte");
                    Program.pl.lastRoom = 2;
                    return true;
                }
            }
            else
            {
                Program.buffer.InsertText("No hay habitación al norte");
                return false;
            }
        }

        public static bool MoveWest()
        {
            Room temp = Program.pl.currentRoom.GetWestRoom();
            if (temp != null)
            {
                if (temp.GetType() == typeof(RoomClosed) && ((RoomClosed)temp).IsClosed())
                {
                    if (RoomClosed.UseKey())
                    {
                        ((RoomClosed)temp).OpenRoom();
                        if (Program.pl.GetMaldicion(4))
                            Program.pl.currentRoom.SetVisible(0);
                        Program.pl.currentRoom = temp;
                        Program.buffer.InsertText("Vas al oeste");
                        Program.pl.lastRoom = 3;
                        return true;
                    }
                    else
                    {
                        if (!Program.pl.GetMaldicion(4))
                            temp.SetVisible(1);
                        return false;
                    }
                }
                else
                {
                    if (Program.pl.GetMaldicion(4))
                        Program.pl.currentRoom.SetVisible(0);
                    Program.pl.currentRoom = temp;
                    Program.buffer.InsertText("Vas al oeste");
                    Program.pl.lastRoom = 3;
                    return true;
                }
            }
            else
            {
                Program.buffer.InsertText("No hay habitación al oeste");
                return false;
            }
        }

        public static bool MoveSouth()
        {
            Room temp = Program.pl.currentRoom.GetSouthRoom();
            if (temp != null)
            {
                if (temp.GetType() == typeof(RoomClosed) && ((RoomClosed)temp).IsClosed())
                {
                    if (RoomClosed.UseKey())
                    {
                        ((RoomClosed)temp).OpenRoom();
                        if (Program.pl.GetMaldicion(4))
                            Program.pl.currentRoom.SetVisible(0);
                        Program.pl.currentRoom = temp;
                        Program.buffer.InsertText("Vas al sur");
                        Program.pl.lastRoom = 0;
                        return true;
                    }
                    else
                    {
                        if (!Program.pl.GetMaldicion(4))
                            temp.SetVisible(1);
                        return false;
                    }
                }
                else
                {
                    if (Program.pl.GetMaldicion(4))
                        Program.pl.currentRoom.SetVisible(0);
                    Program.pl.currentRoom = temp;
                    Program.buffer.InsertText("Vas al sur");
                    Program.pl.lastRoom = 0;
                    return true;
                }
            }
            else
            {
                Program.buffer.InsertText("No hay habitación al sur");
                return false;
            }
        }

        public static bool MoveEast()
        {
            Room temp = Program.pl.currentRoom.GetEastRoom();
            if (temp != null)
            {
                if(temp.GetType() == typeof(RoomClosed) && ((RoomClosed)temp).IsClosed())
                {
                    if (RoomClosed.UseKey())
                    {
                        ((RoomClosed)temp).OpenRoom();
                        if (Program.pl.GetMaldicion(4))
                            Program.pl.currentRoom.SetVisible(0);
                        Program.pl.currentRoom = temp;
                        Program.buffer.InsertText("Vas al este");
                        Program.pl.lastRoom = 1;
                        return true;
                    }
                    else
                    {
                        if (!Program.pl.GetMaldicion(4))
                            temp.SetVisible(1);
                        return false;
                    }
                }
                else
                {
                    if (Program.pl.GetMaldicion(4))
                        Program.pl.currentRoom.SetVisible(0);
                    Program.pl.currentRoom = temp;
                    Program.buffer.InsertText("Vas al este");
                    Program.pl.lastRoom = 1;
                    return true;
                }
            }
            else
            {
                Program.buffer.InsertText("No hay habitación al este");
                return false;
            }
        }

        public static bool GetStats()
        {
            Program.buffer.PrintBackground();
            Item[] bagitem = Program.pl.GetGemas();
            int modh = 0;
            int moda = 0;
            int modd = 0;
            float modac = 0;

            for (int i = 0; i < bagitem.Length; i++)
            {
                if(bagitem[i] != null && bagitem[i].GetType() == typeof(ItemGema))
                {
                    ItemGema gema = (ItemGema)bagitem[i];
                    modh += gema.ModifierHp();
                    moda += gema.ModifierAtt();
                    modd += gema.ModifierDef();
                    modac += gema.ModifierAcc();
                }
            }
            ItemWeapon weapon = Program.pl.GetWeapon();
            if(weapon != null)
            {
                modh += weapon.ModifierHp();
                moda += weapon.ModifierAtt();
                modd += weapon.ModifierDef();
                modac += weapon.ModifierAcc();
            }
            Program.buffer.Print(1, 0, "STATS");

            Program.buffer.Print(2, 3, "Nvl. " + Program.pl.GetLevel()+"  Exp. "+Program.pl.Experiencia);

            if(modh == 0)
                Program.buffer.Print(2, 5, "VIDA (HP)-> " + Program.pl.GetHealth() + "/" + Program.pl.GetMHealth() + " --> Capacidad de aguante");
            else
                Program.buffer.Print(2, 5, "VIDA (HP)-> " + Program.pl.GetHealth() + "/" + Program.pl.GetMHealth() + "+(" + modh+ ") --> Capacidad de aguante");

            if (moda == 0)
                Program.buffer.Print(2, 7, "ATAQUE (Att)-> " + Program.pl.GetFlatAtt() + " --> Daño que inflinges");
            else
                Program.buffer.Print(2, 7, "ATAQUE (Att)-> " + Program.pl.GetFlatAtt() + "+(" + moda + ") --> Daño que inflinges");

            if(modd == 0)
                Program.buffer.Print(2, 9, "DEFENSA (Def)-> " + Program.pl.GetFlatDef() + " --> Daño que reduces");
            else
                Program.buffer.Print(2, 9, "DEFENSA (Def)-> " + Program.pl.GetFlatDef() + "+(" + modd + ") --> Daño que reduces");

            if(modac == 0)
                Program.buffer.Print(2, 11, "PRECISIÓN (Acc) -> " + Program.pl.GetFlatAccuracy() + " --> Probabilidad de dar");
            else
                Program.buffer.Print(2, 11, "PRECISIÓN (Acc) -> " + Program.pl.GetFlatAccuracy() + "+(" + modac + ") --> Probabilidad de dar");

            Program.buffer.Print(2, 13, "MANA (mana) -> " + Program.pl.GetMana() + "/" + Program.pl.GetManaM());

            Program.buffer.Print(2, 15, "Velocidad (Vel.) -> " + Program.pl.GetSpeed());

            Program.buffer.PrintScreen();
            Console.ReadKey();
            return true;
        }

        public static bool ClearConsole() {
            Program.buffer.ClearBox();
            Console.Clear();
            Program.buffer.InsertText(Program.pl.currentRoom.GetDescriptionTotal());
            return true;
        }

        public static bool GetRoomDescr()
        {
            Program.buffer.InsertText(Program.pl.currentRoom.GetDescriptionTotal());
            return true;
        }

        public static bool LookAtBag()
        {
            Item[] bag = Program.pl.GetBag();
            for(int i = 0; i<bag.Length; i++)
            {
                int ii = i;
                int x = 0;
                if (ii >= 5)
                {
                    ii -= 5;
                    x = 1;
                }
                if(bag[i] != null)
                {
                    if (bag[i].GetType().BaseType == typeof(ItemEquipable))
                    {
                        ItemEquipable equipo = (ItemEquipable)bag[i];
                        Program.buffer.Print(1 + 50*x, 2 + ii * 3, equipo.GetName());
                        string texto = "";
                        if (equipo.ModifierHp() < 0)
                            texto += "HP(" + equipo.ModifierHp() + ") ";
                        else
                            texto += "HP(+" + equipo.ModifierHp() + ") ";

                        if (equipo.ModifierAtt() < 0)
                            texto += "ATT(" + equipo.ModifierAtt() + ") ";
                        else
                            texto += "ATT(+" + equipo.ModifierAtt() + ") ";

                        if (equipo.ModifierDef() < 0)
                            texto += "DEF(" + equipo.ModifierDef() + ") ";
                        else
                            texto += "DEF(+" + equipo.ModifierDef() + ") ";

                        if (equipo.ModifierAcc() < 0)
                            texto += "ACC(" + equipo.ModifierAcc() + ") ";
                        else
                            texto += "ACC(+" + equipo.ModifierAcc() + ") ";
                        Program.buffer.Print(5 + 50 * x, 3 + ii * 3, texto);
                    }
                    else if(bag[i].GetType() == typeof(ItemPocion))
                    {
                        ItemPocion consumable = (ItemPocion)bag[i];
                        Program.buffer.Print(1+50*x, 2 + ii * 3, consumable.GetName());
                        if(consumable.GetPocionType() == ItemPocion.PocionType.hp)
                            Program.buffer.Print(1+50*x, 3 + ii * 3, "    +"+consumable.GetFlatCant().ToString()+"% HP");
                        else
                            Program.buffer.Print(1 + 50 * x, 3 + ii * 3, "    +" + consumable.GetFlatCant().ToString() + "% Mana");


                    }
                    else
                    {
                        Program.buffer.Print(1 + 50 * x, 2 + ii * 3, bag[i].GetName());
                    }
                }
            }
            Program.buffer.PrintBackground();
            Program.buffer.Print(1, Program.buffer.height - 2, "Pulsa cualquier boton para salir");
            Program.buffer.Print(1, 0, "MOCHILA");
            Program.SmallMap();
            Program.buffer.PrintScreen();
            Console.ReadKey();
            return true;
        }

        public static bool PickItemFromRoom()
        {
            Player pl = Program.pl;
            if (pl.currentRoom.RoomHasItem())
            {
                Program.buffer.InsertText("¿Que objeto quieres coger?");
                Item[] tempRoomItems = pl.currentRoom.GetRoomItems();
                pl.currentRoom.ListOfItems();
                Program.buffer.PrintBackground();
                Program.buffer.PrintText(Program.buffer.height - 3);
                Program.buffer.Print(1, 0, "PRINCIPAL");
                Program.buffer.Print(1, Program.buffer.height - 2, ">");
                Program.SmallMap();
                Program.buffer.PrintScreen();
                Console.SetCursorPosition(2, Program.buffer.height - 2);
                bool obj = int.TryParse(Console.ReadLine(), out int num);

                if (!obj)
                {
                    Program.buffer.InsertText("Solo acepta numeros");
                    return false;
                }

                if (num >= 0 && num <= tempRoomItems.Length - 1 && tempRoomItems[num] != null)
                {
                    pl.PickItem(num);
                    Item.Ordenar(tempRoomItems);
                    return true;
                }
                else
                {
                    Program.buffer.InsertText("Ese número no es válido");
                }
            }
            Program.buffer.InsertText("No hay nada que coger");
            return false;
        }
        
        public static bool PlayerDropItem()
        {
            //Check
            Item[] bag = Program.pl.GetBag();
            bool empty = true;
            for(int i = 0; i < bag.Length; i++)
            {
                if(bag[i] != null)
                {
                    empty = false;
                }
            }
            if (empty)
            {
                Program.buffer.InsertText("No tienes objetos equipados");
                return false;
            }
            Program.buffer.InsertText("Que objeto quieres soltar?");
            Program.pl.ListOfItems();
            Program.buffer.PrintBackground();
            Program.buffer.PrintText(Program.buffer.height - 3);
            Program.buffer.Print(1, 0, "PRINCIPAL");
            Program.buffer.Print(1, Program.buffer.height - 2, ">");
            Program.SmallMap();
            Program.buffer.PrintScreen();
            Console.SetCursorPosition(2, Program.buffer.height - 2);
            int num;
            bool obj = int.TryParse(Console.ReadLine(), out num);
            if (obj && num >= 0 && num < bag.Length && bag[num] != null)
            {
                Program.pl.DropItem(num);
                Item.Ordenar(Program.pl.GetBag());
                return true;
            }
            else if(!obj)
            {
                Program.buffer.InsertText("Tiene que ser un numero");
            }
            else
            {
                Program.buffer.InsertText("Esa posicion no es válida");
            }
            return false;
        }

        public static bool NextLevel()
        {
            Room c = Program.pl.currentRoom;
            if (c.GetType() == typeof(RoomExit))
            {
                Program.goNextLevel = true;
                return true;
            }
            else
            {
                Program.buffer.InsertText("No hay ninguna escalera por bajar");
                return false;
            }
        }

        public static bool ConsumeItem()
        {
            Item[] bag = Program.pl.GetBag();
            Program.buffer.InsertText("Que objeto quieres consumir?");
            Program.pl.ListOfItems();
            Program.buffer.PrintBackground();
            Program.buffer.PrintText(Program.buffer.height - 3);
            Program.buffer.Print(1, 0, "PRINCIPAL");
            Program.buffer.Print(1, Program.buffer.height - 2, ">");
            Program.SmallMap();
            Program.buffer.PrintScreen();
            Console.SetCursorPosition(2, Program.buffer.height - 2);
            int num;
            bool obj = int.TryParse(Console.ReadLine(), out num);
            if (obj && num >= 0 && num < bag.Length && bag[num] != null)
            {
                Program.pl.ConsumeItem(num);
                return true;
            }
            else if (!obj)
            {
                Program.buffer.InsertText("Tiene que ser un numero");
            }
            else
            {
                Program.buffer.InsertText("Esa posicion no es válida");
            }
            return false;
        }

        public static bool VerEquipo()
        {
            Program.buffer.PrintBackground();
            Program.buffer.Print(1, 0, "EQUIPO");
            Program.buffer.Print(1, 4, "ARMA");
            ItemWeapon weapon = Program.pl.GetWeapon();
            if (weapon != null)
            {
                Program.buffer.Print(7, 5, weapon.GetName());
                Program.buffer.Print(7, 6, "HP-> " + weapon.ModifierHp());
                Program.buffer.Print(7, 7, "ATT-> " + weapon.ModifierAtt());
                Program.buffer.Print(7, 8, "DEF-> " + weapon.ModifierDef());
                Program.buffer.Print(7, 9, "ACC-> " + weapon.ModifierAcc());
            }
            else
            {
                Program.buffer.Print(7, 5, "Ninguna");
            }

            Program.buffer.Print(101, 3, "Hp  -> " + Program.pl.GetHealth() + "/" + Program.pl.GetMHealth());
            Program.buffer.Print(101, 5, "Att -> " + Program.pl.GetAtt());
            Program.buffer.Print(101, 7, "Def -> " + Program.pl.GetDef());
            Program.buffer.Print(101, 9, "Acc -> " + Program.pl.GetAccuracy());
            Program.buffer.Print(101, 11, "Mana -> " + Program.pl.GetMana() + "/" + Program.pl.GetManaM());
            Program.buffer.Print(101, 13, "Vel. -> " + Program.pl.GetSpeed());

            Program.buffer.Print(51, 4, "GEMAS");

            ItemGema[] gemas = Program.pl.GetGemas();
            bool check = true;
            for(int i = 0; i< gemas.Length; i++)
            {
                if (gemas[i] != null)
                {
                    check = false;
                    if (i < 2)
                    {
                        Program.buffer.Print(55, 5 + i * 7, i.ToString()+":");
                        Program.buffer.Print(57, 6 + i * 7, gemas[i].GetName());
                        Program.buffer.Print(57, 7 + i * 7, "HP-> " + gemas[i].ModifierHp());
                        Program.buffer.Print(57, 8 + i * 7, "ATT-> " + gemas[i].ModifierAtt());
                        Program.buffer.Print(57, 9 + i * 7, "DEF-> " + gemas[i].ModifierDef());
                        Program.buffer.Print(57, 10 + i * 7, "ACC-> " + gemas[i].ModifierAcc());
                    }
                    else
                    {
                        Program.buffer.Print(78, 5, i.ToString() + ":");
                        Program.buffer.Print(80, 6, gemas[i].GetName());
                        Program.buffer.Print(80, 7, "HP-> " + gemas[i].ModifierHp());
                        Program.buffer.Print(80, 8, "ATT-> " + gemas[i].ModifierAtt());
                        Program.buffer.Print(80, 9, "DEF-> " + gemas[i].ModifierDef());
                        Program.buffer.Print(80, 10, "ACC-> " + gemas[i].ModifierAcc());
                    }
                }
            }

            if (check)
            {
                Program.buffer.Print(57, 5, "No tienes gemas");
            }

            Program.buffer.Print(1, Program.buffer.height - 2, "Pulsa cualquier tecla para salir");
            Program.buffer.PrintScreen();
            Console.ReadKey();
            return true;
        }

        public static bool EquiparItem()
        {
            Item[] bag = Program.pl.GetBag();
            Program.buffer.InsertText("¿Que objeto quieres equiparte?");
            Program.pl.ListOfItems();
            Program.buffer.PrintBackground();
            Program.buffer.PrintText(Program.buffer.height - 3);
            Program.buffer.Print(1, 0, "PRINCIPAL");
            Program.buffer.Print(1, Program.buffer.height - 2, ">");
            Program.SmallMap();
            Program.buffer.PrintScreen();
            Console.SetCursorPosition(2, Program.buffer.height - 2);
            int num;
            bool obj = int.TryParse(Console.ReadLine(), out num);
            if (obj && num >= 0 && num < bag.Length && bag[num] != null)
            {
                Program.pl.EquipItem(num);
                return true;
            }
            else if (!obj)
            {
                Program.buffer.InsertText("Tiene que ser un numero");
            }
            else
            {
                Program.buffer.InsertText("Esa posicion no es válida");
            }
            return true;
        }
        public static bool Desequipar()
        {
            if (Program.pl.FilledBag())
            {
                Program.buffer.InsertText("Tienes la mochila llena");
                return false;
            }
            else
            {
                Program.buffer.InsertText("¿Que quieres desequiparte?");
                Program.buffer.InsertText("    >ARMA    >GEMA");
                Program.buffer.Print(1,Program.buffer.height-2,">");
                Program.buffer.PrintBackground();
                Program.buffer.PrintText(Program.buffer.height - 3);
                Program.buffer.PrintScreen();
                Console.SetCursorPosition(2, Program.buffer.height - 2);
                string tipo = Console.ReadLine().ToLower();
                if (tipo.Equals("arma"))
                {
                    if (Program.pl.GetWeapon() != null)
                    {
                        for (int i = 0; i < Program.pl.GetBag().Length; i++)
                        {
                            if (Program.pl.GetBag()[i] == null)
                            {
                                Item rr = Program.pl.DropWeapon();
                                Program.buffer.InsertText("Te has desequipado "+rr.GetName());
                                Program.pl.GetBag()[i] = rr;
                                i = Program.pl.GetBag().Length;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        Program.buffer.InsertText("No tienes arma equipada");
                        return false;
                    }
                }
                else if(tipo.Equals("gema"))
                {
                    if (Program.pl.EmptyGemas())
                    {
                        Program.buffer.InsertText("No tienes gemas equipadas");
                        return false;
                    }
                    else
                    {
                        Program.buffer.InsertText("¿Que gema quieres desequiparte?");
                        Program.pl.ListOfGems();
                        Program.buffer.Print(1, Program.buffer.height - 2, ">");
                        Program.buffer.PrintBackground();
                        Program.buffer.PrintText(Program.buffer.height - 3);
                        Program.buffer.PrintScreen();
                        Console.SetCursorPosition(2, Program.buffer.height - 2);
                        bool obj = int.TryParse(Console.ReadLine().ToLower(),out int gema);
                        if (obj && gema >= 0 && gema < Program.pl.GetGemas().Length && Program.pl.GetGemas()[gema] != null)
                        {
                            ItemGema r = Program.pl.GetGemas()[gema];
                            Program.pl.GetGemas()[gema] = null;
                            for (int i = 0; i < Program.pl.GetBag().Length; i++)
                            {
                                if (Program.pl.GetBag()[i] == null)
                                {
                                    Program.pl.GetBag()[i] = r;
                                    Program.buffer.InsertText("Te has desequipado '"+r.GetName()+"'");
                                    i = Program.pl.GetBag().Length;
                                }
                            }
                            return true;
                        }
                        else if(obj)
                        {
                            Program.buffer.InsertText("Tiene que ser un número");
                        }
                        else
                        {
                            Program.buffer.InsertText("El número no es válido");
                        }
                        return false;
                    }
                }
                else
                {
                    Program.buffer.InsertText("Comando no válido");
                    return false;
                }
            }
        }
        public static bool Rezar()
        {
            Program.buffer.InsertText("De rodillas, empiezas a rezar");
            if (Program.pl.currentRoom.GetType() == typeof(RoomBless))
            {
                RoomBless temp =(RoomBless) Program.pl.currentRoom;
                if (temp.IsBlessed())
                {
                    temp.Effect();
                    return true;
                }
            }
            Program.buffer.InsertText("No ha pasado nada");
            return false;
        }
    }
}
