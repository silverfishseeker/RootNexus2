using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemAction : IBaseAction {

    public Item item; // Item en prefab

    protected override void SubRun(){
        GameStateEngine.gse.oi.Add(item.GetNewMe());
    }
}
