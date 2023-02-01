using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotManager : SelectablePausable, IExclSelectable {

    private ExclusivityManager selectState;

    public static int lastTouched = -1;

    private string itemTitle{
        get => GameStateEngine.gse.oi.itemTitle.text;
        set => GameStateEngine.gse.oi.itemTitle.text = value;
    }
    private string itemDescrp{
        get => GameStateEngine.gse.oi.itemDescrp.text;
        set => GameStateEngine.gse.oi.itemDescrp.text = value;
    }

    public int slotPos; // Item lo usa para moverse

    public override void OverrStart(){
        selectState = new ExclusivityManager(this);
    }

    public override void OverrOnPointerEnter (PointerEventData eventData) {
        if (!selectState.isSelected)
            img.color = overMouseColor;
        lastTouched = slotPos;
    }
    
    public override void OverrOnPointerExit (PointerEventData eventData) {
        if (!selectState.isSelected)
            img.color = neutralColor;
        lastTouched = -1;
    }

    public override void OverrOnPointerClick(PointerEventData eventData){
        selectState.Select();
    }

    public void Deselect() {
        base.OnPointerExit(null);
    }
    public void Select() {
        img.color = selectedColor;

        Dictionary<int,Item> objs = GameStateEngine.gse.oi.objetos;
        if (objs.ContainsKey(slotPos)){
            Item it = objs[slotPos].GetComponent<Item>();
            itemTitle = it.title;
            itemDescrp = it.description;
        } else{
            itemTitle = "";
            itemDescrp = "";
        }
    }
}
