using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private Canvas canvas; 
    private ObjetosInventario inventario;
    private Vector2 originalPosition;

    public string title;
    public string description;
    
    [HideInInspector] // para que no se pueda editar a mano
    public string ID;

    public bool IsEquals(Item o) => o != null && ID == o.ID;

    public override string ToString() => "Item("+ID+", "+title+")";

    public bool IsntID => ID == null || ID == "";

    public bool GetNewID() {
        if(IsntID || true){
            string id = Guid.NewGuid().ToString();
            if (ItemsMananger.AddNewID(id)) {
                ID = id;
                return true;
            } else
                return false;
        } else
            return false;
    }

    public void EnsureMyself() {
        bool notOK = false;
        string message = "Item "+gameObject.name+" has some problems: ";
        if (IsntID) {
            notOK = true;
            message += "[It didn't get ID inizialitated, use \"Generar ID\" on editor first] ";
        }
        if (gameObject.GetComponent<Image>() == null) {
            notOK = true;
            message += "[It needs \"Image\" component to work.] ";
        }
        if (notOK)
            throw new InvalidOperationException(message);
    }

    void Start() {
        EnsureMyself();
        canvas = GameStateEngine.gse.canvas;
        inventario = GameStateEngine.gse.oi;
    }
    

    ////////////////////////////
    /* RUN TIME FUNCTIONALLYTY*/
    ////////////////////////////

    // Algunos items podría devolverse a sí mismo en vez de una copia
    public Item GetMe(Transform parent = null, Vector2 position = new Vector2()) {
        GameObject go = Instantiate(gameObject, new Vector3(), new Quaternion(), parent);
        go.GetComponent<RectTransform>().anchoredPosition = position;
        return go.GetComponent<Item>();
    }

    // DRAG stuff

    public void OnBeginDrag(PointerEventData eventData) {
        // permite que se puedan seleccionar objetos detrás del item cogido
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        originalPosition = transform.position;
    }

    public void OnDrag(PointerEventData data) {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            data.position,
            canvas.worldCamera,
            out pos);
        transform.position = canvas.transform.TransformPoint(pos);
    }

    public void OnEndDrag(PointerEventData eventData) {
		if (SlotManager.lastTouched >= 0)
			inventario.Move(ID, SlotManager.lastTouched);
        else
            transform.position = originalPosition;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
}
