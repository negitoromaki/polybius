using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect4Controller : MonoBehaviour
{
    public int[,] gameBoard = new int[7,6]; // 0 is red; 1 is yellow; -1 means uninitialized

    private int turn = 0; // 0 is red; 1 is yellow
    private bool gameOver = false;

    private void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                gameBoard[i, j] = -1;
            }
        }
    }

    public void placeChip(int x)
    {
        for (int i = 0; i < gameBoard.GetLength(1); i++)
        {
            if (gameBoard[x,i] == -1)
            {
                gameBoard[x,i] = turn;
                // check win condition
                int winner = checkWin(turn, x, i);
                // change turn
                if (winner == -1)
                {
                    switchTurn();
                }
                return;
            }
        }
    }

    public void switchTurn()
    {
        if (turn == 0)
        {
            turn = 1;
        }
        else
        {
            turn = 0;
        }
    }

    public int checkWin(int turn, int x, int y)
    {
        // Check vertical
        if (checkVertical(turn, x, y))
            return turn;
        // Check horizontal
        if (checkHorizontal(turn, x, y))
            return turn;
        // Check diagonals
        // left diagonal
        if (checkLeftDiagonal(turn, x, y))
            return turn;
        // right diagonal
        if (checkRightDiagonal(turn, x, y))
            return turn;

        return -1;
    }

    private bool checkVertical(int turn, int x, int y)
    {
        // only check the three below
        int row = y;
        int column = x;

        if (row <= 3)
        {
            return false;
        }

        for (int i = row - 1; i >= row - 3; i--)
        {
            if (gameBoard[column, i] != turn)
            {
                return false;
            }
        }

        return true;
    }

    private bool checkHorizontal(int turn, int x, int y)
    {
        int row = y;
        int column = x;
        int counter = 1;

        for (int i = column - 1; i >= 0; i--)
        {
            if (gameBoard[i, row] != turn)
            {
                break;
            }

            counter++;
        }

        for (int i = column + 1; i < 7; i++)
        {
            if (gameBoard[i, row] != turn)
            {
                break;
            }

            counter++;
        }

        if (counter >= 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool checkLeftDiagonal(int turn, int x, int y)
    {
        return true; // TODO: fill out method
    }

    private bool checkRightDiagonal(int turn, int x, int y)
    {
        return true; // TODO: fill out method
    }
}
