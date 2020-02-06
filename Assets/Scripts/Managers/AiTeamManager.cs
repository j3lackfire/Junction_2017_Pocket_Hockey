using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTeamManager : TeamManager
{
    public float cachedDecisionTime = -1f;

    public override void DoUpdate()
    {
        base.DoUpdate();

        if (IsControllingPuck())
        {
            HockeyPlayer h = GetPuckHoldingPlayer();
            HockeyPlayer other = h;
            foreach (HockeyPlayer player in myPlayers)
            {
                if (player != h)
                {
                    other = player;
                }
            }

            cachedDecisionTime -= Time.deltaTime;
            if (cachedDecisionTime <= 0f)
            {
                cachedDecisionTime = 0.25f;
                RandomizeDecision(h, other);
            }
            if (updateAnotherPosCountDown <= 0 && h.isMoving)
            {
                other.MoveToPosition(GetMirrorPosition(h.transform.position));
            }

        }
        else
        {
            cachedDecisionTime = -1f;
            HockeyPlayer h = GetNearestPlayerToPuck();
            h.MoveToPuck();

            foreach (HockeyPlayer player in myPlayers)
            {
                if (player != h)
                {
                    if (updateAnotherPosCountDown <= 0 && h.isMoving)
                        player.MoveToPosition(GetMirrorPosition(h.transform.position));
                }
            }
        }
        
    }

    private HockeyPlayer GetPuckHoldingPlayer()
    {
        foreach(HockeyPlayer h in myPlayers)
        {
            if (h.IsHoldingPuck())
            {
                return h;
            }
        }
        return null;
    }

    private void RandomizeDecision(HockeyPlayer h, HockeyPlayer other)
    {
        int choice = Random.Range(0, 10);
        switch(choice)
        {
            case 0:
                //strike
                Vector3 goalPos = new Vector3(
                    isPlayerTeam ? 44f : -44f, 
                    0f, 
                    Random.Range(-5.5f, 5.5f));
                h.PassingPuck(goalPos, Ultilities.CalculateShootingPower(h.transform.position, goalPos) + 1);
                break;
            case 1:
            case 2:
            case 3:
                //pass ball
                h.PassingPuck(other.transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-2f, 2f)),
                    Ultilities.CalculateShootingPower(h.transform.position, other.transform.position));
                break;
            default:
                Vector3 posToMove = new Vector3(
                    isPlayerTeam ? 38f : -38f, 
                    0f, 
                    Random.Range(-20f, 20f));
                h.MoveToPosition(posToMove);
                //move
                break;
        }
    }
}
