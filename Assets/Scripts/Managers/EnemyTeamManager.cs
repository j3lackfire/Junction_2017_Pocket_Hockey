using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeamManager : TeamManager
{
    public override void DoUpdate()
    {
        base.DoUpdate();
        if (IsControllingPuck())
        {

        } else
        {
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
}
