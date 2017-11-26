using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseManager {

    public PlayerTeamManager playerTeamManager;
    public HockeyPlayer controllingPlayer;

    public override void Init()
    {
        base.Init();
        controllingPlayer = FindObjectOfType<HockeyPlayer>();
        playerTeamManager = FindObjectOfType<PlayerTeamManager>();
    }

    public void SetControllingPlayer(HockeyPlayer _controllingPlayer)
    {
        controllingPlayer.OnBeingSelectd(false);
        controllingPlayer = _controllingPlayer;
        controllingPlayer.OnBeingSelectd(true);
    }

    public override void DoUpdate()
    {
        base.DoUpdate();

        CheckMovementInput();
    }

    private void CheckMovementInput()
    {
        float moveRight = 0f;
        float moveUp = 0f;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) { moveRight += -1f; }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) { moveRight += 1f; }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) { moveUp += 1f; }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) { moveUp += -1f; }
        if (moveRight == 0f && moveUp == 0f)
        {
            controllingPlayer.StopMoving();
            playerTeamManager.StopOtherPlayer();
        } else
        {
            //normalize so that player will not move FASTER if they press two button at same time.
            controllingPlayer.MoveByDirection(new Vector2(moveRight, moveUp).normalized);
            //playerTeamManager.SetPosAnotherPlayer();
        }
    }
}
