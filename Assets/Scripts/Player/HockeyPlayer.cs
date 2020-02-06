using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class HockeyPlayer : MonoBehaviour
{
    private TeamManager teamManager;

    private PlayerRenderer playerRenderer;
    private HockeyPuck interactingPuck;

    [SerializeField]
    private Transform selectingArrow;

    public bool isPlayerTeam = false;
    public GameObject puckPosition;

    [ReadOnly]
    public bool isMoving;

    [SerializeField]
    private float shootPower = 10f;

    [ReadOnly]
    [SerializeField] //we need the number so high because the way 
    //the unity physic works.
    private float baseShootingPower = 1000f;

    [SerializeField]
    public float playerMoveSpeed = 1f;

    protected Vector3 mousePosition;

    private Rigidbody myRigidbody;
    private CapsuleCollider myCollider;
    private Vector3 colliderCenter;

    private const float puckInteractionTime = 0.25f;
    private float cachedPuckInteractionTime;

    private float stunDuration = 1f;
    private float stunCownDown = 0f;

    private Sequence shrinkSequence;

    public void Init(TeamManager _teamManager)
    {
        teamManager = _teamManager;
        myRigidbody = GetComponent<Rigidbody>();
        playerRenderer = GetComponentInChildren<PlayerRenderer>();
        playerRenderer.Init(this);
        cachedPuckInteractionTime = 0f;

        myCollider = GetComponent<CapsuleCollider>();
        colliderCenter = myCollider.center;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(selectingArrow.transform.DOLocalMoveZ(0.5f, 0.75f));
        mySequence.Append(selectingArrow.transform.DOLocalMoveZ(-0.5f, 0.5f));
        mySequence.SetLoops(-1, LoopType.Restart);

        selectingArrow.gameObject.SetActive(false);

    }

    private void Update()
    {
        stunCownDown -= Time.deltaTime;
        if (IsStunned())
        {
            float pushBackSpeed = 20f;
            myRigidbody.velocity = new Vector3(
                stunCownDown * (playerRenderer.isFacingLeft ? pushBackSpeed : -pushBackSpeed),
                0f, 0f );
        }
        if (cachedPuckInteractionTime >= 0f)
        {
            cachedPuckInteractionTime -= Time.deltaTime;
            if (cachedPuckInteractionTime <= 0f && !IsHoldingPuck())
            {
                myRigidbody.detectCollisions = true;
            }
        }

        if (transform.position.y <= -1f || transform.position.y >= 4f)
        {
            myRigidbody.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        if (Mathf.Abs(transform.position.z) >= 23f)
        {
            bool biggerThanZero = transform.position.z > 0;
            transform.position = new Vector3(transform.position.x, 0f, biggerThanZero ? 23f : -23f);
        }
        if (Mathf.Abs(transform.position.x) >= 48f)
        {
            transform.position = new Vector3(transform.position.x > 0 ? 48f : -48f, 0f, transform.position.z );
        }
    }

    public void MoveToPosition(Vector3 _position)
    {
        Vector3 targetPos = new Vector3(_position.x - myCollider.center.x, 0f, _position.z - myCollider.center.z);
        Vector2 direction = new Vector2(targetPos.x - transform.position.x, targetPos.z - transform.position.z).normalized;
        MoveByDirection(direction);
    }

    public void MoveByDirection(Vector2 _direction)
    {
        if (IsStunned())
        {
            return;
        }
        if (!isMoving)
        {
            //movementController.Move(new Vector3(direction.x, 0f, direction.y) * playerMoveSpeed);
            isMoving = true;
            playerRenderer.PlayAnimation(AnimationType.Move_Without_Puck);
        }
        myRigidbody.velocity = new Vector3(_direction.x, 0f, _direction.y) * playerMoveSpeed;
        playerRenderer.SetFacing(_direction.x);
    }

    public void SetFacing(bool _left)
    {
        if (_left)
        {
            myCollider.center = new Vector3(-myCollider.center.x, myCollider.center.y, myCollider.center.z);
        } else
        {
            //right
            myCollider.center = colliderCenter;
        }
    }

    public void StopMoving()
    {
        if (IsStunned())
        {
            return;
        }
        if (isMoving)
        {
            isMoving = false;
            playerRenderer.PlayAnimation(AnimationType.Idle);
        }
        myRigidbody.velocity = Vector3.zero;
    }

    public void MoveToPuck()
    {
        MoveToPosition(HockeyPuck.GetInstance().transform.position);
    }

    public bool IsHoldingPuck()
    {
        return interactingPuck != null;
    }

    public Vector3 GetPuckHoldingPos()
    {
        return puckPosition.transform.position;
    }

    private void OnTouchingPuck(HockeyPuck puck)
    {
        interactingPuck = puck;
        cachedPuckInteractionTime = puckInteractionTime;
        puck.OnPlayerInteract(this);
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.detectCollisions = false;

        // if (isPlayerTeam)
        // {
        //     Director.instance.playerTeamManager.SetControllingPlayer(this);
        // }
    }

    public void PassingPuck(Vector3 pos, float power)
    {
        playerRenderer.PlayAnimation(AnimationType.Strike);
        if (interactingPuck != null)
        {
            Vector3 shotDirection = new Vector3(pos.x - transform.position.x, 0f, pos.z - transform.position.z);
            interactingPuck.OnBeingShot(shotDirection, power * baseShootingPower);
            OnLosingPuck();
        }
    }

    public void OnLosingPuck()
    {
        cachedPuckInteractionTime = puckInteractionTime;
        interactingPuck = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == HockeyPuck.GetPuckLayer()
            && cachedPuckInteractionTime <= 0f)
        {
            HockeyPuck puck = collision.gameObject.GetComponent<HockeyPuck>();
            if (puck.currentControllingPlayer == null)
            {
                if (isPlayerTeam)
                {
                    Director.instance.cameraManager.DoScreenShake();
                }
                OnTouchingPuck(collision.gameObject.GetComponent<HockeyPuck>());
            } else
            {
                //teamates
                if (puck.currentControllingPlayer.isPlayerTeam == isPlayerTeam)
                {
                    if (isPlayerTeam)
                    {
                        Director.instance.cameraManager.DoScreenShake();
                    }
                    OnTouchingPuck(collision.gameObject.GetComponent<HockeyPuck>());
                } else
                {
                    //win battle
                    if (BattleForPuck(puck.currentControllingPlayer))
                    {
                        Director.instance.cameraManager.DoScreenShake();
                        DoShrinkSequence();
                        puck.currentControllingPlayer.PushBack();
                        OnTouchingPuck(collision.gameObject.GetComponent<HockeyPuck>());
                    }
                    else //lose battle
                    {
                        Director.instance.cameraManager.DoScreenShake();
                        puck.currentControllingPlayer.DoShrinkSequence();
                        PushBack();
                    }
                }
            }
        }
    }

    public bool BattleForPuck(HockeyPlayer _enemyPlayer)
    {
        if (_enemyPlayer.IsStunned())
        {
            return true;
        }
        if (IsStunned())
        {
            return false;
        }
        return Random.Range(0f, 1f) >= 0.3f;
    }

    public void PushBack()
    {
        stunCownDown = stunDuration;
        myRigidbody.velocity = Vector3.zero;
        if (playerRenderer.isFacingLeft)
        {
            transform.position += new Vector3(1f, 0f, 0f);
        } else
        {
            transform.position += new Vector3(-1f, 0f, 0f);
        }
    }

    public bool IsStunned()
    {
        return stunCownDown >= 0f;
    }

    public void OnBeingSelectd(bool _isSelected)
    {
        selectingArrow.gameObject.SetActive(_isSelected);
    }

    public void DoShrinkSequence()
    {
        int sign = playerRenderer.isFacingLeft ? -1 : 1;
        playerRenderer.transform.localScale = new Vector3(playerRenderer.transform.localScale.x, 1f, 1f);
        shrinkSequence = DOTween.Sequence();
        shrinkSequence.Append(playerRenderer.transform.DOScale(new Vector3(1.4f * sign, 1f, 1.4f), 0.15f));
        shrinkSequence.Append(playerRenderer.transform.DOScale(new Vector3(1f * sign, 1f, 1f), 0.25f));
    }
}
