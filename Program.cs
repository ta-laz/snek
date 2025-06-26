using System.Numerics;
using System.Runtime.CompilerServices;
using Raylib_cs;

namespace Snek;

class Program
{
    static Rectangle CellToRectangle(int row, int col, int size)
    {
        return new Rectangle(row * size, col * size, size, size);
    }

    // trying to draw the apple shape? 
    static Vector2 CellToCenter(int row, int col, int size)
    {
        // Uhhh, we're converting thaaa indices of the cells into 
        // the centers of where they need to drawn (in the pixel world :o )
        float centerX = size * (col + 0.5f);
        float centerY = size * (row + 0.5f);
        return new Vector2(centerX, centerY);
    }

    static void DrawGrid(int rows, int cols, int size)
    {
        for (int row = 0; row < rows; row += 1)
        {
            for (int col = 0; col < cols; col += 1)
            {
                Rectangle cellSquare = CellToRectangle(col, row, size);
                Raylib.DrawRectangleLinesEx(cellSquare, 0.5f, Color.DarkGray);
            }
        }
    }

    public void Draw(int gridRows, int gridCols, int squareSize, Snek snake, Apple apple, int score, int windowWidth, bool gameOver, Vector2 center, int radius)
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);
        DrawGrid(gridRows, gridCols, squareSize);

        // Draw the apple
        Raylib.DrawCircleV(center, radius, Color.Red);

        // Draw the head of the snake  
        Raylib.DrawRectangleRec(CellToRectangle(snake.snakeCol, snake.snakeRow, squareSize), Color.Green);

        // Draw the body of the snake 
        if (score > 0)
        {
            for (int i = 0; i <= score; i += 1)
            {
                Raylib.DrawRectangleRec(CellToRectangle(snake.prevLocCol[i], snake.prevLocRow[i], squareSize), Color.Green);
            }
        }

        // Display the score
        Raylib.DrawRectangle(0, 800, windowWidth, 100, Color.LightGray);
        Raylib.DrawText($"Score: {score}", 20, 820, 30, Color.Black);

        // Display game over 
        if (gameOver)
        {
            Raylib.DrawText("GAME OVER", 600, 820, 30, Color.Red);
        }

        // End the drawing
        Raylib.EndDrawing();
    }

    // Random number generator  
    static Random rng = new Random();

    public static void Main()
    {

        // CONFIGURATION SECTION 
        // (Settings page in a game for example)
        int windowWidth = 800;
        int windowHeight = 900;
        int gridRows = 20;
        int gridCols = 20;

        bool gameOver = false;

        int squareSize = windowWidth / gridRows;
        int radius = squareSize / 2;

        double startingSpeed = 1.0;
        double speedFactor = 0.8;

        int maximumLength = 11;

        Raylib.InitWindow(windowWidth, windowHeight, "Snek");

        Snek snake = new(maximumLength);

        // STATE SECTION 
        // Just means all the things that your program has to keep in mind while it's running.
        // (maybe updated each game loop)
        double lastFrameMoved = 0;
        double secondsToMove = startingSpeed;

        int score = 0;

        // Create a new apple
        Apple apple = new(snake, gridRows, gridCols, score);

        // BROKEN ^ fix the constructor 
        // You need:
        // Apple is on snake - apple.Overlaps(snake) -> true 
        //                     ^ replaces HasEaten
        //                     go through all coords, if match 
        //                     return true 
        // Apple constructor - new Apple(snake, gridRows, gridCols)
        // apple = new(snake, gridRows, gridCols);
        //          \-> generate coordinates
        //            | -> if overlaps ^
        //            | -> else move on


        Vector2 center = CellToCenter(apple.row, apple.col, squareSize);

        // apple.SetPosition(gridRows, gridCols);

        // FIGURING OUT THE APPLE OVERLAP 
        // for (int i = 0; i < score; i++)
        // {
        //     if (apple.InvalidPosition(i, score, snake))
        //     {
        //         apple.SetPosition(gridRows, gridCols);
        //         i = i - 1;
        //     }
        // } [0000000000000]
        //    ^    
        //                    x 
        // LOOP 
        // if(snake.HasEaten(apple)) {
        //   score++;

        // }

        // where the actual loop that we want running goes (frames changing)
        while (!Raylib.WindowShouldClose())
        {

            // UPDATE STATE SECTION 
            // Code to set direction based on keyboard input
            if (Raylib.IsKeyPressed(KeyboardKey.Up)) snake.SetDirection(Snek.Direction.Up);
            if (Raylib.IsKeyPressed(KeyboardKey.Down)) snake.SetDirection(Snek.Direction.Down);
            if (Raylib.IsKeyPressed(KeyboardKey.Left)) snake.SetDirection(Snek.Direction.Left);
            if (Raylib.IsKeyPressed(KeyboardKey.Right)) snake.SetDirection(Snek.Direction.Right);

            // Code to move Snek and check for overlap 
            double currentFrame = Raylib.GetTime();
            if (!gameOver && currentFrame - lastFrameMoved >= secondsToMove)
            {
                lastFrameMoved = currentFrame;
                snake.Move(gridRows, gridCols);
                Console.WriteLine(snake.snakeRow);
                Console.WriteLine(snake.snakeCol);
                
                foreach (var item in snake.prevLocRow)
                {
                    Console.Write(item.ToString(), ", ");
                }
                Console.WriteLine();

                foreach (var item in snake.prevLocCol)
                {
                    Console.Write(item.ToString(), ", ");
                }
                Console.WriteLine();

                gameOver = snake.Overlaps(snake.snakeRow, snake.snakeCol, score, skipHead: true);
            }



            // for the bit above I tried to use Overlaps but it breaks because it checks against the head so it's game over from the first move. I trued to do a condiition thing bool skiphead = false in the method but that also breaks after the first apple was eaten. 




            // Apple eating situation 
            // 1. Check if the snake head overlaps with apple 
            // 2. If yes, update score +1 
            // 3. Speed up the snake 
            // 4. Respawn the apple 
            // 5. Update the center of where the apple needs to be drawn

            if (snake.Overlaps(apple.row, apple.col, score))
            {
                score += 1;
                Console.WriteLine("Score:" + score);
                secondsToMove *= speedFactor; //if i include this will it not be too big?
                apple.Respawn(snake, gridRows, gridCols, score);
                center = CellToCenter(apple.row, apple.col, squareSize);
            }
            // old code
            // if (snake.HasEaten(apple.appleRow, apple.appleCol))
            // {
            //     score += 1;
            //     Console.WriteLine("Score:" + score);
            //     secondsToMove *= speedFactor;
            //     apple.appleRow = rng.Next(0, gridRows);
            //     apple.appleCol = rng.Next(0, gridCols);
            //     center = CellToCenter(apple.appleRow, apple.appleCol, squareSize);
            // }

            // DRAWING SECTION 
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            DrawGrid(gridRows, gridCols, squareSize);

            // Draw the circle, it's randomised once per run now 
            Raylib.DrawCircleV(center, radius, Color.Red);

            Raylib.DrawRectangleRec(CellToRectangle(snake.snakeCol, snake.snakeRow, squareSize), Color.Green);

            // making sure the drawing is only happening after first apple eaten to avoid boxes in the corner
            if (score > 0)
            {
                for (int i = 0; i <= score; i += 1)
                {
                    Raylib.DrawRectangleRec(CellToRectangle(snake.prevLocCol[i], snake.prevLocRow[i], squareSize), Color.Green);
                }
            }

            // code to display the score
            Raylib.DrawRectangle(0, 800, windowWidth, 100, Color.LightGray);
            Raylib.DrawText($"Score: {score}", 20, 820, 30, Color.Black);

            if (gameOver)
            {
                Raylib.DrawText("GAME OVER", 600, 820, 30, Color.Red);
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    private static void IsKeyPressed()
    {
        throw new NotImplementedException();
    }
}