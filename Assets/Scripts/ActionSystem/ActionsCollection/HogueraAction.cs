using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HogueraAction : IBaseAction {
    protected override void SubRun(){
        next = null;
        GameStateEngine.AbrirInventario();
        GameStateEngine.gse.oi.Save();
    }
}
