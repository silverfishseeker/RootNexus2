using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotManager : UISelectable {
    public static int lastTouched = -1;

    private string itemTitle{
        get => GameStateEngine.gse.oi.itemTitle.text;
        set => GameStateEngine.gse.oi.itemTitle.text = value;
    }
    private string itemDescrp{
        get => GameStateEngine.gse.oi.itemDescrp.text;
        set => GameStateEngine.gse.oi.itemDescrp.text = value;
    }

    public int slotPos;
    private GameObject itemRef = null;
    public GameObject item{
        set {
            itemRef = value;
            value.transform.SetParent(transform);
            value.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            value.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            value.transform.SetParent(GameStateEngine.gse.oi.transform, true);
            value.GetComponent<Item>().myslot = this;
        }
        get => itemRef;
    }

    
    public Color selectedColor;
    private static List<UISelectable> instancias = new List<UISelectable>();
    public bool isSelected;

    public override void OverrStart(){
        isSelected = false;
        instancias.Add(this); // por algún motivo se añaden los objetos mal si esta línea se hace en el constructor, así que la dejamos en Start
    }

    public void DetachItem() {
        itemRef.transform.SetParent(null);
        itemRef = null;
    }

    public override void OverrOnPointerEnter (PointerEventData eventData) {
        if (!isSelected)
            img.color = overMouseColor;
        lastTouched = slotPos;
    }
    
    public override void OverrOnPointerExit (PointerEventData eventData) {
        if (!isSelected)
            img.color = neutralColor;
        lastTouched = -1;
    }

    private void Deselect(PointerEventData eventData) {
        isSelected = false;
        base.OnPointerExit(eventData);
    }

    public override void OverrOnPointerClick(PointerEventData eventData){
        foreach(SlotManager sm in instancias) {
            if (this != sm)
                sm.Deselect(eventData);
        }
        isSelected = true;
        img.color = selectedColor;

        if(item == null) {
            itemTitle = "";
            itemDescrp = "";
        } else {
            Item it = item.GetComponent<Item>();
            itemTitle = it.title;
            itemDescrp = it.description;
        }
    }

    
}
