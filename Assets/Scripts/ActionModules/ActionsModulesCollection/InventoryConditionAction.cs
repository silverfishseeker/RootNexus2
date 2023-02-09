using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryConditionAction : AbstractConditionAction {
    
    public List<Item> items;

    protected override bool Condition(){
        foreach(Item s in items)
            if (! GameStateEngine.gse.oi.objetos.ContainsValue(s))
                return false;
        return true;
    }
}
