using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : BaseManager {

    [ReadOnly][SerializeField]
    private HockeyPlayer playerController;

    [ReadOnly][SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private float cameraMoveSpeed = 2f;

    [ReadOnly][SerializeField]
    private Vector3 originalPos;

    public override void Init()
    {
        base.Init();

        mainCamera = transform.Find("Main_Camera").GetComponent<Camera>();
        originalPos = new Vector3(0f, 15f, 3.5f);

        playerController = FindObjectOfType<HockeyPlayer>();

        cameraMoveSpeed = 1.2f * playerController.playerMoveSpeed;
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
    }

    private void AdjustCameraSize()
    {
        //since the screen is usually 16:9, so when the player look up, the camera should zoom out
    }

    public void DoScreenShake()
    {
        Vector3 newPos = originalPos + new Vector3(0.5f, 0f, 0.25f);
        Vector3 newPos_2 = originalPos + new Vector3(-0.3f, 0f, 0.15f);
        Sequence s = DOTween.Sequence();
        s.Append(mainCamera.transform.DOLocalMove(newPos, 0.15f));
        s.Append(mainCamera.transform.DOLocalMove(newPos_2, 0.2f));
        s.Append(mainCamera.transform.DOLocalMove(originalPos, 0.1f));
    }
}
