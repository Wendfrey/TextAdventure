using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventure.Comandos;
// Aqui va el combate
namespace TextAdventure
{
    class EscenaCombate
    {
        static ConsoleBuffer buffer;
        static Player pl;

        private static void StartCombate()
        {
            buffer = Program.buffer;
            pl = Program.pl;
        }

        public static void Combate(Enemigo ene)
        {
            StartCombate();
            buffer.InsertText("¡Ha aparecido un " + ene.GetName() + "!");
            BackgroundCombat();
            string[] comandos = { "atacar", "defender", "huir", "consumir", "habilidad" };
            bool huir = false;
            int itemHuir = -1;
            while (!(pl.IsDead() || ene.IsDead() || huir))
            {
                Console.SetCursorPosition(1, 18);
                string decide = Console.ReadLine().ToLower();
                buffer.InsertText("");
                buffer.InsertText(decide);
                if (decide.Equals(comandos[0]))
                {
                    Attack(pl, ene);
                }
                else if (decide.Equals(comandos[1]))
                {
                    buffer.InsertText("Te has puesto en guardia");
                    AtaqueDirigidoA(ene, pl, -2);
                }
                else if (decide.Equals(comandos[2]))
                {
                    float rand = (float)CustomMath.RandomUnit();
                    float probHuida = (pl.GetMaldicion(3)) ? 0.25f+pl.GetSpeed()/(float)(2*(pl.GetSpeed() +ene.GetSpeed())) : 0.25f+pl.GetSpeed() / (float)(pl.GetSpeed() + ene.GetSpeed());
                    if (rand < probHuida)
                    {
                        buffer.InsertText("¡Has huido!");
                        huir = true;
                    }
                    else
                    {
                        if (pl.GetMaldicion(3) && rand < (probHuida-0.25f)*2+0.25f)
                        {
                            buffer.InsertText("Notas como las piernas no te responden y eres incapaz de moverte");
                        }
                        else
                        {
                            buffer.InsertText("¡No has podido huir!");
                            AtaqueDirigidoA(ene, pl);
                        }
                    }
                }
                else if (decide.Equals(comandos[3]))
                {
                    int i = Comando.ConsumeItemCombat();
                    if (i>-1)
                    {
                        if(pl.GetBag()[i].GetType() == typeof(ItemScroll))
                        {
                            ItemScroll itemScroll = (ItemScroll)pl.GetBag()[i];
                            if (itemScroll.GetId() == 1)
                            {
                                itemHuir = i;
                                huir = true;
                            }
                        }
                        if (itemHuir == 0)
                        {
                            AtaqueDirigidoA(ene, pl);
                        }
                    }
                }
                else if (decide.Equals(comandos[4]))
                {
                    buffer.InsertText("¿Que habilidad quieres usar?");
                    buffer.InsertText("[0]-> Ataque Veloz [1]-> Golpe Aplastador [2]-> Curación Menor [3]-> MegaGolpe [4]-> Dardos Mágicos");
                    buffer.Print(1, buffer.height - 2, ">");
                    BackgroundCombat();
                    Console.SetCursorPosition(2, Program.buffer.height - 2);
                    bool obj = int.TryParse(Console.ReadLine(), out int num);
                    if (!obj)
                    {
                        Program.buffer.InsertText("Solo acepta numeros");
                    }
                    else if (num >= 0 && num <= 4)
                    {
                        bool used = true;
                        if (num == 0 && pl.GetMana() - 10 >= 0)
                        {
                            pl.SetMana(pl.GetMana() - 10);
                        }
                        else if (num == 1 && pl.GetMana() - 5 >= 0)
                        {
                            pl.SetMana(pl.GetMana() - 5);
                        }
                        else if (num == 2 && pl.GetMana() - 5 >= 0)
                        {
                            pl.SetMana(pl.GetMana() - 5);
                        }
                        else if (num == 3 && pl.GetMana() - 10 >= 0)
                        {
                            pl.SetMana(pl.GetMana() - 10);
                        }
                        else if (num == 4 && pl.GetMana() - 10 >= 0)
                        {
                            pl.SetMana(pl.GetMana() - 10);
                        }
                        else
                        {
                            buffer.InsertText("No tienes suficiente maná");
                            used = false;
                        }
                        if (used)
                            Attack(pl, ene, num);
                    }
                    else
                    {
                        Program.buffer.InsertText("Ese número no es válido");
                    }
                }
                else
                {
                    buffer.InsertText("Comando no valido");
                }


                BackgroundCombat();
            }
            if (pl.IsDead())
            {
                buffer.InsertText("Has muerto...");
            }
            else if (ene.IsDead())
            {
                buffer.InsertText("");
                int exp = 5 + 3 * ene.GetLevel();
                buffer.InsertText("¡Has derrotado a " + ene.GetName()+"!");
                int money = CustomMath.RandomIntNumber(5 + 5 * Program.GetLevel(),5+2*Program.GetLevel());
                buffer.InsertText("Has conseguido " + money + " monedas");
                pl.GainMoney(money);
                pl.currentRoom.ene = null;
            }
            if (huir && itemHuir == -1)
            {
                ene.RestoreHealth();
                pl.currentRoom.SetVisible(1);
                switch (pl.lastRoom)
                {
                    case 0:
                        Comando.MoveNorth();
                        break;
                    case 1:
                        Comando.MoveWest();
                        break;
                    case 2:
                        Comando.MoveSouth();
                        break;
                    case 3:
                        Comando.MoveEast();
                        break;
                    default:
                        throw new Exception("Error a la hora de huir");
                }
            } else if (huir)
            {
                ene.RestoreHealth();
                pl.ConsumeItem(itemHuir);
            }
            buffer.InsertText("pulsa cualquier boton para continuar");
            BackgroundCombat();
            Console.ReadKey();
        }

        private static void BackgroundCombat()
        {
            buffer.PrintBackground();
            buffer.Print(1, 0, "COMBATE");
            buffer.Print(1, 17, "<<ATACAR -- DEFENDER -- HUIR -- HABILIDAD -- CONSUMIR>>");
            buffer.PrintText(16);
            buffer.Print(101, 3, "Hp  -> " + pl.GetHealth() + "/" + pl.GetMHealth());
            buffer.Print(101, 5, "Att -> " + pl.GetAtt());
            buffer.Print(101, 7, "Def -> " + pl.GetDef());
            buffer.Print(101, 9, "Att M. -> " + pl.GetAttMa());
            buffer.Print(101, 11, "Maná -> " + pl.GetMana() + "/" + pl.GetManaM());
            buffer.Print(101, 13, "Vel -> " + pl.GetSpeed());
            buffer.PrintScreen();
        }

        private static void Attack(Player pl, Enemigo ene, int hab = -1)
        {
            if (pl.GetSpeed() > ene.GetSpeed())
            {
                //Jugador Ataca
                NombreHabilidad(hab);
                if (hab == 0)
                {
                    AtaqueDirigidoA(pl, ene, hab);
                }
                else if(hab == 2)
                {
                    int rstH = 5 + pl.GetAttMa();
                    pl.RestoreHealth(rstH);
                    if (pl.GetHealth() == pl.GetMHealth())
                    {
                        buffer.InsertText("¡Te has recuperado al máximo!");
                    }
                }
                else if(hab == 4)
                {
                    AtaqueDirigidoA(pl, ene, hab);
                    AtaqueDirigidoA(pl, ene, hab);
                }
                if(hab != 2)
                    AtaqueDirigidoA(pl, ene, hab);
                //Enemigo Ataca
                AtaqueDirigidoA(ene, pl);
            }
            else if (pl.GetSpeed() < ene.GetSpeed())
            {
                //Enemigo Ataca
                AtaqueDirigidoA(ene, pl);
                //Jugador Ataca
                NombreHabilidad(hab);
                if (hab == 0)
                {
                    AtaqueDirigidoA(pl, ene, hab);
                }
                else if (hab == 2)
                {
                    int rstH = 5 + pl.GetAttMa();
                    pl.RestoreHealth(rstH);
                    if (pl.GetHealth() == pl.GetMHealth())
                    {
                        buffer.InsertText("¡Te has recuperado al máximo!");
                    }
                }
                else if (hab == 4)
                {
                    AtaqueDirigidoA(pl, ene, hab);
                    AtaqueDirigidoA(pl, ene, hab);
                }
                if (hab != 2)
                    AtaqueDirigidoA(pl, ene, hab);
            }
            else
            {
                if (CustomMath.RandomUnit() < 0.5)
                {
                    //Jugador Ataca
                    NombreHabilidad(hab);
                    if (hab == 0)
                    {
                        AtaqueDirigidoA(pl, ene, hab);
                    }
                    else if (hab == 2)
                    {
                        int rstH = 5 + pl.GetAttMa();
                        pl.RestoreHealth(rstH);
                        if (pl.GetHealth() == pl.GetMHealth())
                        {
                            buffer.InsertText("¡Te has recuperado al máximo!");
                        }
                    }
                    else if (hab == 4)
                    {
                        AtaqueDirigidoA(pl, ene, hab);
                        AtaqueDirigidoA(pl, ene, hab);
                    }
                    if (hab != 2)
                        AtaqueDirigidoA(pl, ene, hab);
                    //Enemigo Ataca
                    AtaqueDirigidoA(ene, pl);
                }
                else
                {
                    //Enemigo Ataca
                    AtaqueDirigidoA(ene, pl);
                    //Jugador Ataca
                    NombreHabilidad(hab);
                    if (hab == 0)
                    {
                        AtaqueDirigidoA(pl, ene, hab);
                    }
                    else if (hab == 2)
                    {
                        int rstH = 5 + pl.GetAttMa();
                        pl.RestoreHealth(rstH);
                        if (pl.GetHealth() == pl.GetMHealth())
                        {
                            buffer.InsertText("¡Te has recuperado al máximo!");
                        }
                    }
                    else if (hab == 4)
                    {
                        AtaqueDirigidoA(pl, ene, hab);
                        AtaqueDirigidoA(pl, ene, hab);
                    }
                    if (hab != 2)
                        AtaqueDirigidoA(pl, ene, hab);
                }
            }
        }

        private static void NombreHabilidad(int hab)
        {
            if (hab == 0)
            {
                buffer.InsertText("¡Has usado Ataque Veloz!");
            }
            else if(hab == 1)
            {
                buffer.InsertText("¡Has usado Golpe Aplastador!");
            }
            else if(hab == 2)
            {
                buffer.InsertText("¡Has usado Curación Menor!");
            }
            else if (hab == 3)
            {
                buffer.InsertText("¡Has usado MegaGolpe!");
            }
            else if (hab == 4)
            {
                buffer.InsertText("¡Has usado Dardos Mágicos!");
            }
}

        private static void AtaqueDirigidoA(CombatClass atacante, CombatClass defensor,int hab = -1)
        {
            if (!atacante.IsDead())
            {
                float rand = (float)CustomMath.RandomUnit();
                float acc = atacante.GetHitPerc()+rand;
                if (hab == 3)
                    acc /= 2;
                else if(hab == 4)
                {
                    acc = 1;
                }
                if (acc >= defensor.GetAvoidPerc())
                {
                    int ataque = atacante.GetAtt();
                    int defensa = defensor.GetDef();
                    if (hab == -2)
                        defensa = (int)(defensa*1.5);
                    else if (hab == 1)
                        ataque = (int)(ataque * 1.5);
                    else if (hab == 3)
                        ataque = ataque * 3;
                    else if(hab == 4)
                    {
                        ataque = CustomMath.RandomIntNumber(3,1)+atacante.GetAttMa()/2;
                        defensa = 0;
                    }
                    int danorecibido = defensor.ReceiveDamage(ataque, defensa,(hab == 4));
                    try
                    {
                        if (atacante.GetType() == typeof(Player))
                            buffer.InsertText("Has hecho " + danorecibido + " de daño a " + ((Enemigo)defensor).GetName());
                        else
                        {
                            if (pl.GetMaldicion(5))
                            {
                                danorecibido = CustomMath.RandomIntNumber(10000, 5000);
                                buffer.InsertText(((Enemigo)atacante).GetName() + " te ha hecho " + danorecibido + " de daño");
                            }
                            else
                            {
                                buffer.InsertText(((Enemigo)atacante).GetName() + " te ha hecho " + danorecibido + " de daño");
                            }
                        }
                    }catch(InvalidCastException e)
                    {
                        buffer.InsertText("Error: " + e.Message);
                        buffer.InsertText("atacante: " + atacante.GetType().ToString());
                        buffer.InsertText("defensor: " + defensor.GetType().ToString());
                    }
                }
                else
                {
                    if (atacante.GetType() == typeof(Player))
                        buffer.InsertText("¡Has fallado!");
                    else
                        buffer.InsertText("¡" + ((Enemigo)atacante).GetName() + " ha fallado!");
                }
            }
        }
    }
}