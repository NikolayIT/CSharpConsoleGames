using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BreakOut
{
    class Program
    {
        static object lockObject = new object();
        static int firstPlayerPadSize = 10;
        static int ballPositionX = 0;
        static int ballPositionY = 0;
        static int ballDirectionX = -1;
        static int ballDirectionY = 1;
        static int firstPlayerPosition = 0;
        static int point = 0;
        static bool[,] bricks = new bool[10, 4];

        static void SetBallAtTheMiddleOfTheGameField()
        {
            ballPositionX = Console.WindowWidth / 2;
            ballPositionY = Console.WindowHeight / 2;
        }

        static void RemoveScrollBars()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
        }

        static void Init()
        {
            point = 0;
            bricks = new bool[10, 4];
            RemovePaddle();
            firstPlayerPosition = Console.WindowWidth / 2 - firstPlayerPadSize / 2;
            SetBallAtTheMiddleOfTheGameField();
            DrawBricks();
            DrawPaddle();
        }

        private static void MoveBall()
        {
            ballPositionX += ballDirectionX;
            ballPositionY += ballDirectionY;

            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    if (bricks[i, j])
                    {
                        if (ballPositionX >= (i * 5) && ballPositionX <= (i * 5 + 4) && ballPositionY == addHeightToBricks(j))
                        {
                            ballDirectionY *= -1;

                            bricks[i, j] = false;
                            point++;
                            if(point == 40)
                            {
                                Init();
                                return;
                            }
                            for (int k = 0; k < 5; k++)
                            {
                                PrintAtPosition((i * 5) + k, addHeightToBricks(j), ' ');
                            }
                        }
                    }

                }
            }

            //top
            if (ballPositionY == 0)
                ballDirectionY = 1;

            //sides
            if (ballPositionX > Console.WindowWidth - 2 || ballPositionX == 0)
                ballDirectionX = -1 * ballDirectionX;

            //paddle
            if (ballPositionY > Console.WindowHeight - 3)
            {
                if (firstPlayerPosition <= ballPositionX && firstPlayerPosition + firstPlayerPadSize >= ballPositionX)
                    ballDirectionY = -1 * ballDirectionY;
                else
                    Init();
            }
        }

        static void PrintAtPosition(int x, int y, char symbol)
        {
            lock (lockObject)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(symbol);
            }
        }

        static Queue<int[]> ballPath = new Queue<int[]>();

        static void RemoveBall()
        {
            int[] a;
            while (ballPath.Count > 0)
            {
                a = ballPath.Dequeue();
                PrintAtPosition(a[0], a[1], ' ');
            }
        }

        static void DrawBall()
        {
            PrintAtPosition(ballPositionX, ballPositionY, '@');
            ballPath.Enqueue(new int[] { ballPositionX, ballPositionY });
        }

        static void CursorAtTheStart()
        {
            Console.SetCursorPosition(0, 0);
        }

        static void RemovePaddle()
        {
            for (int x = firstPlayerPosition; x < firstPlayerPosition + firstPlayerPadSize; x++)
            {
                PrintAtPosition(x, Console.WindowHeight - 1, ' ');
            }
        }

        static void DrawPaddle()
        {
            for (int x = firstPlayerPosition; x < firstPlayerPosition + firstPlayerPadSize; x++)
            {
                PrintAtPosition(x, Console.WindowHeight - 1, '=');
            }
        }
        static int addHeightToBricks(int j)
        {
            return j + 5;
        }
        static void DrawBricks()
        {
            for (int i = 0; i < bricks.GetLength(0); i++)
            {
                for (int j = 0; j < bricks.GetLength(1); j++)
                {
                    bricks[i, j] = true;
                    PrintAtPosition((i * 5), addHeightToBricks(j), '[');
                    for (int k = 1; k < 4; k++)
                    {
                        PrintAtPosition((i * 5) + k, addHeightToBricks(j), '=');
                    }
                    PrintAtPosition((i * 5) + 4, addHeightToBricks(j), ']');
                }
            }
        }

        static void paddleRight()
        {
            if (firstPlayerPosition < Console.WindowWidth - firstPlayerPadSize - 2)
            {
                firstPlayerPosition++;
            }
        }

        static void paddleLeft()
        {
            if (firstPlayerPosition > 0)
            {
                firstPlayerPosition--;
            }
        }

        static void Main(string[] args)
        {
            Console.WindowWidth = 50;
            Console.BufferWidth = 50;
            RemoveScrollBars();
            Init();
            
            new Thread(() =>
            {
                while (true)
                {

                    if (Console.KeyAvailable)
                    {
                        RemovePaddle();
                        ConsoleKeyInfo keyInfo = Console.ReadKey();
                        if (keyInfo.Key == ConsoleKey.RightArrow)
                        {
                            paddleRight();
                        }
                        if (keyInfo.Key == ConsoleKey.LeftArrow)
                        {
                            paddleLeft();
                        }
                        DrawPaddle();
                    }
                }
            }).Start();

            CursorAtTheStart();

            while (true)
            {
                RemoveBall();
                MoveBall();

                DrawBall();
                Thread.Sleep(120);
            }
        }
    }
}
