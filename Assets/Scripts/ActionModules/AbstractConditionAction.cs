using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractConditionAction : IBaseAction {
    public IBaseAction alternativa;
    private IBaseAction original;
    // Start is called before the first frame update
    void Start() {
        original = next;
    }

    protected override void SubRun(){
        if(Condition())
            next = original;
        else
            next = alternativa;
    }

    protected abstract bool Condition();
}
