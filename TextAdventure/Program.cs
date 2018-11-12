using System;
using System.Collections.Generic;
using TextAdventure.Comandos;
using TextAdventure.Rooms;

namespace TextAdventure
{
    class Program
    {
        static int width = 120;
        static int height = 20;
        static public ConsoleBuffer buffer = null;
        static public Player pl;
        static public List<Room> lvlLayout = null;
        static public bool goNextLevel = false;
        static public int level;

        static void Main(string[] args)
        {
            Console.SetWindowSize(121, 22);
            buffer = new ConsoleBuffer(width, height, height - 4);
            ConsoleKeyInfo k;
            do
            {
                pl = new Player();
                level = 1;
                buffer.ClearBox();
                int cRooms = 10 + 5 * level;
                Level.StartLevel(cRooms);
                lvlLayout = Level.GetListOfRooms();

                pl.currentRoom = lvlLayout[0];
                Introduccion();
                MainScreen();
                if (pl.IsDead())
                    buffer.Print(43, 10, "--HAS MUERTO--");
                else
                    buffer.Print(41, 10, "--TE HAS RENDIDO--");

                buffer.Print(39, 11, "pulsa r para reiniciar");
                buffer.PrintScreen();
                k = Console.ReadKey();
            } while (k.Key == ConsoleKey.R);
        }

        public static void Introduccion()
        {
            buffer.InsertText("En lo mas profundo de un bosque, en el pie de la montaña mas alta, se dice que existe la cueva del diablo, un lugar laberintico y lleno de monstruos. Se dice que cuando alguien entra no vuelve a salir nunca, o almenos no como humano.");
            buffer.InsertText("Tras años de viaje, un explorador se refugia dentro de una cueva. Al entrar, decide mirar el exterior, pero ya no encuentra salida");
            buffer.InsertText("Ese explorador eres tu");
            buffer.InsertText("");
            buffer.InsertText("Pulsa cualquier tecla para continuar");
            buffer.PrintBackground();
            buffer.PrintText(height - 3);
            buffer.PrintScreen();
            Console.ReadKey();
            buffer.ClearBox();
        }

        public static bool DrawMap()
        {
            int miniMapx = pl.currentRoom.GetPosX() * 2;
            int miniMapy = pl.currentRoom.GetPosY() * 2;

            ConsoleKeyInfo keyInfo;
            do
            {
                for (int i = 0; i < lvlLayout.Count; i++)
                {
                    if (lvlLayout[i].IsVisible() != 0)
                    {
                        int xx = (lvlLayout[i].GetPosX()) * 2 - miniMapx + (width - 20) / 2;
                        int yy = (-lvlLayout[i].GetPosY()) * 2 + miniMapy + height / 2;
                        if (xx >= 1 && xx < width - 21 && yy >= 2 && yy < height - 1)
                        {
                            if (lvlLayout[i].IsVisible() == 2)
                            {
                                if (pl.currentRoom.GetPosX() == lvlLayout[i].GetPosX() && pl.currentRoom.GetPosY() == lvlLayout[i].GetPosY())
                                    buffer.Print(xx, yy, "O");
                                else if (lvlLayout[i].GetType() == typeof(RoomTreasure))
                                    buffer.Print(xx, yy, "Z");
                                else if (lvlLayout[i].GetType() == typeof(RoomExit))
                                    buffer.Print(xx, yy, "S");
                                else if (lvlLayout[i].GetType() == typeof(RoomClosed))
                                    buffer.Print(xx, yy, "T");
                                else if (lvlLayout[i].GetType() == typeof(RoomBless))
                                    buffer.Print(xx, yy, "B");
                                else
                                    buffer.Print(xx, yy, "X");

                                if (lvlLayout[i].GetNorthRoom() != null)
                                {
                                    if (yy >= 3)
                                        buffer.Print(xx, yy - 1, "|");
                                }
                                if (lvlLayout[i].GetSouthRoom() != null)
                                {
                                    if (yy < height - 2)
                                        buffer.Print(xx, yy + 1, "|");
                                }
                                if (lvlLayout[i].GetWestRoom() != null)
                                {
                                    if (xx > 0)
                                        buffer.Print(xx - 1, yy, "-");
                                }
                                if (lvlLayout[i].GetEastRoom() != null)
                                {
                                    if (xx < width - 22)
                                        buffer.Print(xx + 1, yy, "-");
                                }
                            }
                            else
                            {
                                if (pl.currentRoom.GetPosX() == lvlLayout[i].GetPosX() && pl.currentRoom.GetPosY() == lvlLayout[i].GetPosY())
                                    throw new Exception("Sala invisible en propia localización");
                                else
                                    buffer.Print(xx, yy, "?");
                                if (lvlLayout[i].IsVisible() == 3)
                                {
                                    if (lvlLayout[i].GetNorthRoom() != null)
                                    {
                                        if (yy >= 3)
                                            buffer.Print(xx, yy - 1, "|");
                                    }
                                    if (lvlLayout[i].GetSouthRoom() != null)
                                    {
                                        if (yy < height - 2)
                                            buffer.Print(xx, yy + 1, "|");
                                    }
                                    if (lvlLayout[i].GetWestRoom() != null)
                                    {
                                        if (xx > width - 19)
                                            buffer.Print(xx - 1, yy, "-");
                                    }
                                    if (lvlLayout[i].GetEastRoom() != null)
                                    {
                                        if (xx < width - 2)
                                            buffer.Print(xx + 1, yy, "-");
                                    }
                                }
                            }
                        }
                    }
                }

                buffer.Print(1, 0, "MAP");
                buffer.Print(71, 0, "Usa flechas para mover mapa");
                buffer.Print(1, 2, "O -> Tu");
                buffer.Print(1, 3, "T, Z, B -> Especial");
                buffer.Print(1, 4, "S -> Salida");
                buffer.Print(101, 0, "Planta: " + -(level-1));
                buffer.PrintBackground();

                buffer.PrintScreen();
                keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        miniMapy -= 2;
                        break;
                    case ConsoleKey.UpArrow:
                        miniMapy += 2;
                        break;
                    case ConsoleKey.LeftArrow:
                        miniMapx -= 2;
                        break;
                    case ConsoleKey.RightArrow:
                        miniMapx += 2;
                        break;
                }
            } while (keyInfo.Key == ConsoleKey.RightArrow || keyInfo.Key == ConsoleKey.LeftArrow || keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.DownArrow);
            return true;
        }

        public static void SmallMap()
        {
            int miniMapx = pl.currentRoom.GetPosX() * 2;
            int miniMapy = pl.currentRoom.GetPosY() * 2;

            for (int i = 0; i < lvlLayout.Count; i++)
            {
                if (lvlLayout[i].IsVisible() != 0)
                {
                    int xx = (lvlLayout[i].GetPosX()) * 2 - miniMapx + width - 10;
                    int yy = (-lvlLayout[i].GetPosY()) * 2 + miniMapy + height / 2;
                    if (xx >= width - 20 && xx < width - 1 && yy >= 2 && yy < height - 1)
                    {
                        if (lvlLayout[i].IsVisible() == 2)
                        {
                            if (pl.currentRoom.GetPosX() == lvlLayout[i].GetPosX() && pl.currentRoom.GetPosY() == lvlLayout[i].GetPosY())
                                buffer.Print(xx, yy, "O");
                            else if (lvlLayout[i].GetType() == typeof(RoomTreasure))
                                buffer.Print(xx, yy, "Z");
                            else if (lvlLayout[i].GetType() == typeof(RoomExit))
                                buffer.Print(xx, yy, "S");
                            else if (lvlLayout[i].GetType() == typeof(RoomClosed))
                                buffer.Print(xx, yy, "T");
                            else if (lvlLayout[i].GetType() == typeof(RoomBless))
                                buffer.Print(xx, yy, "B");
                            else
                                buffer.Print(xx, yy, "X");

                            if (lvlLayout[i].GetNorthRoom() != null)
                            {
                                if (yy >= 3)
                                    buffer.Print(xx, yy - 1, "|");
                            }
                            if (lvlLayout[i].GetSouthRoom() != null)
                            {
                                if (yy < height - 2)
                                    buffer.Print(xx, yy + 1, "|");
                            }
                            if (lvlLayout[i].GetWestRoom() != null)
                            {
                                if (xx > width - 19)
                                    buffer.Print(xx - 1, yy, "-");
                            }
                            if (lvlLayout[i].GetEastRoom() != null)
                            {
                                if (xx < width - 2)
                                    buffer.Print(xx + 1, yy, "-");
                            }
                        }
                        else
                        {
                            if (pl.currentRoom.GetPosX() == lvlLayout[i].GetPosX() && pl.currentRoom.GetPosY() == lvlLayout[i].GetPosY())
                                throw new Exception("Sala invisible en propia localización");
                            else
                                buffer.Print(xx, yy, "?");

                            if(lvlLayout[i].IsVisible() == 3)
                            {
                                if (lvlLayout[i].GetNorthRoom() != null)
                                {
                                    if (yy >= 3)
                                        buffer.Print(xx, yy - 1, "|");
                                }
                                if (lvlLayout[i].GetSouthRoom() != null)
                                {
                                    if (yy < height - 2)
                                        buffer.Print(xx, yy + 1, "|");
                                }
                                if (lvlLayout[i].GetWestRoom() != null)
                                {
                                    if (xx > width - 19)
                                        buffer.Print(xx - 1, yy, "-");
                                }
                                if (lvlLayout[i].GetEastRoom() != null)
                                {
                                    if (xx < width - 2)
                                        buffer.Print(xx + 1, yy, "-");
                                }
                            }
                        }
                    }
                }
            }
            buffer.Print(101, 0, "Planta: " + -(level-1));
        }

        //Usado para guardar metodos dentro
        public delegate bool CommandMethod();


        //pantalla principal, donde pasa toda la magia
        public static void MainScreen()
        {
            CommandMethod method;
            string textInput;
            //Dibuja el fondo de primeras
            buffer.Print(1, 0, "PRINCIPAL");
            buffer.PrintBackground();
            buffer.InsertText(pl.currentRoom.GetDescriptionTotal());
            buffer.PrintText(height - 3);
            SmallMap();
            buffer.PrintScreen();

            do
            {
                //Pon el cursos en la posicion de escribir
                Console.SetCursorPosition(1, 18);

                //Espera input
                textInput = Console.ReadLine();

                //Espacio para facilitar lectura
                buffer.InsertText("");
                //Lo inserta en el lineTexto
                buffer.InsertText(textInput);

                //Si el comando es valido
                if (Comando.CheckCommand(textInput))
                {
                    // obten el metodo
                    method = Comando.ReturnCommand(textInput);

                    //si ha obtenido metodo
                    if (method != null)
                    {
                        bool temp = method();
                        if ((method.Method.Name.Equals("MoveNorth") || method.Method.Name.Equals("MoveSouth") || method.Method.Name.Equals("MoveWest") || method.Method.Name.Equals("MoveEast")) && temp)
                        {
                            pl.currentRoom.SetVisible(2);
                            if (pl.GetMaldicion(1))
                            {
                                if (pl.GetHealth() > 1 && CustomMath.RandomUnit() < 0.1f)
                                {
                                    buffer.InsertText("Te has tropezado y has perdido 1 punto de vida");
                                    pl.ReceiveDamage(1, 0);
                                }
                            }

                            if (pl.currentRoom.ene != null)
                            {
                                buffer.InsertText("Un enemigo ha aparecido");
                                buffer.Print(1, 0, "PRINCIPAL");
                                buffer.PrintBackground();
                                buffer.PrintText(17);
                                SmallMap();
                                buffer.PrintScreen();
                                Console.ReadKey();
                                ConsoleBuffer cbMain = buffer;
                                buffer = new ConsoleBuffer(width, height, height - 5);
                                EscenaCombate.Combate(pl.currentRoom.ene);
                                buffer = cbMain;
                                if (pl.IsDead())
                                {
                                    textInput = "exit";
                                }
                            }

                            buffer.InsertText(pl.currentRoom.GetDescriptionTotal());
                        }
                    }
                    else
                    {
                        //En caso contrario avisa
                        buffer.InsertText("Not implemented");
                    }
                }
                else
                {
                    buffer.InsertText("Comando no valido");
                }
                if (goNextLevel)
                {
                    level++;
                    goNextLevel = false;
                    int cRooms = 10 + 5 * level;
                    Level.StartLevel(cRooms);
                    lvlLayout = Level.actualRooms;
                    pl.currentRoom = lvlLayout[0];
                    buffer.ClearBox();
                    if (pl.GetMaldicion(3))
                    {
                        if(CustomMath.RandomUnit() < 0.5)
                        {
                            buffer.InsertText("Tu invalidez ha hecho que ruedes escaleras abajo de manera muy sonora y cómica. Milagrosamente no has recibido ningún daño. La escalera por la que has rebotado se ha destruido");
                        }
                        else
                        {
                            buffer.InsertText("Tu invalidez ha hecho que tus piernas dejen de responder y tengas que bajar usando las manos. Después de media hora has conseguido bajar. Al tocar el suelo del siguiente piso tus piernas vuelven a responder. La escalera por la que te has arrastrado se ha destruido");
                        }
                    }
                    else
                    {

                        buffer.InsertText("Cuando bajas escuchas un fuerte estruendo. Al mirar arriba las escaleras acaban en el techo de la sala");
                    }

                    buffer.InsertText(pl.currentRoom.GetDescriptionTotal());
                    if (pl.GetMaldicion(4))
                    {
                        buffer.InsertText("Sientes como un peso se levanta de ti");
                        for (int i = 0; i < pl.GetArrMal().Length; i++)
                        {
                            if (pl.GetArrMal()[i].GetId() == 4)
                            {
                                pl.GetArrMal()[i] = null;
                                i = pl.GetArrMal().Length;
                            }
                        }
                    }
                    
                }

                buffer.Print(1, 0, "PRINCIPAL");
                buffer.PrintBackground();
                buffer.PrintText(17);
                SmallMap();
                buffer.PrintScreen();
            } while (!textInput.Equals("exit") && !pl.IsDead());
        }

        public static int GetLevel()
        {
            return level;
        }
    }
}
