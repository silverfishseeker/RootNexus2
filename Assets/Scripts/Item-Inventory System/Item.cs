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
    

    public override string ToString() => "Item("+name+", "+title+")";

    public SlotManager myslot;
    public static bool dragging;

    public Item(){
        // Establecemos el id en el contructor en vez de en el start porque éste no se ejecuta hasta
        // que se abre el inventario por primera vez, y necesitamos que tenga id antes de eso para
        // cargar correctamente el diccionario de ObjetosInventario
        try{ // Esto da error porque unity pre crea los objetos, pero luego funciona bien en el play
            id = GameStateEngine.gse.GetNewId();
        } catch(NullReferenceException) {
            id = -1; // normalmente nunca deberíamos llegar aquí
        }
       
    }

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
        return go.GetComponent<Item>();
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
