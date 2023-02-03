using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Muestra una única nube de diálogo sencilla
public class NubeAction : IBaseAction, INotificableDialogue {
    
    public string textico;

    public void NotificateMe(){
        isFinished = true;
    }
    
    protected override void SubRun(){
        GameStateEngine.gse.dd.Show(textico,this);
        isFinished = false;
    }
}
