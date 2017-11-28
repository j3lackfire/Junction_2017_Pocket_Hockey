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
    public float meltDownTime = 5f;
    public Button meltDownButton;
    public Text meltDownText;
    public Image melthDownProgress; //initial size = 300, end size = 100

    public GameObject mdPanel;
    public GameObject mdText_1;
    public GameObject mdText_2;

    public Text endGameReults;

    public bool meltDownPause = false;

    public override void Init()
    {
        base.Init();
        timer = 90f;
        playerScore = 0;
        enemyScore = 0;
        playerScoreText.text = "0";
        enemyScoreText.text = "0";

        cachedTimeTillCountdown = timeTillCountDown;

        meltDownPause = false;

        mdPanel.gameObject.SetActive(false);
        mdText_1.gameObject.SetActive(false);
        mdText_2.gameObject.SetActive(false);

        scorePanel.gameObject.SetActive(false);
        PauseGame(2f);
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MeltDownPlayerTeam();
        }
        if (cachedTimeTillCountdown >= 0f)
        {
            cachedTimeTillCountdown -= Time.deltaTime;
            float xScale = cachedTimeTillCountdown / timeTillCountDown;
            melthDownProgress.transform.localScale = new Vector3(xScale, 1f, 1f);
            if (cachedTimeTillCountdown <= 0)
            {
                mdSequence.Kill();
                mdSequence = DOTween.Sequence();
                mdSequence.Append(meltDownText.transform.DOScale(1.2f,0.5f));
                mdSequence.Append(meltDownText.transform.DOScale(1f, 0.5f));
                mdSequence.SetLoops(-1, LoopType.Restart);
                meltDownText.text = "PRESS [SPACE]";
            }
        }

        if (meltDownTime >= 0f)
        {
            meltDownTime -= Time.unscaledDeltaTime;
            if (meltDownTime <= 0)
            {
                director.playerTeamManager.BackToNormal();
                meltDownText.text = "MELT DOWN!";
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
            if(meltDownPause)
            {
                if (pauseTimer <= 1.5f)
                {
                    mdText_1.SetActive(true);
                }
                if (pauseTimer <= 1f)
                {
                    mdText_2.SetActive(true);
                }
            }
            if (pauseTimer <= 0f)
            {
                meltDownPause = false;
                mdPanel.gameObject.SetActive(false);
                mdText_1.gameObject.SetActive(false);
                mdText_2.gameObject.SetActive(false);

                ContinueGame();
            }
        }
    }

    public void MeltDownPause()
    {
        PauseGame(2);
        meltDownPause = true;
        mdPanel.gameObject.SetActive(true);
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

    public void MeltDownPlayerTeam()
    {
        if (cachedTimeTillCountdown >= 0f ||
            director.enemyTeamManager.meltDown ||
            isPause )
        {
            return;
        }
        director.playerTeamManager.SpeedUp();
        meltDownTime = 5f;
        cachedTimeTillCountdown = timeTillCountDown;
        mdSequence.Kill();
        meltDownText.transform.localScale = Vector3.one;
    }
}
