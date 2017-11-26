using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour {

    protected Director director;

    public virtual void Init()
    {
        director = Director.instance;
    }

    public virtual void DoUpdate() { }

    //called when a scene is reloaded. Temp function for testing.
    public virtual void Exit() { }
}
