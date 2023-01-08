using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotManager : UISelectable {
    public static int lastTouched = -1;

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
    private static List<UISelectable> instancias;
    public bool isSelected;

    static SlotManager(){
        instancias = new List<UISelectable>();
    }

    new void Start(){
        base.Start();
        isSelected = false;
        instancias.Add(this); // por algún motivo se añaden los objetos mal si esta línea se hace en el constructor, así que la dejamos en Start
    }

    public void DetachItem() {
        itemRef.transform.SetParent(null);
        itemRef = null;
    }

    public override void OnPointerEnter (PointerEventData eventData) {
        if (!isSelected) {
            base.OnPointerEnter(eventData);
        }
        lastTouched = slotPos;
            
    }
    
    public override void OnPointerExit (PointerEventData eventData) {
        if (!isSelected)
            base.OnPointerExit(eventData);
        lastTouched = -1;
    }

    private void Deselect(PointerEventData eventData) {
        isSelected = false;
        base.OnPointerExit(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData){
        foreach(SlotManager sm in instancias) {
            if (this != sm)
                sm.Deselect(eventData);
        }
        isSelected = true;
        img.color = selectedColor;
    }

    
}
