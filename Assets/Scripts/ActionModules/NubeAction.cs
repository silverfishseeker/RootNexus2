using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Muestra una única nube de diálogo sencilla
public class NubeAction : IBaseAction, INotificableDialogue {
    
    public string textico;

    public void NotificateMe(){
        isFinished = true;
    }
    
    public override void Run(){
        isFinished = false;
        GameStateEngine.gse.dd.Show(textico,this);
    }
}
