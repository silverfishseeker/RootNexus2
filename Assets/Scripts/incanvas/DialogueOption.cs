using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueOption : Selectable, IExclSelectable {

    private ExclusivityManager selectState;

    public override void OverrStart(){
        selectState = new ExclusivityManager(this);
    }

    public override void OnPointerEnter (PointerEventData eventData){
        if (!selectState.isSelected)
            img.color = overMouseColor;
    }

    public override void OnPointerExit  (PointerEventData eventData){
        if (!selectState.isSelected)
            img.color = neutralColor;
    }

    public override void OnPointerClick (PointerEventData eventData){
        selectState.Select();
    }

    public void Select(){
        img.color = selectedColor;
    }

    public void Deselect(){
        OnPointerExit(null);
    }
}
