using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Espera a que pulses la tecla Jump
public class WaitAction : IBaseAction {

    protected override void SubRun(){
        enabled =  true;
        isFinished = false;
    }

    void Update() {
        if (Input.GetButtonUp(DialogueDisplayer.NEXT_BUTTOM)) {
            isFinished = true;
            enabled = false;
        }
    }
}
