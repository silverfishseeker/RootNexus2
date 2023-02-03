using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SelectablePausable : Selectable {

    public override void OnPointerEnter (PointerEventData eventData) {
        if (GameStateEngine.isntPaused)
            OverrOnPointerEnter(eventData);
    }

    public override void OnPointerExit (PointerEventData eventData) {
        if (GameStateEngine.isntPaused)
            OverrOnPointerExit (eventData);
    }

    public override void OnPointerClick(PointerEventData eventData) {
        if (GameStateEngine.isntPaused)
            OverrOnPointerClick(eventData);
    }

    public abstract void OverrOnPointerEnter(PointerEventData eventData);
    public abstract void OverrOnPointerExit (PointerEventData eventData);
    public abstract void OverrOnPointerClick(PointerEventData eventData);

}
