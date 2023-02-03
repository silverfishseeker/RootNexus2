using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Muestra una única nube de diálogo sencilla
public class OptionsAction : IBaseAction, INotificableDialogue {

    public string textico;
    public List<string> opciones;
    public List<IBaseAction> outcomes;
    
    public void NotificateMe(){
        next = GameStateEngine.gse.dd.chosen;
        isFinished = true;
    }
    protected override void SubRun(){
        GameStateEngine.gse.dd.ShowOptions(textico, opciones, outcomes, this);
        isFinished = false;
    }
}
