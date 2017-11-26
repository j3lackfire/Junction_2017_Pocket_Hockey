using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : BaseManager {

    public float pauseTimer;
    public bool isPause = false;
    public float timer;
    public int playerScore;
    public int enemyScore;

    public Text timerText;
    public Text playerScoreText;
    public Text enemyScoreText;

    public float meltDownTime = 3f;
    public Button meltDownButton;

    public override void Init()
    {
        base.Init();
        timer = 180f;
        playerScore = 0;
        enemyScore = 0;
        playerScoreText.text = "0";
        enemyScoreText.text = "0";
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        if (meltDownTime >= 0f)
        {
            meltDownTime -= Time.deltaTime;
            if (meltDownTime <= 0)
            {
                director.playerTeamManager.BackToNormal();
            }
        }

        timer -= Time.deltaTime;
        timerText.text = SecondToMinute();
        if (timer <= 0f)
        {
            EndMatch();
        }
        if (isPause)
        {
            pauseTimer -= Time.unscaledDeltaTime;
            if (pauseTimer <= 0f)
            {
                ContinueGame();
            }
        }
    }

    public void ContinueGame()
    {
        Debug.Log("Continue game");
        isPause = false;
        Time.timeScale = 1f;
    }

    public void PauseGame(float duration)
    {
        Time.timeScale = 0.0000001f;
        isPause = true;
        pauseTimer = duration;
    }

    public void EndMatch()
    {
        PauseGame(999f);
        if (playerScore > enemyScore)
        {
            //blue win
        } else
        {
            if (enemyScore > playerScore)
            {
                //red win
            } else
            {
                //draw
            }
        }
    }

    public void DoScore(bool isPlayer)
    {
        if (isPlayer)
        {
            playerScore++;
            playerScoreText.text = playerScore.ToString();
        } else
        {
            enemyScore++;
            enemyScoreText.text = enemyScore.ToString();
        }
        PauseGame(2f);
    }

    public string SecondToMinute()
    {
        int countDown = (int)timer;
        if (countDown < 60)
        {
            return countDown.ToString();
        } else
        {
            return (countDown/60).ToString() + ":" + (countDown%60).ToString();
        }
    }

    public void MeltDownPlayerTeam()
    {
        director.playerTeamManager.SpeedUp();
        meltDownTime = 3f;
    }
}
