using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TicTacToeController : MonoBehaviour
{
    public int[,] gameBoard = new int[3, 3]; // 0 is X; 1 is O
    private int turn = 0;
    private bool gameOver = false;

    // pieces
    public GameObject xPiece;
    public GameObject oPiece;

    // ui stuff
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                gameBoard[i, j] = -1;
            }
        }
    }

    // handle touch input
    private void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began)) || Input.GetMouseButtonDown(0))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                Debug.Log("Something Hit");
                if (raycastHit.collider.CompareTag("TicTacBox"))
                {
                    Debug.Log("Clicked a box!");
                    TicBoxController boxControl = raycastHit.collider.gameObject.GetComponent<TicBoxController>();
                    if (boxControl != null)
                    {
                        placeChip(boxControl.xCoord, boxControl.yCoord);
                    }
                    else
                    {
                        Debug.LogError("Missing box controller!");
                    }
                }
            }
        }
    }

    public void placeChip(int x, int y)
    {
        if(gameBoard[x, y] == -1)
        {
            Debug.Log("Found point: " + x + "," + y);
            gameBoard[x, y] = turn;

            Vector3 instantiatePos = Vector3.zero;

            if (x != 1)
            {
                instantiatePos.x = (x - 1) * 4;
            }

            if (y != 1)
            {
                instantiatePos.z = (y - 1) * 4;
            }

            if (turn == 0)
            {
                // instantiate game objects respective to the player piece
                GameObject newPiece = (GameObject)Instantiate(xPiece, instantiatePos, Quaternion.identity);
            }
            else if (turn == 1)
            {
                GameObject newPiece = (GameObject)Instantiate(oPiece, instantiatePos, Quaternion.identity);
            }

            int winner = checkWin(turn, x, y);

            if (winner == -1)
            {
                switchTurn();
            }
            else
            {
                gameOver = true;
                if (winner == -2)
                {
                    turn = -2;
                }
                EndGame();
            }
        }
    }

    private int checkWin(int turn, int x, int y)
    {
        // check column then row then diagonals
        int row = y + 1;
        int column = x;
        int counter = 1;
        while (row < 3)
        {
            if (gameBoard[column, row] == turn)
            {
                counter++;
                row++;
            }
            else
            {
                break;
            }
        }

        row = y - 1;
        while (row >= 0)
        {
            if (gameBoard[column, row] == turn)
            {
                counter++;
                row--;
            }
            else
            {
                break;
            }
        }

        if (counter >= 3)
        {
            return turn;
        }

        row = y;
        column = x + 1;
        counter = 1;
        while (column < 3)
        {
            if (gameBoard[column, row] == turn)
            {
                counter++;
                column++;
            }
            else
            {
                break;
            }
        }

        column = x - 1;
        while (column >= 0)
        {
            if (gameBoard[column, row] == turn)
            {
                counter++;
                column--;
            }
            else
            {
                break;
            }
        }

        if (counter >= 3)
        {
            return turn;
        }
        
        if ((x == 1 && y == 1) 
            || (x == 0 && y == 0) || (x == 0 && y == 2)
            || (x == 2 && y == 0) || (x == 0 && y == 2))
        {
            counter = 1;
            row = y + 1;
            column = x + 1;
            while (row < 3 && column < 3)
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

            row = y - 1;
            column = x - 1;
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
            if (counter >= 3)
            {
                return turn;
            }

            counter = 1;
            row = y + 1;
            column = x - 1;

            while (row < 3 && column >= 0)
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

            while (row >= 0 && column < 3)
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

            if (counter >= 3)
            {
                return turn;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (gameBoard[i, j] == -1)
                {
                    return -1;
                }
            }
        }

        return -2;
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

    public void EndGame()
    {
        gameOverText.text = "Game Over!\n";
        if (turn == 0)
        {
            gameOverText.text += "Player X Wins!";
        }
        else if (turn == 1)
        {
            gameOverText.text += "Player O Wins!";
        }
        else if (turn == -2)
        {
            gameOverText.text += "Cat's game!";
        }
        gameOverPanel.SetActive(true);
    }
}
