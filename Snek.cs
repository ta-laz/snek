using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Raylib_cs;

namespace Snek;

public struct Cord
{
    public int X;
    public int Y;

    public readonly bool Equals(Cord other)
    {
        return (this.X == other.X) && (this.Y == other.Y);
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
        snakeCords[0] = new() { X = 10, Y = 10 };
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

        //endless faff of using structs fml 
        Cord newHead = Head;

        newHead.X += D[(int)direction, 0];
        newHead.Y += D[(int)direction, 1];

        // wrapping around situ
        if (newHead.X >= grid.cols) { newHead.X = 0; }
        if (newHead.X < 0) { newHead.X = grid.cols - 1; }
        if (newHead.Y >= grid.rows) { newHead.Y = 0; }
        if (newHead.Y < 0) { newHead.Y = grid.rows - 1; }

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
    public Cord appleCords;

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
        appleCords.X = rng.Next(0, grid.cols);
        appleCords.Y = rng.Next(0, grid.rows);

        /* check if apple overlaps with any part of the snake  */
        while (snake.Overlaps(appleCords, score))
        {
            appleCords.X = rng.Next(0, grid.cols);
            appleCords.Y = rng.Next(0, grid.rows);
        }
    }

}