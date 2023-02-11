using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class Selectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    protected Image img;
    protected Color neutralColor;
    public Color overMouseColor;
    public Color selectedColor;
    

    protected void Start() {
        img = GetComponent<Image>();
        neutralColor = img.color;
        OverrStart();
    }
    public abstract void OverrStart();
    public abstract void OnPointerEnter (PointerEventData eventData);
    public abstract void OnPointerExit  (PointerEventData eventData);
    public abstract void OnPointerClick (PointerEventData eventData);
}
