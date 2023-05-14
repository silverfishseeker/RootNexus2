using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnabledHitboxAction : IBaseAction {
    public Collider2D coll;
    public bool toEnable;
    protected override void SubRun(){
        coll.enabled = toEnable;
    }
}
