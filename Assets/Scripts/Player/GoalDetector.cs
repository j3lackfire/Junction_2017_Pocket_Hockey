using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour {

    public bool isPlayerTeam;
    public float puckTimer = -1f;

    private void Awake()
    {
        puckTimer = -1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        HockeyPuck puck = other.GetComponent<HockeyPuck>();
        if (puck != null)
        {
            Director.instance.uiManager.DoScore(!isPlayerTeam);
            puckTimer = 1.5f;
        }
    }

    private void Update()
    {
        if (puckTimer >= 0f)
        {
            puckTimer -= Time.unscaledDeltaTime;
            if (puckTimer <= 0f)
            {
                HockeyPuck.GetInstance().transform.position = Vector3.zero;
                HockeyPuck.GetInstance().myRigidbody.velocity = Vector3.zero;

                Director.instance.playerTeamManager.ResetPlayerPosition();
                Director.instance.enemyTeamManager.ResetPlayerPosition();
            }
        }
    }
}
