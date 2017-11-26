using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour {

    private HockeyPlayer hockeyPlayer;
    [SerializeField]

    private Animator playerAnimator;
    public bool isFacingLeft;

    private float changeFacingCountDown = 0.5f;

    public void Init(HockeyPlayer _hockeyPlayer)
    {
        playerAnimator = GetComponent<Animator>();
        hockeyPlayer = _hockeyPlayer;
        isFacingLeft = false;
        //transform.parent = null;
    }

    private void Update()
    {
        changeFacingCountDown -= Time.deltaTime;
    }

    public void SetFacing(float _directionX)
    {
        if (changeFacingCountDown >= 0f)
        {
            return;
        }
        if (_directionX != 0)
        {
            if (isFacingLeft && _directionX > 0f)
            {
                isFacingLeft = false;
                transform.localScale = Vector3.one;
                hockeyPlayer.SetFacing(isFacingLeft);
                changeFacingCountDown = 0.25f;
            } else
            {
                if (!isFacingLeft && _directionX < 0f)
                {
                    isFacingLeft = true;
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    hockeyPlayer.SetFacing(isFacingLeft);
                    changeFacingCountDown = 0.25f;
                }
            }
        }
    }

    public void PlayAnimation(AnimationType _type)
    {
        playerAnimator.SetTrigger("Enter_" + _type.ToString());
    }
}

public enum AnimationType
{
    Idle,
    Move_Without_Puck,
    Move_With_Puck,
    Strike
}
