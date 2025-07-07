using System.Numerics;
using Raylib_cs;

namespace Snek;

public struct Grid
{
    public int rows;
    public int cols;

    public Grid()
    {
        Console.WriteLine("hi");
    }
}

public struct Config
{
    public int windowWidth;
    public int windowHeight;
    public int gridRows;
    public int gridCols;

    public double startingSpeed;
    public double speedFactor;

    public int maximumLength;
    public int squareSize => gridRows != 0 ? windowWidth / gridRows : 0;
    public int radius => squareSize / 2;

    public static Config Default => new Config
    {
        windowWidth = 800,
        windowHeight = 900,
        gridRows = 20,
        gridCols = 20,
        startingSpeed = 1.0,
        speedFactor = 0.8,
        maximumLength = 11,
    }; // there's.a ; because this is a property definition 

}

class Program
{
    static Rectangle CellToRectangle(int row, int col, int size)
    {
        return new Rectangle(row * size, col * size, size, size);
    }

    // trying to draw the apple shape? 
    static Vector2 CellToCenter(Cord appleCord, int size)
    {
        // Uhhh, we're converting thaaa indices of the cells into 
        // the centers of where they need to drawn (in the pixel world :o )
        float centerX = size * (appleCord.Y + 0.5f);
        float centerY = size * (appleCord.X + 0.5f);
        return new Vector2(centerX, centerY);
    }

    static void DrawGrid(Grid grid, int size)
    {
        for (int row = 0; row < grid.cols; row += 1)
        {
            for (int col = 0; col < grid.rows; col += 1)
            {
                Rectangle cellSquare = CellToRectangle(col, row, size);
                Raylib.DrawRectangleLinesEx(cellSquare, 0.5f, Color.DarkGray);
            }
        }
    }


    // public void Draw(Grid grid, Snek snake, Apple apple, bool gameOver, Config config, int score)
    // {
    //     // this still feels like a bit of a cluster fuck tbh, maybe a class that has all this and the other celltocenter and drawgrid bits in one? 

    //     // should this be in here? 
    //     Vector2 center = CellToCenter(apple.appleCords, config.squareSize);

    //     // Draw the grid 
    //     DrawGrid(grid, config.squareSize);

    //     // Draw the apple, it's randomised once per run now 
    //     Raylib.DrawCircleV(center, config.radius, Color.Red);

    //     for (int i = 0; i <= score; i += 1)
    //     {
    //         int row;
    //         int col;

    //         if (i == 0 && score == 0)
    //         {
    //             row = snake.Head.X;
    //             col = snake.Head.Y;
    //         }

    //         else
    //         {
    //             row = snake.snakeCords[i].X;
    //             col = snake.snakeCords[i].Y;
    //         }

    //         // Draw the snake 
    //         Raylib.DrawRectangleRec(CellToRectangle(col, row, config.squareSize), Color.Green);
    //     }

    //     // code to display the score
    //     Raylib.DrawRectangle(0, 800, config.windowWidth, 100, Color.LightGray);
    //     Raylib.DrawText($"Score: {score}", 20, 820, 30, Color.Black);

    //     if (gameOver)
    //     {
    //         Raylib.DrawText("GAME OVER", 600, 820, 30, Color.Red);
    //     }
    // }

    // Random number generator  
    static Random rng = new Random();

    public static void Main()
    {
        Config config = Config.Default;

        // STATE SECTION 
        // Just means all the things that your program has to keep in mind while it's running.
        double lastFrameMoved = 0;
        double secondsToMove = config.startingSpeed;

        int score = 0;

        bool gameOver = false;

        // Create a new snake and apple
        Grid grid = new() { rows = config.gridRows, cols = config.gridCols };
        Snek snake = new(config.maximumLength);
        Apple apple = new(snake, grid, score);

        Vector2 center = CellToCenter(apple.appleCords, config.squareSize);

        Raylib.InitWindow(config.windowWidth, config.windowHeight, "Snek");

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
                snake.Move(grid);
                gameOver = snake.Overlaps(snake.Head, score, skipHead: true);
            }

            if (snake.Overlaps(apple.appleCords, score))
            {
                score += 1;
                Console.WriteLine("Score:" + score);
                secondsToMove *= config.speedFactor; //if i include this will it not be too big?
                apple.Respawn(snake, score);
                center = CellToCenter(apple.appleCords, config.squareSize);
            }

            // DRAWING SECTION 
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            DrawGrid(grid, config.squareSize);

            // Draw the circle, it's randomised once per run now 
            Raylib.DrawCircleV(center, config.radius, Color.Red);

            for (int i = 0; i <= score; i += 1)
            {
                int row;
                int col;

                if (i == 0 && score == 0)
                {
                    row = snake.Head.X;
                    col = snake.Head.Y;
                }

                else
                {
                    row = snake.snakeCords[i].X;
                    col = snake.snakeCords[i].Y;
                }

                Raylib.DrawRectangleRec(CellToRectangle(col, row, config.squareSize), Color.Green);
            }

            // code to display the score
            Raylib.DrawRectangle(0, 800, config.windowWidth, 100, Color.LightGray);
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