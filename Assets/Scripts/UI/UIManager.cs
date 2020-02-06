using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : BaseManager {

    public float pauseTimer;
    public bool isPause = false;
    public float timer;
    public int playerScore;
    public int enemyScore;

    public GameObject scorePanel;
    public GameObject logoPanel;
    public GameObject endGamePanel;

    public Text timerText;
    public Text playerScoreText;
    public Text enemyScoreText;

    Sequence mdSequence;

    public float cachedTimeTillCountdown = 30f;
    public float timeTillCountDown = 10f;
    
    public Text endGameReults;

    public override void Init()
    {
        base.Init();
        timer = 90f;
        playerScore = 0;
        enemyScore = 0;
        playerScoreText.text = "0";
        enemyScoreText.text = "0";

        cachedTimeTillCountdown = timeTillCountDown;

        scorePanel.gameObject.SetActive(false);
        PauseGame(2f);
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        if (cachedTimeTillCountdown >= 0f)
        {
            cachedTimeTillCountdown -= Time.deltaTime;
            float xScale = cachedTimeTillCountdown / timeTillCountDown;
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
        endGamePanel.gameObject.SetActive(true);
        PauseGame(999f);
        string display = "";
        string extra = "";
        if (playerScore > enemyScore)
        {
            //blue win
            display += "<color=#0044AAFF>BLUE</color> wins\n";
            extra = "Good job!";
        } else
        {
            if (enemyScore > playerScore)
            {
                //red win
                display += "The winner is <color=#AA0000FF>RED</color>\n";
                extra = "That was fun ^^";
            } else
            {
                //draw
                display += "<color=#aaffaaff> DRAW</color>\n";
                extra = "You did OK.";
            }
        }
        display += "<color=#0044AAFF>" + playerScore.ToString() + "</color>   -   <color=#AA0000FF>" + enemyScore.ToString() + "</color>\n\n";
        display += extra;
        endGameReults.text = display;
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
            if (countDown % 60 >= 10)
            {
                return (countDown / 60).ToString() + ":" + (countDown % 60).ToString();
            } else
            {
                return (countDown / 60).ToString() + ":0" + (countDown % 60).ToString();
            }

        }
    }
}
