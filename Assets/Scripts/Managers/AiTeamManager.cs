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
        h.DoRandomizeDecision(other);
    }
}
