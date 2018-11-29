using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PongManager : MonoBehaviour
{
    public PongBall ball;
    public int leftScore;
    public int aiScore;

    public bool gameOver = false;

    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI aiScoreText;

    public GameObject GameOverUI;
    public TextMeshProUGUI gameOverText;

    private void Update()
    {
        playerScore.text = "Player: " + leftScore;
        aiScoreText.text = "AI: " + aiScore;

        if (leftScore >= 10)
        {
            gameOver = true;
            GameOverUI.SetActive(true);
            gameOverText.text = "Game Over!\nPlayer Wins!";
        }
        else if (aiScore >= 10)
        {
            gameOver = true;
            GameOverUI.SetActive(true);
            gameOverText.text = "Game Over!\nAI Wins!";
        }
    }

    private void Start()
    {
        leftScore = 0;
        aiScore = 0;
        ball.ResetBall();
    }

    public void ScoreLeft()
    {
        leftScore++;
        ball.ResetBall();
    }

    public void ScoreRight()
    {
        aiScore++;
        ball.ResetBall();
    }
}
