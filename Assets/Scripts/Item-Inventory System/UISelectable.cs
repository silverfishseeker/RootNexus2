using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UISelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    protected Image img;
    protected Color neutralColor;
    public Color overMouseColor;

    protected void Start() {
        img = gameObject.GetComponent<Image>();
        neutralColor = img.color;
        OverrStart();
    }

    public void OnPointerEnter (PointerEventData eventData) {
        if (GameStateEngine.isntPaused)
            OverrOnPointerEnter(eventData);
    }

    public void OnPointerExit (PointerEventData eventData) {
        if (GameStateEngine.isntPaused)
            OverrOnPointerExit (eventData);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (GameStateEngine.isntPaused)
            OverrOnPointerClick(eventData);
    }

    public abstract void OverrStart();
    public abstract void OverrOnPointerEnter(PointerEventData eventData);
    public abstract void OverrOnPointerExit (PointerEventData eventData);
    public abstract void OverrOnPointerClick(PointerEventData eventData);

}
