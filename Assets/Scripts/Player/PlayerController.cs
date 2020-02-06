using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseManager {

    public LineRenderer lr;

    public override void Init()
    {
        base.Init();
    }

    public override void DoUpdate()
    {
        base.DoUpdate();

        //Maybe just draw a line from the player holding puck, to the goal,
        //to show intention
        // if (controllingPlayer.IsHoldingPuck())
        // {
        //     lr.SetPosition(0, controllingPlayer.transform.position);
        //     lr.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        // } else
        // {
        //     lr.SetPosition(0, Vector3.one * 100f);
        //     lr.SetPosition(1, Vector3.one * 100f);
        // }
    }
}
