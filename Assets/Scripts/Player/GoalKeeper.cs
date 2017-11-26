using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeper : MonoBehaviour {
    public bool isPlayerTeam;

    public GameObject[] posList;

    public float moveSpeed = 4f;
    public bool countingUp = true;
    public int currentPos = 0;

    private void Awake()
    {
        countingUp = true;
        currentPos = Random.Range(0,4);
        transform.position = posList[currentPos].transform.position;

    }

    private void Update()
    {
        MoveToPos();
    }

    private void MoveToPos()
    {
        if (IsPosReached())
        {
            SetNextPos();
        }
        transform.position = Vector3.MoveTowards(transform.position, posList[currentPos].transform.position, Time.deltaTime * moveSpeed);
    }

    private void SetNextPos()
    {
        if (countingUp)
        {
            currentPos++;
            if (currentPos >= posList.Length - 1)
            {
                countingUp = false;
            }
        } else
        {
            currentPos--;
            if (currentPos <= 0)
            {
                countingUp = true;
            }
        }
    }

    private bool IsPosReached()
    {
        Vector3 myPos = new Vector3(transform.position.x, 0f, transform.position.z);
        return Vector3.Distance(myPos, posList[currentPos].transform.position) <= 0.5f;
    }
}
