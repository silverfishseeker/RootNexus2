using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activable : ActionSetter {
    public bool active = true;
    protected bool isAvalaible => GameStateEngine.isntEitherPause && active;

    new public void Run(){
        if(isAvalaible)
            base.Run();
    }
}
