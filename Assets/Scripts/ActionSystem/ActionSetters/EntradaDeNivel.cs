using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntradaDeNivel : ActionSetter {
    public int id;
    public bool facingLeft;

    void Start(){
        if(id == 0)
            Debug.LogError("Una entrada de nivel no puede tener un id 0");
    }
    public override void Run(){
        if (first == null)
            return;
        base.Run();
    }

}
