using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connect4Controller : MonoBehaviour
{
    public Connect4Player player;
    public GameObject redPuck;
    public GameObject yellowPuck;

    public GameObject gameOverPanel;
    public GameObject buttonPanel;
    public Text gameOverText;

    // 7x6 gameboard, 7 columns 6 rows
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
                Debug.Log("Found point: " + x + "," + i);
                gameBoard[x,i] = turn;
                if (turn == 0)
                {
                    GameObject newPuck = (GameObject)Instantiate(redPuck, new Vector3(0.1f - (3f - x) * 8.7f, 1.7f + (i * 7.3f), 0), Quaternion.identity);
                }
                else if (turn == 1)
                {
                    GameObject newPuck = (GameObject)Instantiate(yellowPuck, new Vector3(0.1f - (3f - x) * 8.7f, 1.7f + (i * 7.3f), 0), Quaternion.identity);
                }
                // check win condition
                int winner = checkWin(turn, x, i);
                // change turn
                if (winner == -1)
                {
                    switchTurn();
                    player.changeInstantiated();
                }
                else
                {
                    // game won
                    Debug.Log("Game won by: " + turn);
                    gameOver = true;
                    EndGame();
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

        if (row <= 2)
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
        int row = y - 1;
        int column = x - 1;
        int counter = 1;

        while (row >= 0 && column >= 0)
        {
            if (gameBoard[column, row] == turn)
            {
                counter++;
                row--;
                column--;
            }
            else
            {
                break;
            }
        }

        row = y + 1;
        column = x + 1;
        while (row < 6 && column < 7)
        {
            if (gameBoard[column, row] == turn)
            {
                counter++;
                row++;
                column++;
            }
            else
            {
                break;
            }
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

    private bool checkRightDiagonal(int turn, int x, int y)
    {
        int column = x - 1;
        int row = y + 1;
        int counter = 1;

        while (row < 6 && column >= 0)
        {
            if (gameBoard[column, row] == turn)
            {
                counter++;
                row++;
                column--;
            }
            else
            {
                break;
            }
        }

        row = y - 1;
        column = x + 1;
        while (row >= 0 && column < 7)
        {
            if (gameBoard[column, row] == turn)
            {
                counter++;
                row--;
                column++;
            }
            else
            {
                break;
            }
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

    public int getTurn()
    {
        return turn;
    }

    public bool GameOver()
    {
        return gameOver;
    }

    private void EndGame()
    {
        buttonPanel.SetActive(false);
        gameOverText.text = "Game Over!\n";
        if (turn == 0)
        {
            gameOverText.text += "Red Wins!";
        }
        else
        {
            gameOverText.text += "Yellow Wins!";
        }

        gameOverPanel.SetActive(true);
    }
}
