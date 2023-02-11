using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionAction : AbstractConditionAction {
    
    public Personaje personaje;
    public List<string> caracteristicas;

    protected override bool Condition() => personaje.Condition(caracteristicas);

}
