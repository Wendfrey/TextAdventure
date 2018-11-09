using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextAdventure.Comandos;

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
                    float rand = (float)CustomMath.RandomUnit();
                    if (rand <= ene.GetAccuracy())
                    {
                        int danorecibido = pl.ReceiveDamage(ene.GetAtt(), (int)(pl.GetDef() * 1.5));
                        buffer.InsertText(ene.GetName() + " te ha hecho " + danorecibido + " de daño");
                    }
                    else
                    {
                        buffer.InsertText("¡" + ene.GetName() + " ha fallado!");
                    }
                }
                else if (decide.Equals(comandos[2]))
                {
                    float rand = (float)CustomMath.RandomUnit();
                    float probHuida = (pl.GetMaldicion(3)) ? pl.GetAccuracy() / 2f : pl.GetAccuracy();
                    if (rand < probHuida)
                    {
                        buffer.InsertText("¡Has huido!");
                        huir = true;
                    }
                    else
                    {
                        if (pl.GetMaldicion(3) && rand < pl.GetAccuracy())
                        {
                            buffer.InsertText("Notas como las piernas no te responden y eres incapaz de moverte");
                        }
                        else
                        {
                            buffer.InsertText("¡No has podido huir!");
                            rand = (float)CustomMath.RandomUnit();
                            if (rand <= ene.GetAccuracy())
                            {
                                int danorecibido = pl.ReceiveDamage(ene.GetAtt(), (int)(pl.GetDef() * 1.5));
                                buffer.InsertText(ene.GetName() + " te ha hecho " + danorecibido + " de daño");
                            }
                            else
                            {
                                buffer.InsertText("¡" + ene.GetName() + " ha fallado!");
                            }
                        }
                    }
                }
                else if (decide.Equals(comandos[3]))
                {
                    if (Comando.ConsumeItem())
                    {
                        int danorecibido = pl.ReceiveDamage(ene.GetAtt(), pl.GetDef());
                        buffer.InsertText(ene.GetName() + " te ha hecho " + danorecibido + " de daño");
                    }
                }
                else if (decide.Equals(comandos[4]))
                {
                    buffer.InsertText("¿Que habilidad quieres usar?");
                    buffer.InsertText("[0]-> Ataque Veloz [1]-> Golpe Aplastador");
                    buffer.Print(1, buffer.height - 2, ">");
                    BackgroundCombat();
                    Console.SetCursorPosition(2, Program.buffer.height - 2);
                    bool obj = int.TryParse(Console.ReadLine(), out int num);
                    if (!obj)
                    {
                        Program.buffer.InsertText("Solo acepta numeros");
                    }

                    if (num >= 0 && num <= 1)
                    {
                        if(num == 0)
                            if (pl.GetMana() - 10 >= 0)
                            {
                                pl.SetMana(pl.GetMana() - 10);
                                Attack(pl, ene, 1);
                            }
                            else
                            {
                                buffer.InsertText("No tienes suficiente maná");
                            }
                        else if(num == 1)
                            if (pl.GetMana() - 5 >= 0)
                            {
                                pl.SetMana(pl.GetMana() - 5);
                                Attack(pl, ene, 2);
                            }
                            else
                            {
                                buffer.InsertText("No tienes suficiente maná");
                            }
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
                buffer.InsertText("¡Has derrotado a " + ene.GetName() + ", has conseguido " + exp + " xp!");
                pl.Experiencia += exp;
                if (pl.levelUp)
                {
                    pl.levelUp = false;
                    buffer.InsertText("¡HAS SUBIDO DE NIVEL!");
                    buffer.InsertText("Nivel: " + (pl.GetLevel() - 1) + " >> " + pl.GetLevel());
                }
                pl.currentRoom.ene = null;
            }
            buffer.InsertText("pulsa cualquier boton para continuar");
            BackgroundCombat();
            if (huir)
            {
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
            }
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
            buffer.Print(101, 9, "Acc -> " + pl.GetAccuracy());
            buffer.Print(101, 11, "Maná -> " + pl.GetMana() + "/" + pl.GetManaM());
            buffer.Print(101, 13, "Vel -> " + pl.GetSpeed());
            buffer.PrintScreen();
        }

        private static void Attack(Player pl, Enemigo ene, int hab = 0)
        {
            if (pl.GetSpeed() > ene.GetSpeed())
            {
                //Jugador Ataca
                NombreHabilidad(hab);
                if (hab == 1)
                {
                    AtaqueDirigidoA(pl, ene, hab);
                }
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
                if (hab == 1)
                {
                    AtaqueDirigidoA(pl, ene, hab);
                }
                AtaqueDirigidoA(pl, ene, hab);
            }
            else
            {
                if (CustomMath.RandomUnit() < 0.5)
                {
                    //Jugador Ataca
                    NombreHabilidad(hab);
                    if (hab == 1)
                    {
                        AtaqueDirigidoA(pl, ene, hab);
                    }
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
                    if (hab == 1)
                    {
                        AtaqueDirigidoA(pl, ene, hab);
                    }
                    AtaqueDirigidoA(pl, ene, hab);
                }
            }
        }

        private static void NombreHabilidad(int hab)
        {
            if (hab == 1)
            {
                buffer.InsertText("¡Has usado Ataque Veloz!");
            }
            else if(hab == 2)
            {
                buffer.InsertText("¡Has usado Golpe Aplastador!");
            }
        }

        private static void AtaqueDirigidoA(CombatClass atacante, CombatClass defensor,int hab = 0)
        {
            if (!atacante.IsDead())
            {
                float rand = (float)CustomMath.RandomUnit();
                if (rand <= atacante.GetAccuracy())
                {
                    int ataque = atacante.GetAtt();
                    int defensa = defensor.GetDef();
                    if (hab == 2)
                        ataque = (int)(ataque * 1.5);
                    int danorecibido = defensor.ReceiveDamage(ataque, defensa);
                    try
                    {
                        if (atacante.GetType() == typeof(Player))
                            buffer.InsertText("Has hecho " + danorecibido + " de daño a " + ((Enemigo)defensor).GetName());
                        else
                            buffer.InsertText(((Enemigo)atacante).GetName() + " te ha hecho " + danorecibido + " de daño");
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
