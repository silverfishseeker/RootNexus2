using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Item : SelectablePausable, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private Canvas canvas; 
    private ObjetosInventario inventario;
    private Vector2 originalPosition;

    public string title;
    public string description;

    public int id { get; private set; }
    public string itemName => gameObject.name.EndsWith("(Clone)") ?
        gameObject.name.Substring(0, gameObject.name.Length - 7) :
        gameObject.name; // quitar el "(Clone)"

    public override string ToString() => "Item("+name+", "+title+")";

    public SlotManager myslot;
    public static bool dragging;

    private static int nextId = 0;

    public Item(){
        id = nextId++;
    }

    public override bool Equals(object obj) => obj != null && GetType() == obj.GetType() && (obj as Item).itemName == itemName;
    
    public override int GetHashCode() => itemName.GetHashCode();    

    public override void OverrStart(){
        canvas = GameStateEngine.gse.canvas;
        inventario = GameStateEngine.gse.oi;
    }
    

    ////////////////////////////
    /* RUN TIME FUNCTIONALLYTY*/
    ////////////////////////////

    // Algunos items podría devolverse a sí mismo en vez de una copia
    public Item GetNewMe(Transform parent = null, Vector2 position = new Vector2()) {
        GameObject go = Instantiate(gameObject, new Vector3(), new Quaternion(), parent);
        go.GetComponent<RectTransform>().anchoredPosition = position;
        Item it =  go.GetComponent<Item>();
        it.id = id;
        return it;
    }


    // DRAG stuff

    public void OnBeginDrag(PointerEventData eventData) {
        if (!GameStateEngine.isntPaused)
            return;
            
        // permite que se puedan seleccionar objetos detrás del item cogido
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        originalPosition = transform.position;
        dragging = true;
        OnPointerClick(eventData);
    }

    public void OnDrag(PointerEventData data) {
        if (!GameStateEngine.isntPaused)
            return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            data.position,
            canvas.worldCamera,
            out pos);
        transform.position = canvas.transform.TransformPoint(pos);
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!GameStateEngine.isntPaused)
            return;
            
		if (SlotManager.lastTouched >= 0 && SlotManager.lastTouched != myslot.slotPos &&
                inventario.Move(id, SlotManager.lastTouched)){
            myslot.OnPointerClick(eventData);
        }  else
            transform.position = originalPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        dragging = true;
	}

    
    // seleccionable detrás:
    public override void OverrOnPointerEnter (PointerEventData eventData) {
        myslot.OnPointerEnter(eventData);
    }

    public override void OverrOnPointerExit (PointerEventData eventData) {
        myslot.OverrOnPointerExit(eventData);
    }
    
    public override void OverrOnPointerClick(PointerEventData eventData){
        myslot.OverrOnPointerClick(eventData);
    }
}
