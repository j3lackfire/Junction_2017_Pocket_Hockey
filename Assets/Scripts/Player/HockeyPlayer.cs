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

    [ReadOnly]
    [SerializeField] //we need the number so high because the way 
    //the unity physic works.
    private float shootingPowerMultiplier = 1000f;

    //Object properties
    public float initialMoveSpeed = 18f;
    public float acceleration = 5f;
    public float maxSpeed = 40f;

    public int combatWeight = 50;
    public int moveChance = 20;
    public int passChance = 10;
    public int shootChance = 4;

    private float moveSpeed = 0f;

    [SerializeField]
    public float defaultPower = 1.5f;
    [SerializeField]
    public float powerAcceleration = 2f;
    private float currentPower;    

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

        ResetMoveSpeed();
    }

    private void Update()
    {
        if (IsHoldingPuck()) {
            ResetMoveSpeed();
            currentPower += acceleration * Time.deltaTime;
        }
        stunCownDown -= Time.deltaTime;
        if (IsStunned())
        {
            float pushBackSpeed = 20f;
            myRigidbody.velocity = new Vector3(
                stunCownDown * (playerRenderer.isFacingLeft ? pushBackSpeed : -pushBackSpeed),
                0f, 0f);
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

    public void DoRandomizeDecision(HockeyPlayer other) {
        int direction = isPlayerTeam ? 1 : -1; //so that we know to move left or right
        int sum = moveChance + passChance + shootChance;
        int choice = Random.Range(0, sum);
        if (choice < moveChance) {
            //do move
            Vector3 posToMove = new Vector3(38f * direction, 0f, Random.Range(-20f, 20f));
            MoveToPosition(posToMove);
        } else {
            if (choice < moveChance + passChance) {
                float maxPassingPower = Ultilities.CalculateShootingPower(transform.position, other.transform.position);
                if (currentPower < maxPassingPower - 0.5f) {
                    DoRandomizeDecision(other);
                } else {
                    float passingPower = Mathf.Min(maxPassingPower + 0.5f, currentPower);
                    PassingPuck(other.transform.position + new Vector3(Random.Range(-1f, 1f), 
                                0f,  Random.Range(-2f, 2f)),  passingPower);
                }
            } else {
                //shoot
                Vector3 goalPos = new Vector3(44f * direction, 0f, Random.Range(-5.5f, 5.5f));
                float shootPower = Ultilities.CalculateShootingPower(transform.position, goalPos);
                if (currentPower < shootPower - 1f) {
                    DoRandomizeDecision(other);
                } else {
                    float passPower = Mathf.Min(shootPower + 1.5f, currentPower);
                    PassingPuck(goalPos, currentPower);
                }
            }
        }
    }

    private void ResetMoveSpeed() {
        moveSpeed = initialMoveSpeed;
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
        myRigidbody.velocity = new Vector3(_direction.x, 0f, _direction.y) * moveSpeed;
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

    public void MoveToPuck()
    {
        MoveToPosition(HockeyPuck.GetInstance().transform.position);
        
        moveSpeed = Mathf.Min(moveSpeed + acceleration * Time.deltaTime, maxSpeed);
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
    }

    public void PassingPuck(Vector3 pos, float power)
    {
        ResetMoveSpeed();
        playerRenderer.PlayAnimation(AnimationType.Strike);
        if (interactingPuck != null)
        {
            Vector3 shotDirection = new Vector3(pos.x - transform.position.x, 0f, pos.z - transform.position.z);
            interactingPuck.OnBeingShot(shotDirection, power * shootingPowerMultiplier);
            OnLosingPuck();
        }
    }

    public void OnLosingPuck()
    {
        currentPower = defaultPower;
        cachedPuckInteractionTime = puckInteractionTime;
        interactingPuck = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ResetMoveSpeed();
        currentPower = defaultPower;
        if (collision.gameObject.layer == HockeyPuck.GetPuckLayer()
            && cachedPuckInteractionTime <= 0f)
        {
            HockeyPuck puck = collision.gameObject.GetComponent<HockeyPuck>();
            if (puck.currentControllingPlayer == null)
            {
                OnTouchingPuck(collision.gameObject.GetComponent<HockeyPuck>());
            } else
            {
                //teamates
                if (puck.currentControllingPlayer.isPlayerTeam == isPlayerTeam)
                {
                    OnTouchingPuck(collision.gameObject.GetComponent<HockeyPuck>());
                } else
                {
                    //win battle
                    if (BattleForPuck(puck.currentControllingPlayer))
                    {
                        DoShrinkSequence();
                        puck.currentControllingPlayer.PushBack();
                        OnTouchingPuck(collision.gameObject.GetComponent<HockeyPuck>());
                    }
                    else //lose battle
                    {
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
        int sum = combatWeight + _enemyPlayer.combatWeight;
        int result = Random.Range(0, sum);
        return result < combatWeight;
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
        ResetMoveSpeed();
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
