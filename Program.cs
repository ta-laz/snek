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
        // Uhhh, we're converting thaaa indices of the cells into 
        // the centers of where they need to drawn (in the pixel world :o )
        float centerX = size * (col + 0.5f);
        float centerY = size * (row + 0.5f);
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

        int[,] D = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

        Raylib.InitWindow(windowWidth, windowHeight, "Snek");

        // STATE SECTION 
        // Just means all the things that your program has to keep in mind while it's running.
        // (maybe updated each game loop)
        // Location of snek. 
        int snakeRow = 10;
        int snakeCol = 10;

        int topScore = 11;
        int[] prevLocRow = new int[topScore];
        int[] prevLocCol = new int[topScore];

        int direction = 0;
       
        double lastFrameMoved = 0;
        double secondsToMove = startingSpeed;

        int score = 0;

         // I need to randomise the location of the apple here so it only does it once per run not every loop        
        int appleRow = rng.Next(0, gridRows);
        int appleCol = rng.Next(0, gridCols);
        Vector2 center = CellToCenter(appleRow, appleCol, squareSize);

        // FIGURING OUT THE APPLE OVERLAP 
        for (int i = 0; i < score; i++)
        {
            if (appleRow == prevLocRow[i] && appleCol == prevLocCol[i])
            {
                appleRow = rng.Next(0, gridRows);
                appleCol = rng.Next(0, gridCols);
                i = i - 1;
            }
        }

        // where the actual loop that we want running goes (frames changing)
        while (!Raylib.WindowShouldClose())
            {

                // UPDATE STATE SECTION 
                // Code to make the snek move and draw the snek 
                if (Raylib.IsKeyPressed(KeyboardKey.Up)) direction = 3;
                if (Raylib.IsKeyPressed(KeyboardKey.Down)) direction = 1;
                if (Raylib.IsKeyPressed(KeyboardKey.Left)) direction = 2;
                if (Raylib.IsKeyPressed(KeyboardKey.Right)) direction = 0;

                // Code to automatically move snek, directionless kinda
                double currentFrame = Raylib.GetTime();
                if (!gameOver && currentFrame - lastFrameMoved >= secondsToMove)
                {
                    snakeRow += D[direction, 0];
                    snakeCol += D[direction, 1];
                    lastFrameMoved = currentFrame;

                    // wrapping around situ
                    if (snakeRow >= gridRows)
                    {
                        // Console.WriteLine("The snake has gone too far down");
                        snakeRow = 0;
                    }
                    if (snakeRow < 0)
                    {
                        // Console.WriteLine("The snake has gone too far up");
                        snakeRow = gridRows - 1;
                    }
                    if (snakeCol >= gridCols)
                    {
                        // Console.WriteLine("The snake has gone too far right");
                        snakeCol = 0;
                    }
                    if (snakeCol < 0)
                    {
                        // Console.WriteLine("The snake has gone too far left");
                        snakeCol = gridCols - 1;
                    }

                    int a = prevLocRow.Length - 2;
                    while (a >= 0)
                    {
                        prevLocRow[a + 1] = prevLocRow[a];
                        prevLocCol[a + 1] = prevLocCol[a];
                        a = a - 1;
                    }
                    prevLocRow[0] = snakeRow;
                    prevLocCol[0] = snakeCol;

                    int b = 2; // this is set as 2 as otherwise it just ignores the last part of the tail 
                    while (b <= score)
                    {
                        if (snakeRow == prevLocRow[b] && snakeCol == prevLocCol[b])
                        {
                            gameOver = true;
                        }
                        b = b + 1;
                    }

                    // printing to check
                    // Console.WriteLine("rows");
                    // foreach (int row in prevLocRow)
                    // {
                    //     Console.WriteLine(row);
                    // }
                    // Console.WriteLine("cols");
                    // foreach (int col in prevLocCol)
                    // {
                    //     Console.WriteLine(col);
                    // }

                }

                // Apple eating situation 
                if ((snakeRow == appleRow) && (snakeCol == appleCol))
                {
                    score += 1;
                    Console.WriteLine("Score:" + score);
                    secondsToMove *= speedFactor;
                    appleRow = rng.Next(0, gridRows);
                    appleCol = rng.Next(0, gridCols);
                    center = CellToCenter(appleRow, appleCol, squareSize);
                }

                // DRAWING SECTION 
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);

                // Nested for loops wow look at me 
                DrawGrid(gridRows, gridCols, squareSize);

                // Draw the circle, it's randomised once per run now 
                Raylib.DrawCircleV(center, radius, Color.Red);

                Raylib.DrawRectangleRec(CellToRectangle(snakeCol, snakeRow, squareSize), Color.Green);

                // making sure the drawing is only happening after first apple eaten to avoid boxes in the corner
                if (score > 0)
                {
                    for (int i = 1; i <= score; i += 1)
                    {
                        Raylib.DrawRectangleRec(CellToRectangle(prevLocCol[i], prevLocRow[i], squareSize), Color.Green);
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