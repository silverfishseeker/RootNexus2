using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViajarAction : IBaseAction {

    public string nombreEscena;
    public int idEntrada; // si es 0 te lleva a 0,0

    protected override void SubRun(){
        GameStateEngine.LoadScene(nombreEscena, idEntrada);
    }
}
