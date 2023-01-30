using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Espera a que pulses la tecla Jump
public class WaitAction : IBaseAction {

    public override void Run(){
        enabled =  true;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonUp(DialogueDisplayer.NEXT_BUTTOM)) {
            isFinished = true;
            enabled = false;
        }
    }
}
