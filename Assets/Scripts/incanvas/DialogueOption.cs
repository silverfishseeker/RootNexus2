using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DialogueOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IExclSelectable {

    private ExclusivityManager selectState;
    private Image img;
    private Sprite neutralSprite;

    public TextMeshProUGUI tmp;
    public Sprite overMouseSprite;
    public Sprite selectedSprite;
    public IBaseAction action;

    protected void Start() {
        selectState = new ExclusivityManager(this);
        img = GetComponent<Image>();
        neutralSprite = img.sprite;
    }

    public void SetTextAndAction(string text, IBaseAction action){
        tmp.text=text;
        this.action = action;
    }

    public void OnPointerEnter (PointerEventData eventData){
        if (!selectState.isSelected)
            img.sprite = overMouseSprite;
    }

    public void OnPointerExit  (PointerEventData eventData){
        if (!selectState.isSelected)
            img.sprite = neutralSprite;
    }

    public void OnPointerClick (PointerEventData eventData){
        selectState.Select();
    }

    public void Select(){
        img.sprite = selectedSprite;
    }

    public void Deselect(){
        OnPointerExit(null);
    }

    public void OnDestroy(){
        selectState.Destroy();
    }
}
