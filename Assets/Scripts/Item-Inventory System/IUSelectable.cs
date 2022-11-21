using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IUSelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private Image img;
    private Color neutralColor;
    public Color selectedColor;

    void Start() {
        img = gameObject.GetComponent<Image>();
        neutralColor = img.color;
    }

    public virtual void OnPointerEnter (PointerEventData eventData) {
        img.color = selectedColor;
    }

    public virtual void OnPointerExit (PointerEventData eventData) {
        img.color = neutralColor;
    }
}
