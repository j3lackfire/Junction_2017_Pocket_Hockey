using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamManager : TeamManager
{
    public PlayerController playerController;
    private int currentControllingPlayerIndex;

    public override void Init()
    {
        base.Init();
        playerController = director.playerController;
        currentControllingPlayerIndex = 0;
        playerController.SetControllingPlayer(myPlayers[currentControllingPlayerIndex]);
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        if (IsControllingPuck())
        {
            if (updateAnotherPosCountDown <= 0 && GetControlledPlayer().isMoving)
            {
                updateAnotherPosCountDown = updateAnotherPlayerPosTimer;
                SetPosAnotherPlayer();
            }
        } else
        {
            if (GetUncontrolledPlayer() == GetNearestPlayerToPuck())
            {
                GetUncontrolledPlayer().MoveToPuck();
            } else
            {
                if (updateAnotherPosCountDown <= 0 && GetControlledPlayer().isMoving)
                {
                    updateAnotherPosCountDown = updateAnotherPlayerPosTimer;
                    SetPosAnotherPlayer();
                }
            }
        }
        
    }

    public HockeyPlayer GetControlledPlayer()
    {
        return myPlayers[currentControllingPlayerIndex];
    }

    public HockeyPlayer GetUncontrolledPlayer()
    {
        return currentControllingPlayerIndex == 0 ? myPlayers[1] : myPlayers[0];
    }

    public void SetControllingPlayer(HockeyPlayer _player)
    {
        if (myPlayers.Contains(_player))
        {
            currentControllingPlayerIndex = myPlayers.IndexOf(_player);
            playerController.SetControllingPlayer(_player);
        }
    }

    public void SetControllingNextPlayer()
    {
        currentControllingPlayerIndex++;
        if (currentControllingPlayerIndex >= myPlayers.Count)
        {
            currentControllingPlayerIndex = 0;
        }
        playerController.SetControllingPlayer(myPlayers[currentControllingPlayerIndex]);
    }

    public void SetPosAnotherPlayer()
    {
        Vector3 newPos = GetMirrorPosition(myPlayers[currentControllingPlayerIndex].transform.position);
        GetUncontrolledPlayer().MoveToPosition(newPos);
    }

    public void StopOtherPlayer()
    {
        HockeyPlayer anotherPlayer = currentControllingPlayerIndex == 0 ? myPlayers[1] : myPlayers[0];
        anotherPlayer.StopMoving();
    }

}
