using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaracteristicaAction : IBaseAction {
    public string caracteristica;
    public Personaje personaje;
    
    protected override void SubRun(){
        string caract;
        if (Personaje.IsNegation(caracteristica, out caract))
            personaje.Remove(caract);
        else
            personaje.Add(caract);
    }
}
