using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace Snake
{


    class Program
    {
        const int FOOD_DISAPPEAR_TIME = 8000;

        static ICollection<Position> CreatePositions()
        {
            return new Position[]
            {
                new Position(0, 1), // right
                new Position(0, -1), // Left
                new Position(1, 0), // Down
                new Position(-1, 0), // Up
            };
        }

        static ICollection<Position> CreateObstacles()
        {
            List<Position> obstacles = new List<Position>()
            {
                new Position(12, 12),
                new Position(14, 20),
                new Position(7, 7),
                new Position(19, 19),
                new Position(6, 9),
            };

            foreach (Position obstacle in obstacles)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(obstacle.y, obstacle.x);
                Console.Write("=");
            }

            return obstacles;
        }

        static Queue<Position> CreateSnake()
        {
            Queue<Position> snakeElements = new Queue<Position>();

            for (int i = 0; i <= 5; i++)
            {
                snakeElements.Enqueue(new Position(0, i));
            }

            return snakeElements;
        }

        static void AddGameObject(Position position, char symbol, ConsoleColor? color = null)
        {
            Console.SetCursorPosition(position.y, position.x);

            if (color.HasValue)
            {
                Console.ForegroundColor = color.Value;
            }

            Console.Write(symbol);
        }

        static Position CreateRandomPosition()
        {
            Random randomNumbersGenerator = new Random();

            var x = randomNumbersGenerator.Next(0, Console.WindowHeight);
            var y = randomNumbersGenerator.Next(0, Console.WindowWidth);

            return  new Position(x,y);
        }

        static void Main(string[] args)
        {
            int lastFoodTime = 0;
            int negativePoints = 0;

            Position[] directions = CreatePositions().ToArray();

            double sleepTime = 100;
            Direction direction = Direction.Right;
            

            Console.BufferHeight = Console.WindowHeight;
            lastFoodTime = Environment.TickCount;

            List<Position> obstacles = CreateObstacles().ToList();

            Queue<Position> snakeElements = CreateSnake();

            Position food;

            do
            {
                food = CreateRandomPosition();
            }
            while (snakeElements.Contains(food) || obstacles.Contains(food));

            AddGameObject(food, '@', ConsoleColor.Yellow);

            foreach (Position position in snakeElements)
            {
                AddGameObject(position, '*', ConsoleColor.DarkGray);
            }

            while (true)
            {
                negativePoints++;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (direction != Direction.Right) direction = Direction.Left;
                    }
                    if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        if (direction != Direction.Left) direction = Direction.Right;
                    }
                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        if (direction != Direction.Down) direction = Direction.Up;
                    }
                    if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        if (direction != Direction.Up) direction = Direction.Down;
                    }
                }

                Position snakeHead = snakeElements.Last();
                Position nextDirection = directions[(int)direction];

                Position snakeNewHead = new Position(snakeHead.x + nextDirection.x,
                    snakeHead.y + nextDirection.y);

                if (snakeNewHead.y < 0) snakeNewHead.y = Console.WindowWidth - 1;
                if (snakeNewHead.x < 0) snakeNewHead.x = Console.WindowHeight - 1;
                if (snakeNewHead.x >= Console.WindowHeight) snakeNewHead.x = 0;
                if (snakeNewHead.y >= Console.WindowWidth) snakeNewHead.y = 0;

                if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game over!");
                    int userPoints = (snakeElements.Count - 6) * 100 - negativePoints;
                    //if (userPoints < 0) userPoints = 0;
                    userPoints = Math.Max(userPoints, 0);
                    Console.WriteLine("Your points are: {0}", userPoints);
                    return;
                }

                Console.SetCursorPosition(snakeHead.y, snakeHead.x);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("*");

                snakeElements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.y, snakeNewHead.x);
                Console.ForegroundColor = ConsoleColor.Gray;
                if (direction == Direction.Right) Console.Write(">");
                if (direction == Direction.Left) Console.Write("<");
                if (direction == Direction.Up) Console.Write("^");
                if (direction == Direction.Down) Console.Write("v");


                if (snakeNewHead.y == food.y && snakeNewHead.x == food.x)
                {
                    // feeding the snake
                    do
                    {
                        food = CreateRandomPosition();
                    }
                    while (snakeElements.Contains(food) || obstacles.Contains(food));
                    lastFoodTime = Environment.TickCount;
                    AddGameObject(food, '@', ConsoleColor.Yellow);
                    sleepTime--;

                    Position obstacle = new Position();
                    do
                    {
                        obstacle = CreateRandomPosition();
                    }
                    while (snakeElements.Contains(obstacle) ||
                        obstacles.Contains(obstacle) ||
                        (food.x != obstacle.x && food.y != obstacle.x));
                    obstacles.Add(obstacle);

                    AddGameObject(obstacle, '=', ConsoleColor.Cyan);
                }
                else
                {
                    // moving...
                    AddGameObject(snakeElements.Dequeue(), ' ');
                }

                if (Environment.TickCount - lastFoodTime >= FOOD_DISAPPEAR_TIME)
                {
                    negativePoints = negativePoints + 50;
                    AddGameObject(food, ' ');

                    do
                    {
                        food = CreateRandomPosition();
                    }
                    while (snakeElements.Contains(food) || obstacles.Contains(food));

                    lastFoodTime = Environment.TickCount;
                }

                AddGameObject(food, '@', ConsoleColor.Yellow);

                sleepTime -= 0.01;

                Thread.Sleep((int)sleepTime);
            }
        }
    }
}
