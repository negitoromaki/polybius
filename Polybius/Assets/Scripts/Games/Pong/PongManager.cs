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

    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI aiScoreText;

    private void Update()
    {
        playerScore.text = "Player: " + leftScore;
        aiScoreText.text = "AI: " + aiScore;
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
