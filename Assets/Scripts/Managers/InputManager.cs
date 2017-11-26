using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : BaseManager {

    private PlayerController playerController;

    public override void Init()
    {
        base.Init();
        playerController = director.playerController;
    }

    public override void DoUpdate()
    {
        if (Input.GetMouseButtonDown(0)
            && !EventSystem.current.IsPointerOverGameObject())
        {
            playerController.controllingPlayer.PassingPuck(Camera.main.ScreenToWorldPoint(Input.mousePosition), 
                Ultilities.CalculateShootingPower(playerController.controllingPlayer.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
    }
}
