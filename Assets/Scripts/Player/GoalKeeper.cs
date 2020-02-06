using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeper : MonoBehaviour {
    public bool isPlayerTeam;

    private Vector3 targetPosition;
    private Vector3 centerGoalPosition = new Vector3(44f, 0f, 0f);
    float distanceToGoal = 5f;
    private float moveSpeed = 8f;
    private float currentTimer;
    private float updatePositionTime = 0.05f;

    private void Awake()
    {
        currentTimer = 0f;
        if (isPlayerTeam) {
            centerGoalPosition = new Vector3(
                -centerGoalPosition.x, centerGoalPosition.y, centerGoalPosition.z);
        }
    }

    private void Update()
    {
        currentTimer -= Time.deltaTime;
        if (currentTimer <= 0f) {
            currentTimer += updatePositionTime;
            UpdateTargetPosition();
        }
        MoveToPos();
    }

    private void UpdateTargetPosition() 
    {
        Vector3 puckPosition = HockeyPuck.GetInstance().transform.position;
        if (Mathf.Abs(puckPosition.x) > Mathf.Abs(centerGoalPosition.x)) {
            puckPosition = new Vector3(centerGoalPosition.x, 0f, puckPosition.z);
        }
        Vector3 newDirection = new Vector3(
            puckPosition.x - centerGoalPosition.x,
            0f, puckPosition.z - centerGoalPosition.z).normalized;
        targetPosition = centerGoalPosition + newDirection * distanceToGoal;
        // RandomizeTargetPosition();
    }

    private void MoveToPos()
    {
        // if (IsTargetPosReached()) {
        //     RandomizeTargetPosition();
        // }
        transform.position = Vector3.MoveTowards(
            transform.position, 
            targetPosition, 
            Time.deltaTime * moveSpeed);
    }

    private bool IsTargetPosReached() {
        return (transform.position - targetPosition).magnitude <= 0.1f;
    }

    private void RandomizeTargetPosition() {
        targetPosition += new Vector3(
            Random.Range(-0.2f, 0.2f), 0f, Random.Range(-0.5f, 0.5f));
    }
}
