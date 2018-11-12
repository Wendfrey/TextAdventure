using System;
using System.Collections.Generic;
using TextAdventure.Rooms;

namespace TextAdventure
{
    struct Vector2
    {
        public int x;
        public int y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public float Magnitud()
        {
            return (float)Math.Pow((x * x) + (y * y), 0.5f);
        }

        public bool Equals(Vector2 comparer)
        {
            return (this.x == comparer.x && this.y == comparer.y);
        }
        public static Vector2 operator *(Vector2 a, int b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 up = new Vector2(0, 1);
        public static Vector2 down = new Vector2(0, -1);
        public static Vector2 right = new Vector2(1, 0);
        public static Vector2 left = new Vector2(-1, 0);
    }

    class Level
    {
        public static List<Room> actualRooms;

        public static List<Room> GetListOfRooms()
        {
            return actualRooms;
        }

        public static void StartLevel(int cantidad)
        {
            actualRooms = new List<Room>();
            List<Vector2> actualVectors;
            if (Program.level > 5)
            {
                double prob = CustomMath.RandomUnit();
                if (prob < 95d / 300d)
                {
                    actualVectors = GenerateLevelVersion2(cantidad);
                }
                else if (prob < 190d / 300d)
                {
                    actualVectors = GenerateLevel(cantidad);
                }
                else if (prob < 285d/300d)
                {
                    actualVectors = GenerateLevelVersion3(cantidad);
                }
                else
                {
                    actualVectors = GenerateLevelVersion4(cantidad);
                }
            }
            else
            {
                actualVectors = GenerateLevel(cantidad);
            }
            for (int i = 0; i < actualVectors.Count; i++)
            {
                actualRooms.Add(new Room(actualVectors[i].x, actualVectors[i].y));
            }
            //First Room is visible
            actualRooms[0].SetVisible(2);
            
            //SET DOORS
            for (int i = 0; i < actualRooms.Count; i++)
            {
                if (!(actualRooms[i].GetNorthRoom() != null && actualRooms[i].GetWestRoom() != null && actualRooms[i].GetSouthRoom() != null && actualRooms[i].GetEastRoom() != null))
                {
                    for (int j = i + 1; j < actualRooms.Count; j++)
                    {
                        if (actualRooms[i].GetNorthRoom() == null)
                        {
                            if (actualRooms[i].GetPosX() == actualRooms[j].GetPosX() && actualRooms[i].GetPosY() + 1 == actualRooms[j].GetPosY())
                            {
                                actualRooms[i].SetNorth(actualRooms[j]);
                                actualRooms[j].SetSouth(actualRooms[i]);
                            }
                        }
                        if (actualRooms[i].GetWestRoom() == null)
                        {
                            if (actualRooms[i].GetPosX() - 1 == actualRooms[j].GetPosX() && actualRooms[i].GetPosY() == actualRooms[j].GetPosY())
                            {
                                actualRooms[i].SetWest(actualRooms[j]);
                                actualRooms[j].SetEast(actualRooms[i]);
                            }
                        }
                        if (actualRooms[i].GetSouthRoom() == null)
                        {
                            if (actualRooms[i].GetPosX() == actualRooms[j].GetPosX() && actualRooms[i].GetPosY() - 1 == actualRooms[j].GetPosY())
                            {
                                actualRooms[i].SetSouth(actualRooms[j]);
                                actualRooms[j].SetNorth(actualRooms[i]);
                            }
                        }
                        if (actualRooms[i].GetEastRoom() == null)
                        {
                            if (actualRooms[i].GetPosX() + 1 == actualRooms[j].GetPosX() && actualRooms[i].GetPosY() == actualRooms[j].GetPosY())
                            {
                                actualRooms[i].SetEast(actualRooms[j]);
                                actualRooms[j].SetWest(actualRooms[i]);
                            }
                        }
                        if (actualRooms[i].GetNorthRoom() != null && actualRooms[i].GetWestRoom() != null && actualRooms[i].GetSouthRoom() != null && actualRooms[i].GetEastRoom() != null)
                        {
                            j = actualRooms.Count;
                        }
                    }
                }
            }

            /*--------------------------------------Crear salas especiales------------------------------------------*/
            List<Room> exitRooms = new List<Room>();
            /*se usa para la cantidad de puertas que debe tener una sala*/
            int temp = 3;
            do
            {
                for (int i = 1; i < actualRooms.Count; i++)
                {
                    int noDoors = 0;
                    if (actualRooms[i].GetEastRoom() == null)
                    {
                        noDoors++;
                    }

                    if (actualRooms[i].GetWestRoom() == null)
                    {
                        noDoors++;
                    }

                    if (actualRooms[i].GetNorthRoom() == null)
                    {
                        noDoors++;
                    }

                    if (actualRooms[i].GetSouthRoom() == null)
                    {
                        noDoors++;
                    }
                    if (noDoors == temp)
                    {
                        /*solo habitaciones con 3 salidas*/
                        exitRooms.Add(actualRooms[i]);
                    }
                }
                if(exitRooms.Count == 0)
                    temp--;
            } while (exitRooms.Count == 0);

            /*Salida_____________*/
            int num = CustomMath.RandomIntNumber(exitRooms.Count - 1);
            Room exit = exitRooms[num];
            exitRooms.RemoveAt(num);//Elimina la sala de las posibilidades de salida
            RoomExit texit = new RoomExit(exit.GetPosX(), exit.GetPosY());

            texit.SetEast(exit.GetEastRoom());
            texit.SetWest(exit.GetWestRoom());
            texit.SetNorth(exit.GetNorthRoom());
            texit.SetSouth(exit.GetSouthRoom());
            if (texit.GetNorthRoom() != null)
                texit.GetNorthRoom().SetSouth(texit);
            if (texit.GetSouthRoom() != null)
                texit.GetSouthRoom().SetNorth(texit);
            if (texit.GetWestRoom() != null)
                texit.GetWestRoom().SetEast(texit);
            if (texit.GetEastRoom() != null)
                texit.GetEastRoom().SetWest(texit);
            actualRooms.Remove(exit);
            actualRooms.Add(texit);

            /*Habitacion cerrada__________*/
            bool existsClosedRoom = false;
            if (temp == 3 && CustomMath.RandomUnit() < (Program.level-1)/3f && exitRooms.Count > 0)
            {
                existsClosedRoom = true;
                num = CustomMath.RandomIntNumber(exitRooms.Count - 1);//num se esta reutilizando de salida
                Room tempRoom = exitRooms[num];
                exitRooms.RemoveAt(num);//Elimina la sala cerrada de las posibilidades
                Room newRoom;
                newRoom = new RoomClosed(tempRoom.GetPosX(), tempRoom.GetPosY());
                newRoom.SetEast(tempRoom.GetEastRoom());
                newRoom.SetWest(tempRoom.GetWestRoom());
                newRoom.SetNorth(tempRoom.GetNorthRoom());
                newRoom.SetSouth(tempRoom.GetSouthRoom());
                if (newRoom.GetNorthRoom() != null)
                    newRoom.GetNorthRoom().SetSouth(newRoom);
                if (newRoom.GetSouthRoom() != null)
                    newRoom.GetSouthRoom().SetNorth(newRoom);
                if (newRoom.GetWestRoom() != null)
                    newRoom.GetWestRoom().SetEast(newRoom);
                if (newRoom.GetEastRoom() != null)
                    newRoom.GetEastRoom().SetWest(newRoom);
                actualRooms.Remove(tempRoom);
                actualRooms.Add(newRoom);
            }

            /*Tesoro____________*/

            int limit = (Program.level > 5)? 3 : (Program.level+1)/2;
            RoomTreasure treasure = null;
            while(limit > 0)
            {
                
                num = CustomMath.RandomIntNumber(actualRooms.Count-1, 1);//num se esta reutilizando de salida
                if(actualRooms[num].GetType() == typeof(Room))
                {
                    Room r = actualRooms[num];
                    treasure = new RoomTreasure(r.GetPosX(), r.GetPosY());
                    treasure.SetEast(r.GetEastRoom());
                    treasure.SetWest(r.GetWestRoom());
                    treasure.SetNorth(r.GetNorthRoom());
                    treasure.SetSouth(r.GetSouthRoom());
                    if (treasure.GetNorthRoom() != null)
                        treasure.GetNorthRoom().SetSouth(treasure);
                    if (treasure.GetSouthRoom() != null)
                        treasure.GetSouthRoom().SetNorth(treasure);
                    if (treasure.GetWestRoom() != null)
                        treasure.GetWestRoom().SetEast(treasure);
                    if (treasure.GetEastRoom() != null)
                        treasure.GetEastRoom().SetWest(treasure);
                    actualRooms.Remove(r);
                    actualRooms.Add(treasure);
                    limit--;
                }
            }
            if(CustomMath.RandomUnit() < 0.5)
            {
                RoomBless bless;
                Room r;
                do
                {
                    r = actualRooms[CustomMath.RandomIntNumber(actualRooms.Count - 1,1)];
                } while (r.GetType() != typeof(Room));

                bless = new RoomBless(r.GetPosX(), r.GetPosY());
                bless.SetEast(r.GetEastRoom());
                bless.SetWest(r.GetWestRoom());
                bless.SetNorth(r.GetNorthRoom());
                bless.SetSouth(r.GetSouthRoom());
                if (bless.GetNorthRoom() != null)
                    bless.GetNorthRoom().SetSouth(bless);
                if (bless.GetSouthRoom() != null)
                    bless.GetSouthRoom().SetNorth(bless);
                if (bless.GetWestRoom() != null)
                    bless.GetWestRoom().SetEast(bless);
                if (bless.GetEastRoom() != null)
                    bless.GetEastRoom().SetWest(bless);
                actualRooms.Remove(r);
                actualRooms.Add(bless);
            }
            /*Miscelaneous*/

            if (existsClosedRoom)
            {
                bool control = true;
                do
                {
                    num = CustomMath.RandomIntNumber(actualRooms.Count - 1, 1);
                    if (actualRooms[num].GetType() == typeof(Room))
                    {
                        actualRooms[num].GetItem(new Item("Llave vieja"));
                        control = false;
                    }
                } while (control);
            }
        }

        public static List<Vector2> GenerateLevel(int cantidad)
        {
            List<Vector2> actualVectors = new List<Vector2>();
            List<Vector2> posibilidades = new List<Vector2>()
            {
                new Vector2(0, 0)
            };


            while (actualVectors.Count < cantidad)
            {
                //Paso 3.1
                Vector2 vv, vv1, vv2, vh1, vh2;

                //Paso 1 -- Añade sala aleatoria de lista de posibilidades
                int index = CustomMath.RandomIntNumber(posibilidades.Count - 1);
                actualVectors.Add(posibilidades[index]);

                //paso 3.2
                vv = posibilidades[index];
                vv1 = new Vector2(vv.x, vv.y - 1);
                vv2 = new Vector2(vv.x, vv.y + 1);
                vh1 = new Vector2(vv.x - 1, vv.y);
                vh2 = new Vector2(vv.x + 1, vv.y);

                //Paso 2 -- Borra la sala añadida de la lista de posibilidades
                posibilidades.RemoveAt(index);

                //Paso 3 -- quita las no posibles de la lista

                for (int i = 0; i < posibilidades.Count; i++)
                {
                    if (posibilidades[i].Equals(vv1) || posibilidades[i].Equals(vv2) || posibilidades[i].Equals(vh1) || posibilidades[i].Equals(vh2))
                    {
                        posibilidades.RemoveAt(i);
                        i--;
                    }
                }

                bool up = true, down = true, left = true, right = true;
                for (int i = 0; i < actualVectors.Count; i++)
                {
                    if (up)
                    {
                        if (actualVectors[i].Equals(vv2) || actualVectors[i].Equals(new Vector2(vv2.x - 1, vv2.y)) || actualVectors[i].Equals(new Vector2(vv2.x + 1, vv2.y)))
                        {
                            up = false;
                        }
                    }

                    if (down)
                    {
                        if (actualVectors[i].Equals(vv1) || actualVectors[i].Equals(new Vector2(vv1.x - 1, vv1.y)) || actualVectors[i].Equals(new Vector2(vv1.x + 1, vv1.y)))
                        {
                            down = false;
                        }
                    }

                    if (right)
                    {
                        if (actualVectors[i].Equals(vh2) || actualVectors[i].Equals(new Vector2(vh2.x, vh2.y - 1)) || actualVectors[i].Equals(new Vector2(vh2.x, vh2.y + 1)))
                        {
                            right = false;
                        }
                    }

                    if (left)
                    {
                        if (actualVectors[i].Equals(vh1) || actualVectors[i].Equals(new Vector2(vh1.x, vh1.y - 1)) || actualVectors[i].Equals(new Vector2(vh1.x, vh1.y + 1)))
                        {
                            left = false;
                        }
                    }
                }

                if (up)
                {
                    posibilidades.Add(vv2);
                }
                if (right)
                {
                    posibilidades.Add(vh2);
                }
                if (down)
                {
                    posibilidades.Add(vv1);
                }
                if (left)
                {
                    posibilidades.Add(vh1);
                }
            }

            return actualVectors;
        }

        public static List<Vector2> GenerateLevelVersion2(int cantidad)
        {
            List<Vector2> actualVectors = new List<Vector2>();
            List<Vector2> posibilidades = new List<Vector2>()
            {
                new Vector2(0, 0)
            };
            
            while (actualVectors.Count < cantidad)
            {
                //Paso 3.1
                Vector2 vv, vv1, vv2, vh1, vh2;

                //Paso 1 -- Añade sala aleatoria de lista de posibilidades
                int index = CustomMath.RandomIntNumber(posibilidades.Count - 1);
                actualVectors.Add(posibilidades[index]);

                //paso 3.2
                vv = posibilidades[index];
                vv1 = new Vector2(vv.x, vv.y - 1);//aba
                vv2 = new Vector2(vv.x, vv.y + 1);//arr
                vh1 = new Vector2(vv.x - 1, vv.y);//izq
                vh2 = new Vector2(vv.x + 1, vv.y);//der

                //Paso 2 -- Borra la sala añadida de la lista de posibilidades
                posibilidades.RemoveAt(index);

                //Paso 3 -- quita las no posibles de la lista

                for (int i = 0; i < posibilidades.Count; i++)
                {
                    if (posibilidades[i].Equals(vv1) || posibilidades[i].Equals(vv2) || posibilidades[i].Equals(vh1) || posibilidades[i].Equals(vh2))
                    {
                        posibilidades.RemoveAt(i);
                        i--;
                    }
                    else if (posibilidades[i].Equals(vv1 + Vector2.down))
                    {
                        bool temp = true;
                        for (int j = 0; j < actualVectors.Count; j++)
                        {
                            if (actualVectors[j].Equals(vv1))
                            {
                                temp = false;
                                j = actualVectors.Count;
                            }
                        }
                        if (temp)
                        {
                            posibilidades.RemoveAt(i);
                            i--;
                        }

                    }
                    else if (posibilidades[i].Equals(vv2 + Vector2.up))
                    {
                        bool temp = true;
                        for (int j = 0; j < actualVectors.Count; j++)
                        {
                            if (actualVectors[j].Equals(vv2))
                            {
                                temp = false;
                                j = actualVectors.Count;
                            }
                        }
                        if (temp)
                        {
                            posibilidades.RemoveAt(i);
                            i--;
                        }
                    }
                    else if (posibilidades[i].Equals(vh1 + Vector2.left))
                    {
                        bool temp = true;
                        for (int j = 0; j < actualVectors.Count; j++)
                        {
                            if (actualVectors[j].Equals(vh1))
                            {
                                temp = false;
                                j = actualVectors.Count;
                            }
                        }
                        if (temp)
                        {
                            posibilidades.RemoveAt(i);
                            i--;
                        }

                    }
                    else if (posibilidades[i].Equals(vh2 + Vector2.right))
                    {
                        bool temp = true;
                        for (int j = 0; j < actualVectors.Count; j++)
                        {
                            if (actualVectors[j].Equals(vh2))
                            {
                                temp = false;
                                j = actualVectors.Count;
                            }
                        }
                        if (temp)
                        {
                            posibilidades.RemoveAt(i);
                            i--;
                        }

                    }
                }

                bool up = true, down = true, left = true, right = true;
                for (int i = 0; i < actualVectors.Count; i++)
                {
                    if (up)
                    {
                        if (actualVectors[i].Equals(vv2) || actualVectors[i].Equals(new Vector2(vv2.x - 1, vv2.y)) || actualVectors[i].Equals(new Vector2(vv2.x - 2, vv2.y)) || actualVectors[i].Equals(new Vector2(vv2.x + 1, vv2.y)) || actualVectors[i].Equals(new Vector2(vv2.x + 2, vv2.y)))
                        {
                            up = false;
                        }
                    }

                    if (down)
                    {
                        if (actualVectors[i].Equals(vv1) || actualVectors[i].Equals(new Vector2(vv1.x - 1, vv1.y)) || actualVectors[i].Equals(new Vector2(vv1.x - 2, vv1.y)) || actualVectors[i].Equals(new Vector2(vv1.x + 1, vv1.y)) || actualVectors[i].Equals(new Vector2(vv1.x + 2, vv1.y)))
                        {
                            down = false;
                        }
                    }

                    if (right)
                    {
                        if (actualVectors[i].Equals(vh2) || actualVectors[i].Equals(new Vector2(vh2.x, vh2.y - 1)) || actualVectors[i].Equals(new Vector2(vh2.x, vh2.y - 2)) || actualVectors[i].Equals(new Vector2(vh2.x, vh2.y + 1)) || actualVectors[i].Equals(new Vector2(vh2.x, vh2.y + 2)))
                        {
                            right = false;
                        }
                    }

                    if (left)
                    {
                        if (actualVectors[i].Equals(vh1) || actualVectors[i].Equals(new Vector2(vh1.x, vh1.y - 1)) || actualVectors[i].Equals(new Vector2(vh1.x, vh1.y - 2)) || actualVectors[i].Equals(new Vector2(vh1.x, vh1.y + 1)) || actualVectors[i].Equals(new Vector2(vh1.x, vh1.y + 2)))
                        {
                            left = false;
                        }
                    }
                }

                if (up)
                {
                    posibilidades.Add(vv2);
                }
                if (right)
                {
                    posibilidades.Add(vh2);
                }
                if (down)
                {
                    posibilidades.Add(vv1);
                }
                if (left)
                {
                    posibilidades.Add(vh1);
                }
            }

            return actualVectors;
        }

        public static List<Vector2> GenerateLevelVersion3(int cantidad)
        {
            List<Vector2> actualVectors = new List<Vector2>();
            List<Vector2> posibilidades = new List<Vector2>()
            {
                new Vector2(0, 0)
            };


            while (actualVectors.Count < cantidad)
            {
                //Paso 3.1
                Vector2 vv, vv1, vv2, vh1, vh2;

                //Paso 1 -- Añade sala aleatoria de lista de posibilidades
                int index = CustomMath.RandomIntNumber(posibilidades.Count - 1);
                actualVectors.Add(posibilidades[index]);

                //paso 3.2
                vv = posibilidades[index];
                vv1 = new Vector2(vv.x, vv.y - 1);
                vv2 = new Vector2(vv.x, vv.y + 1);
                vh1 = new Vector2(vv.x - 1, vv.y);
                vh2 = new Vector2(vv.x + 1, vv.y);

                //Paso 2 -- Borra la sala añadida de la lista de posibilidades
                posibilidades.RemoveAt(index);

                //Paso 3 -- quita las no posibles de la lista

                for (int i = 0; i < posibilidades.Count; i++)
                {
                    if (posibilidades[i].Equals(vv1) || posibilidades[i].Equals(vv2) || posibilidades[i].Equals(vh1) || posibilidades[i].Equals(vh2))
                    {
                        posibilidades.RemoveAt(i);
                        i--;
                    }
                }

                bool up = true, down = true, left = true, right = true;
                if (vv.x % 2 == 0 && vv.y % 2 == 0)
                {
                    for (int i = 0; i < actualVectors.Count; i++)
                    {
                        if (up)
                        {
                            if (actualVectors[i].Equals(vv2))
                            {
                                up = false;
                            }
                        }

                        if (down)
                        {
                            if (actualVectors[i].Equals(vv1))
                            {
                                down = false;
                            }
                        }

                        if (right)
                        {
                            if (actualVectors[i].Equals(vh2))
                            {
                                right = false;
                            }
                        }

                        if (left)
                        {
                            if (actualVectors[i].Equals(vh1))
                            {
                                left = false;
                            }
                        }
                    }
                }
                else if (vv.y % 2 == 0)
                {
                    up = false;
                    down = false;
                    for (int i = 0; i < actualVectors.Count; i++)
                    {
                        if (right)
                        {
                            if (actualVectors[i].Equals(vh2))
                            {
                                right = false;
                            }
                        }

                        if (left)
                        {
                            if (actualVectors[i].Equals(vh1))
                            {
                                left = false;
                            }
                        }
                    }
                }
                else if (vv.x % 2 == 0)
                {
                    left = false;
                    right = false;
                    for (int i = 0; i < actualVectors.Count; i++)
                    {
                        if (up)
                        {
                            if (actualVectors[i].Equals(vv2))
                            {
                                up = false;
                            }
                        }

                        if (down)
                        {
                            if (actualVectors[i].Equals(vv1))
                            {
                                down = false;
                            }
                        }
                    }
                }

                if (up)
                {
                    posibilidades.Add(vv2);
                }
                if (right)
                {
                    posibilidades.Add(vh2);
                }
                if (down)
                {
                    posibilidades.Add(vv1);
                }
                if (left)
                {
                    posibilidades.Add(vh1);
                }
            }

            return actualVectors;
        }

        public static List<Vector2> GenerateLevelVersion4(int cantidad)
        {
            List<Vector2> actualVectors = new List<Vector2>();
            List<Vector2> posibilidades = new List<Vector2>()
            {
                new Vector2(0, 0)
            };


            while (actualVectors.Count < cantidad)
            {
                //Paso 3.1
                Vector2 vv, vv1, vv2, vh1, vh2;

                //Paso 1 -- Añade sala aleatoria de lista de posibilidades
                int index = CustomMath.RandomIntNumber(posibilidades.Count - 1);
                actualVectors.Add(posibilidades[index]);

                //paso 3.2
                vv = posibilidades[index];
                vv1 = new Vector2(vv.x, vv.y - 1);
                vv2 = new Vector2(vv.x, vv.y + 1);
                vh1 = new Vector2(vv.x - 1, vv.y);
                vh2 = new Vector2(vv.x + 1, vv.y);

                //Paso 2 -- Borra la sala añadida de la lista de posibilidades
                posibilidades.RemoveAt(index);

                //Paso 3 -- quita las no posibles de la lista
                for(int i = 0; i<posibilidades.Count; i++)
                {
                    if (posibilidades[i].Equals(vv1) || posibilidades[i].Equals(vv2) || posibilidades[i].Equals(vh1) || posibilidades[i].Equals(vh2))
                    {
                        posibilidades.RemoveAt(i);
                    }
                }

                bool up = true, down = true, left = true, right = true;
                for (int i = 0; i < actualVectors.Count; i++)
                {
                    if (up)
                    {
                        if (actualVectors[i].Equals(vv2))
                        {
                            up = false;
                        }
                    }

                    if (down)
                    {
                        if (actualVectors[i].Equals(vv1))
                        {
                            down = false;
                        }
                    }

                    if (right)
                    {
                        if (actualVectors[i].Equals(vh2))
                        {
                            right = false;
                        }
                    }

                    if (left)
                    {
                        if (actualVectors[i].Equals(vh1))
                        {
                            left = false;
                        }
                    }
                }
                

                if (up)
                {
                    posibilidades.Add(vv2);
                }
                if (right)
                {
                    posibilidades.Add(vh2);
                }
                if (down)
                {
                    posibilidades.Add(vv1);
                }
                if (left)
                {
                    posibilidades.Add(vh1);
                }
            }

            return actualVectors;
        }
    }
}
