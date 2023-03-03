using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntradaDeNivel : MonoBehaviour {
    public int id;
    public bool facingLeft;

    void Start(){
        if(id == 0)
            Debug.LogError("Una entrada de nivel no puede tener un id 0");
    }
}
