using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBaseAction : MonoBehaviour {

    public IBaseAction next;

    public bool isFinished = false;
    // Acción básica, devuelbe la seguiente acción que toca
    public abstract void Run();
}
