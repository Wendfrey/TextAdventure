﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Rooms
{
    class RoomBless : Room
    {
        bool hasEffect = true;
        public RoomBless(int x, int y) : base(x, y)
        {
            descr = "La estatua de un angel de pie esta en el centro de la habitación, puedes <<rezar>>";
        }

        public bool IsBlessed()
        {
            return hasEffect;
        }

        public void Effect()
        {
            if (hasEffect)
            {
                do
                {
                    int efecto = CustomMath.RandomIntNumber(3);
                    Player pl = Program.pl;
                    if (efecto == 0)
                    {
                        List<int> r = new List<int>();
                        for (int i = 0; i < pl.GetArrMal().Length; i++)
                        {
                            if (pl.GetArrMal()[i] != null)
                            {
                                r.Add(i);
                            }
                        }
                        if (r.Count > 0)
                        {
                            Program.buffer.InsertText("Al rezar, sientes como tu cuerpo se siente mas ligero");
                            int temp = CustomMath.RandomIntNumber(r.Count - 1);
                            Program.buffer.InsertText("¡La " + pl.GetArrMal()[r[temp]].GetName() + " ha desaparecido!");
                            pl.GetArrMal()[r[temp]] = null;
                            hasEffect = false;
                        }
                    }
                    else if(efecto == 1)
                    {
                        Item item;

                        if (CustomMath.RandomUnit() < 0.9)
                        {
                            if (CustomMath.RandomUnit() < 0.5)
                            {
                                item = new ItemPocion("Poción de Vida", CustomMath.RandomIntNumber(75, 50), ItemPocion.PocionType.hp);
                            }
                            else
                            {
                                item = new ItemPocion("Poción de Mana", CustomMath.RandomIntNumber(75, 50), ItemPocion.PocionType.mana);
                            }
                        }
                        else
                            item = new ItemScroll("Pergamino de visión", 0);

                        if (!pl.FilledBag())
                        {
                            pl.GetItem(item);
                            Program.buffer.InsertText("Un objeto ha aparecido en tu mochila");
                            hasEffect = false;
                        }
                        else if (GetItem(item))
                        {
                            Program.buffer.InsertText("Ha aparecido un objeto en la sala");
                            hasEffect = false;
                        }
                    }
                    else if(efecto == 2)
                    {
                        Program.buffer.InsertText("Los ojos los tienes más despiertos y eres capaz de ver en la oscuridad");
                        hasEffect = false;
                        for(int i = 0; i<Program.lvlLayout.Count; i++)
                        {
                            Program.lvlLayout[i].SetVisible(2);
                        }
                        if (pl.GetMaldicion(4))
                        {
                            for(int i = 0; i< pl.GetArrMal().Length; i++)
                            {
                                if(pl.GetArrMal()[i].GetId() == 4)
                                {
                                    pl.GetArrMal()[i] = null;
                                    i = pl.GetArrMal().Length;
                                    Program.buffer.InsertText("¡Has perdido la Maldición del ciego!");
                                }
                            }
                        }
                    }
                    else if(efecto == 3 && CustomMath.RandomUnit() < 0.5)
                    {
                        for (int i = 0; i < pl.GetArrMal().Length; i++)
                            pl.GetArrMal()[i] = null;

                        pl.ExcesoMaldito = 0;
                        pl.RestoreHealth();
                        Program.buffer.InsertText("¡Tu cuerpo se siente renacido!");
                        Program.buffer.InsertText("¡Has recuperado toda la vida!");
                        Program.buffer.InsertText("¡Todas las maldiciones se han desvanecido!");
                    }
                } while (hasEffect);

            }
        }
    }
}