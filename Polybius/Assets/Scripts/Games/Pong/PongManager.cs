using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongManager : MonoBehaviour
{
    public PongBall ball;
    public int leftScore;
    public int aiScore;

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
