using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HockeyPuck : MonoBehaviour {

    public static HockeyPuck _instance;

    public static HockeyPuck GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<HockeyPuck>();
        }
        return _instance;
    }

    public HockeyPlayer currentControllingPlayer;
    public Rigidbody myRigidbody;
    private bool isBeingHold;
    private Vector3 localHoldPosition = new Vector3(-2f, 0.2f, 2.5f);

    public static int GetPuckLayer() { return LayerMask.NameToLayer("Puck"); }

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (currentControllingPlayer != null)
        {
            transform.position = currentControllingPlayer.GetPuckHoldingPos();
            myRigidbody.velocity = Vector3.zero;
        }
    }

    public void OnPlayerInteract(HockeyPlayer player)
    {
        if (currentControllingPlayer != null)
        {
            currentControllingPlayer.OnLosingPuck();
        }
        currentControllingPlayer = player;
        myRigidbody.velocity = Vector3.zero;
    }

    public void OnBeingShot(Vector3 direction, float power)
    {
        currentControllingPlayer = null;
        transform.parent = null;
        myRigidbody.AddForce(direction.normalized * power);
    }
}
