using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeltDown : MonoBehaviour {

    bool isMeltDown = false;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMeltDown)
            {
                MeltDownFunction(Director.instance.playerTeamManager);
            } else
            {
                Director.instance.playerTeamManager.BackToNormal();
                isMeltDown = false;
            }
        }

    }

    public void MeltDownFunction(TeamManager _team)
    {
        isMeltDown = true;
        _team.SpeedUp();
    }
}
