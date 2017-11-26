using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : BaseManager {

    [ReadOnly][SerializeField]
    private HockeyPlayer playerController;

    [ReadOnly][SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private float cameraMoveSpeed = 2f;

    private Vector3 positionToFollow;

    public override void Init()
    {
        base.Init();
        positionToFollow = Vector3.zero;

        mainCamera = transform.Find("Main_Camera").GetComponent<Camera>();

        playerController = FindObjectOfType<HockeyPlayer>();

        cameraMoveSpeed = 1.2f * playerController.playerMoveSpeed;
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        positionToFollow = Vector3.zero;
        //FollowPlayer();

        MoveToPos();
    }

    private void AdjustCameraSize()
    {
        //since the screen is usually 16:9, so when the player look up, the camera should zoom out
    }

    private void MoveToPos()
    {
        //transform.position = Vector3.Lerp(transform.position, positionToFollow, 1f);
        transform.position = Vector3.MoveTowards(transform.position, positionToFollow, Time.deltaTime * cameraMoveSpeed);
    }
}
