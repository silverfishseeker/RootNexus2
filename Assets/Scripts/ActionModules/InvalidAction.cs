using System;
using UnityEngine;

public class InvalidAction : IBaseAction {
    
    protected override void SubRun(){
        throw new InvalidOperationException("An EndAction can't be executed");
    }
}
