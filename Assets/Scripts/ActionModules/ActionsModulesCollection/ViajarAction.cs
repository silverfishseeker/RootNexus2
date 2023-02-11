using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViajarAction : IBaseAction {

    public string nombreEscena;
    public string entrada; // si es null te lleva a 0,0

    protected override void SubRun(){
        GameStateEngine.LoadScene(nombreEscena, entrada);
    }
}
