using System.Numerics;
using System.Reflection;
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

    public Snek(int topScore)
    {
        prevLocRow = new int[topScore];
        prevLocCol = new int[topScore];
    }

    // methods now
    public void Move(int gridRows, int gridCols)
    {
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
        snakeRow += D[(int)direction, 0];
        snakeCol += D[(int)direction, 1];

        // wrapping around situ
        if (snakeRow >= gridRows){snakeRow = 0;}
        if (snakeRow < 0){snakeRow = gridRows - 1;}
        if (snakeCol >= gridCols){snakeCol = 0;}
        if (snakeCol < 0){snakeCol = gridCols - 1;}

    }

    public void SetDirection(Direction direction)
    {
        this.direction = direction;
    }

    public bool HasEaten(int appleRow, int appleCol)
    {
        // This only checks the head of the snake. 
        if ((snakeRow == appleRow) && (snakeCol == appleCol))
        {
            return true;
        }
        return false;
    }

    public bool Overlaps(int row, int col, int score, bool skipHead = false)
    {
        // 1. Check the head 
        if (!skipHead && snakeRow == row && snakeCol == col)
        {
            return true;
        }
        else    // 2. Check the rest of the body through loop with PrevLoc
        {
            for (int i = 0; i < score; i++)
            {
                if (prevLocRow[i] == row && prevLocCol[i] == col)
                {
                    return true;
                }
            }
            return false;
        }
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
/// //////////////////////////////////////////
public class Apple
{
    public int row;
    public int col;

    static Random rng = new Random();

    // constructor:
    public Apple(Snek snake, int gridRows, int gridCols, int score)
    {
        this.Respawn(snake, gridRows, gridCols, score);
    }

    public void Respawn(Snek snake, int gridRows, int gridCols, int score)
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

    public void Overlaps(Snek snake, int score, int gridRows, int gridCols)
    {
        // if the head overlaps, then add to the score
        // I dont think this bit should be here cause 
        // the HasEaten is used to do more stuff

        // if (appleRow == snake.snakeRow && appleCol == snake.snakeCol)
        // {
        //     score += 1;
        // }
        // if body overlaps then respawn apple 
        for (int i = 0; i < score; i++)
        {
            while (row == snake.prevLocRow[i] && col == snake.prevLocCol[i])
            {
                row = rng.Next(0, gridRows);
                col = rng.Next(0, gridCols);
                i = -1;
                break;
            }
        }
    }
}