using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Raylib_cs;

namespace Snek;

public class Snek
{
    public enum Direction
    {
        Right,
        Down,
        Left,
        Up
    }

    // starting position and direction of the snake

    public int[] row;
    public int[] col;

    public Direction direction = Direction.Right;

    // direction in which it can move
    int[,] D = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

    public Snek(int topScore)
    {
        row = new int[topScore + 1];
        col = new int[topScore + 1];

        row[0] = 10;
        col[0] = 10;
    }

    // methods now
    public void Move(int gridRows, int gridCols)
    {
        //keeps track of all of the snake
        int a = row.Length - 2;
        while (a >= 0)

        {
            row[a + 1] = row[a];
            col[a + 1] = col[a];
            a = a - 1;
        }

        row[0] += D[(int)direction, 0];
        col[0] += D[(int)direction, 1];

        // wrapping around situ
        if (row[0] >= gridRows) { row[0] = 0; }
        if (row[0] < 0) { row[0] = gridRows - 1; }
        if (col[0] >= gridCols) { col[0] = 0; }
        if (col[0] < 0) { col[0] = gridCols - 1; }

    }

    public void SetDirection(Direction direction)
    {
        this.direction = direction;
    }

    public bool Overlaps(int rowNew, int colNew, int score, bool skipHead = false)
    {
        // 1. Check the head 
        if (!skipHead && row[0] == rowNew && col[0] == colNew)
        {
            return true;
        }
        else    // 2. Check the rest of the body through loop with PrevLoc
        {
            for (int i = 1; i < score; i++)
            {
                if (row[i] == rowNew && col[i] ==
                 colNew)
                {
                    return true;
                }
            }
            return false;
        }
    }
    
}
// ----------------------------------------------------------------
public class Apple
{
    public int row;
    public int col;

    public int gridRows;
    public int gridCols;
    static Random rng = new Random();

    // constructor:
    public Apple(Snek snake, int gridRows, int gridCols, int score)
    {
        this.gridRows = gridRows;
        this.gridCols = gridCols;
        this.Respawn(snake, score);
    }

    public void Respawn(Snek snake, int score)
    {
        row = rng.Next(0, gridRows);
        col = rng.Next(0, gridCols);

        /* check if apple overlaps with any part of the snake  */
        while (snake.Overlaps(row, col, score))
        {
            row = rng.Next(0, gridRows);
            col = rng.Next(0, gridCols);
        }
    }

}