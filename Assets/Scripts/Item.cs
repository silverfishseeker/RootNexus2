using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : MonoBehaviour {

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

    void start() {
        if (ID == null){
            throw new InvalidOperationException("Item "+gameObject.name+" didn't got ID inizialitated, use Generar ID on editor first");
        }
    }
    
}
