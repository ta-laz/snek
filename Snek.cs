using System.Numerics;
using System.Runtime.CompilerServices;
using Raylib_cs;

namespace Snek;

public class Snek
{
    public enum Direction
    {
        Up = 3,
        Down = 1,
        Left = 2, 
        Right = 0, 
    }
    
    // starting position and direction of the snake
    public int snakeRow = 10;
    public int snakeCol = 10;
    public Direction direction = Direction.Right;

    // direction in which it can move
    int[,] D = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

    // storing the last position in the arrays based on top score
    public int[] prevLocRow;
    public int[] prevLocCol; 
    
    public Snek(int topScore) {
        prevLocRow = new int[topScore];
        prevLocCol = new int[topScore];
    }

// methods now
    public void Move(int gridRows, int gridCols)
    {
        snakeRow += D[(int)direction, 0];
        snakeCol += D[(int)direction, 1];

        //keeps track of all of the snake
        int a = prevLocRow.Length - 2;
        while (a >= 0)
        {
            prevLocRow[a + 1] = prevLocRow[a];
            prevLocCol[a + 1] = prevLocCol[a];
            a = a - 1;
        }
        prevLocRow[0] = snakeRow;
        prevLocCol[0] = snakeCol;

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
    }

    public void SetDirection(Direction direction)
    {
        this.direction = direction;
    }

    public void Grow()
    {
    
    }

    public bool CheckOverlap(int score)
    {
        int b = 2; // this is set as 2 as otherwise it just ignores the last part of the tail 
        while (b <= score)
        {
            if (snakeRow == prevLocRow[b] && snakeCol == prevLocCol[b])
            {
                return true;
            }
            b = b + 1;
        }
        return false;
    }

}
