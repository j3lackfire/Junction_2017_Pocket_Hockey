using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : BaseManager {

    public List<HockeyPlayer> myPlayers;
    public GoalKeeper goalKeeper;
    public bool isPlayerTeam;

    public float fieldHeight = 25f;
    public float destinationFluctuation = 1f;

    protected float updateAnotherPlayerPosTimer = 0.5f;
    protected float updateAnotherPosCountDown;

    public bool meltDown = false;

    public override void Init()
    {
        base.Init();
        myPlayers = new List<HockeyPlayer>();

        HockeyPlayer[] allPlayers = FindObjectsOfType<HockeyPlayer>();
        for (int i = 0; i < allPlayers.Length; i++)
        {
            if (allPlayers[i].isPlayerTeam == isPlayerTeam)
            {
                myPlayers.Add(allPlayers[i]);
                allPlayers[i].Init(this);
            }
        }

        updateAnotherPosCountDown = updateAnotherPlayerPosTimer;

        GoalKeeper[] gk = FindObjectsOfType<GoalKeeper>();
        for (int i = 0; i < gk.Length; i ++)
        {
            if (gk[i].isPlayerTeam == isPlayerTeam)
            {
                goalKeeper = gk[i];
                break;
            }
        }
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        updateAnotherPosCountDown -= Time.deltaTime;
    }

    public Vector3 GetMirrorPosition(Vector3 _pos)
    {
        float newPosZ = _pos.z > 0 ? _pos.z - fieldHeight : _pos.z + fieldHeight;
        return new Vector3(_pos.x, 0f, newPosZ) + new Vector3(
            Random.Range(-destinationFluctuation, destinationFluctuation), 
            0f, 
            Random.Range(-destinationFluctuation, destinationFluctuation));
    }

    public bool IsControllingPuck()
    {
        for (int i = 0; i < myPlayers.Count; i++)
        {
            if (myPlayers[i].IsHoldingPuck())
            {
                return true;
            }
        }
        return false;
    }

    public HockeyPlayer GetNearestPlayerToPuck()
    {
        int returnIndex = 0;
        float distance = -1f;
        for (int i = 0; i < myPlayers.Count; i ++)
        {
            float d = Vector3.Distance(HockeyPuck.GetInstance().transform.position, myPlayers[i].transform.position);
            if (distance == -1 || distance > d)
            {
                returnIndex = i; 
                distance = d;
            }
        }
        return myPlayers[returnIndex];
    }

    public void ResetPlayerPosition()
    {
        myPlayers[0].transform.position = new Vector3(
            isPlayerTeam ? -14.5f : 14.5f,
            0f,
            14.25f);

        myPlayers[1].transform.position = new Vector3(
            isPlayerTeam ? -14.5f : 14.5f,
            0f,
            -10.25f);
    }

    public void SpeedUp()
    {
        if (meltDown)
        {
            return;
        }
        meltDown = true;
        director.uiManager.MeltDownPause();
        foreach (HockeyPlayer p in myPlayers)
        {
            p.playerMoveSpeed = 20f;
        }

        if (isPlayerTeam)
        {
            foreach (HockeyPlayer p in director.enemyTeamManager.myPlayers)
            {
                p.playerMoveSpeed = 2;
            }
            director.enemyTeamManager.goalKeeper.moveSpeed = 4f;
        }
        else
        {
            foreach (HockeyPlayer p in director.playerTeamManager.myPlayers)
            {
                p.playerMoveSpeed = 2;
            }
            director.playerTeamManager.goalKeeper.moveSpeed = 4f;
        }
    }

    public void BackToNormal()
    {
        if (!meltDown)
        {
            return;
        }
        meltDown = false;
        foreach (HockeyPlayer p in director.enemyTeamManager.myPlayers)
        {
            p.playerMoveSpeed = 20f;
        }
        foreach (HockeyPlayer p in director.playerTeamManager.myPlayers)
        {
            p.playerMoveSpeed = 20f;
        }
        director.playerTeamManager.goalKeeper.moveSpeed = 12f;
        director.enemyTeamManager.goalKeeper.moveSpeed = 12f;
    }
}
