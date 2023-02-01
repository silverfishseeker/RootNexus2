using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Muestra una única nube de diálogo sencilla
public class OptionsAction : NubeAction {

    public List<string> opciones;
    public List<IBaseAction> outcomes;
    
    protected override void SubRun(){
        GameStateEngine.gse.dd.Show(textico,this);
        isFinished = false;
    }
}
