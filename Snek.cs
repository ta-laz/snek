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

    public int[] rows;
    public int[] cols;

    public Direction direction = Direction.Right;

    // direction in which it can move
    int[,] D = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

    public Snek(int topScore)
    {
        rows = new int[topScore + 1];
        cols = new int[topScore + 1];

        rows[0] = 10;
        cols[0] = 10;
    }

    // snek properties for row and col 
    public int HeadRow
    {
        get { return rows[0]; }
        set { rows[0] = value; }
    }

    public int HeadCol
    {
        get { return cols[0]; }
        set { cols[0] = value; }
    }

    // methods now
    public void Move(int gridRows, int gridCols)
    {
        //keeps track of all of the snake
        int a = rows.Length - 2;
        while (a >= 0)

        {
            rows[a + 1] = rows[a];
            cols[a + 1] = cols[a];
            a = a - 1;
        }

        this.HeadRow += D[(int)direction, 0];
        this.HeadCol += D[(int)direction, 1];

        // wrapping around situ
        if (this.HeadRow >= gridRows) { this.HeadRow = 0; }
        if (this.HeadRow < 0) { this.HeadRow = gridRows - 1; }
        if (this.HeadCol >= gridCols) { this.HeadCol = 0; }
        if (this.HeadCol < 0) { this.HeadCol = gridCols - 1; }

    }

    public void SetDirection(Direction direction)
    {
        this.direction = direction;
    }

    public bool Overlaps(int rowNew, int colNew, int score, bool skipHead = false)
    {
        // 1. Check the head 
        if (!skipHead && this.HeadRow == rowNew && this.HeadCol == colNew)
        {
            return true;
        }
        else    // 2. Check the rest of the body through loop with PrevLoc
        {
            for (int i = 1; i < score; i++)
            {
                if (rows[i] == rowNew && cols[i] ==
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