using System.Numerics;
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

        // 0, 0, 40 -> 20, 20
        // 20 = 0 + 20;
        // 20 = 0 + 20

        // 1, 1, 40 -> 60, 60
        // 60 = 1 + 20;
        // 60 = 1 + 20

        // Uhhh, we're converting thaaa indices of the cells into 
        // the centers of where they need to drawn (in the pixel world :o )
        float centerX = size * (row + 0.5f);
        float centerY = size * (col + 0.5f);
        return new Vector2(centerX, centerY); }

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
        Raylib.InitWindow(800, 800, "Snek");

        // CONFIGURATION SECTION 
        // (Settings page in a game for example)
        int gridRows = 20;
        int gridCols = 20;
        int squareSize = 40;

        // STATE SECTION 
        // Just means all the things that your program has to keep in mind while it's running.
        // (maybe updated each game loop)
        // Location of snek. 
        int snakeRow = 10;
        int snakeCol = 10;
        
         // I need to randomise the location of the apple here so it only does it once per run not every loop        
        int appleRow = rng.Next(0, 20);
        int appleCol = rng.Next(0, 20);
        Vector2 center = CellToCenter(appleRow, appleCol, squareSize);

        // where the actual loop that we want running goes (frames changing)
        while (!Raylib.WindowShouldClose())
        {

            // UPDATE STATE SECTION 
            // Code to make the snek move and draw the snek 
            if (Raylib.IsKeyPressed(KeyboardKey.Up)) snakeRow -= 1;
            if (Raylib.IsKeyPressed(KeyboardKey.Down)) snakeRow += 1;
            if (Raylib.IsKeyPressed(KeyboardKey.Left)) snakeCol -= 1;
            if (Raylib.IsKeyPressed(KeyboardKey.Right)) snakeCol += 1;

            // DRAWING SECTION 
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            // Nested for loops wow look at me 
            DrawGrid(gridRows, gridCols, squareSize);

            // Draw the circle, it's randomised once per run now 
            Raylib.DrawCircleV(center, 20, Color.Red);

            Raylib.DrawRectangleRec(CellToRectangle(snakeCol, snakeRow, squareSize), Color.Green);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    private static void IsKeyPressed()
    {
        throw new NotImplementedException();
    }
}