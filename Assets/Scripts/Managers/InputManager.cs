using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : BaseManager {

    private PlayerController playerController;

    public override void Init()
    {
        base.Init();
        playerController = director.playerController;
    }

    public override void DoUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerController.controllingPlayer.PassingPuck();
        }
    }
}
