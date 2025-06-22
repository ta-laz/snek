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

    // This is a random number generator that allows me to use it for the apple placement later    
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

        // I need to randomise the location of the apple here so it only does it once per run not every loop 
        Apple apple = new();

        apple.SetPosition(gridRows, gridCols);

        Vector2 center = CellToCenter(apple.appleRow, apple.appleCol, squareSize);

        // FIGURING OUT THE APPLE OVERLAP 
        for (int i = 0; i < score; i++)
        {
            if (apple.InvalidPosition(i, score, snake))
            {
                apple.SetPosition(gridRows, gridCols);
                i = i - 1;
            }
        }

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
                gameOver = snake.CheckOverlap(score);
            }

            // Apple eating situation 
            if (snake.HasEaten(apple.appleRow, apple.appleCol))
            {
                score += 1;
                Console.WriteLine("Score:" + score);
                secondsToMove *= speedFactor;
                apple.appleRow = rng.Next(0, gridRows);
                apple.appleCol = rng.Next(0, gridCols);
                center = CellToCenter(apple.appleRow, apple.appleCol, squareSize);
            }

            // DRAWING SECTION 
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            // Nested for loops wow look at me 
            DrawGrid(gridRows, gridCols, squareSize);

            // Draw the circle, it's randomised once per run now 
            Raylib.DrawCircleV(center, radius, Color.Red);

            Raylib.DrawRectangleRec(CellToRectangle(snake.snakeCol, snake.snakeRow, squareSize), Color.Green);

            // making sure the drawing is only happening after first apple eaten to avoid boxes in the corner
            if (score > 0)
            {
                for (int i = 1; i <= score; i += 1)
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