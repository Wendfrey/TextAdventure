using System;
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

                        if (CustomMath.RandomUnit() < 0.75)
                        {
                            if (CustomMath.RandomUnit() < 0.5)
                            {
                                item = new ItemPocion("Gran poción de Vida", 100, ItemPocion.PocionType.hp);
                            }
                            else
                            {
                                item = new ItemPocion("Gran poción de Maná", 100, ItemPocion.PocionType.mana);
                            }
                        }
                        else
                        {
                            double prob = CustomMath.RandomUnit();
                            if (prob < 0.5)
                            {
                                item = new ItemScroll("Pergamino de visión", 0);
                            }
                            else
                            {
                                item = new ItemScroll("Pergamino de salida", 1);
                            }
                        }

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
                        List<Room> r = Program.lvlLayout;
                        for (int i = 0; i<r.Count; i++)
                        {
                            if(r[i].IsVisible() == 0)
                                r[i].SetVisible(3);
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
                        pl.RestoreMana();
                        Program.buffer.InsertText("¡Tu cuerpo se siente renacido!");
                        Program.buffer.InsertText("¡Has recuperado toda la vida!");
                        Program.buffer.InsertText("¡Has recuperado todo el maná!");
                        Program.buffer.InsertText("¡Todas las maldiciones se han desvanecido!");
                        hasEffect = false;
                    }
                } while (hasEffect);
            }
        }
    }
}
