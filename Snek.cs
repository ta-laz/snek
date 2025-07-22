using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Raylib_cs;

namespace Snek;

public struct Cord
{
    public int col;
    public int row;

    public readonly bool Equals(Cord other)
    {
        return (this.col == other.col) && (this.row == other.row);
    }
}

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
    public Cord[] snakeCords;
    public Direction direction = Direction.Right;

    // direction in which it can move
    int[,] D = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

    public Snek(int topScore)
    {
        snakeCords = new Cord[topScore + 1];
        snakeCords[0] = new() { col = 10, row = 10 };
    }

    // snek properties for row and col 
    public Cord Head
    {
        get { return snakeCords[0]; }
        set { snakeCords[0] = value; }
    }

    // methods now
    public void Move(Grid grid)
    {
        //keeps track of all of the snake
        int a = snakeCords.Length - 2;
        while (a >= 0)

        {
            snakeCords[a + 1] = snakeCords[a];
            a = a - 1;
        }

        Cord newHead = Head;

        newHead.col += D[(int)direction, 0];
        newHead.row += D[(int)direction, 1];

        // wrapping around situ
        if (newHead.col >= grid.cols) { newHead.col = 0; }
        if (newHead.col < 0) { newHead.col = grid.cols - 1; }
        if (newHead.row >= grid.rows) { newHead.row = 0; }
        if (newHead.row < 0) { newHead.row = grid.rows - 1; }

        Head = newHead;
    }

    public void SetDirection(Direction direction)
    {
        this.direction = direction;
    }

    public bool Overlaps(Cord newSnakeCords, int score, bool skipHead = false)
    {
        // 1. Check the head 
        if (!skipHead && Head.Equals(newSnakeCords))
        {
            return true;
        }
        else    // 2. Check the rest of the body through loop with PrevLoc
        {
            for (int i = 1; i < score; i++)
            {
                if (snakeCords[i].Equals(newSnakeCords))
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
    public Cord coords;

    public Grid grid;

    static Random rng = new Random();

    // constructor:
    public Apple(Snek snake, Grid grid, int score)
    {
        this.grid = grid;
        this.Respawn(snake, score);
    }

    public void Respawn(Snek snake, int score)
    {
        coords.col = rng.Next(0, grid.cols);
        coords.row = rng.Next(0, grid.rows);

        /* check if apple overlaps with any part of the snake  */
        while (snake.Overlaps(coords, score))
        {
            coords.col = rng.Next(0, grid.cols);
            coords.row = rng.Next(0, grid.rows);
        }
    }

}
