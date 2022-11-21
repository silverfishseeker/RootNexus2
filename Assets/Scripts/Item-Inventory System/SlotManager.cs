using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotManager : IUSelectable {
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
        }
        get => itemRef;
    }

    public void DetachItem() {
        itemRef.transform.SetParent(null);
        itemRef = null;
    }

    public override void OnPointerEnter (PointerEventData eventData) {
        base.OnPointerEnter(eventData);
        lastTouched = slotPos;
    }
    
    public override void OnPointerExit (PointerEventData eventData) {
        base.OnPointerExit(eventData);
        lastTouched = -1;
    }
}
