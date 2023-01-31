using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivateAction : IBaseAction {
    
    public Activable activable;

    protected override void SubRun(){
        activable.active = false;
    }
}
