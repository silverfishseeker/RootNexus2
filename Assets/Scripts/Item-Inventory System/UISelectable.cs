using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UISelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    protected Image img;
    private Color neutralColor;
    public Color overMouseColor;

    protected void Start() {
        img = gameObject.GetComponent<Image>();
        neutralColor = img.color;
    }

    public virtual void OnPointerEnter (PointerEventData eventData) {
        img.color = overMouseColor;
    }

    public virtual void OnPointerExit (PointerEventData eventData) {
        img.color = neutralColor;
    }

    public abstract void OnPointerClick(PointerEventData eventData);

}
