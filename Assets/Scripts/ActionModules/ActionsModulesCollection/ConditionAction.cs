using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionAction : IBaseAction {
    
    public IBaseAction alternativa;
    public Personaje personaje;
    private IBaseAction original;
    public List<string> caracteristicas;

    void Start() {
        original = next;
    }

    protected override void SubRun(){
        bool b = personaje.Condition(caracteristicas);
        if(b)
            next = original;
        else
            next = alternativa;
    }

}
