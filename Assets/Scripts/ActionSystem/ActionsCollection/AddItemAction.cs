using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemAction : IBaseAction {

    public Item item; // Item en prefab
    public bool quitar;

    protected override void SubRun(){
        if(quitar)
            GameStateEngine.gse.oi.Remove(item);
        else
            GameStateEngine.gse.oi.Add(item.GetNewMe());
    }
}
