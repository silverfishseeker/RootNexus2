using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickableChest : ClickableReader {
    public Item item;
    public Sprite abierto;

    public override void Action() {
        GameStateEngine.gse.oi.Add(item.GetNewMe());
        transform.GetComponent<SpriteRenderer>().sprite = abierto;
        active = false;
        base.Action();
    }
}
