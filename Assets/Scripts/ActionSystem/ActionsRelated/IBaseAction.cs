using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBaseAction : MonoBehaviour {

    public IBaseAction next;

    [HideInInspector]
    public bool isFinished = false;
    // Acción básica, devuelbe la seguiente acción que toca
    public void Run(){
        isFinished = true;
        SubRun();
    }
    protected abstract void SubRun();
}
