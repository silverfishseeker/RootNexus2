using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemAction : IBaseAction {

    public Item item; // Item en prefab

    public override void Run(){
        GameStateEngine.gse.oi.Add(item.GetNewMe());
        isFinished = true;
    }
}
