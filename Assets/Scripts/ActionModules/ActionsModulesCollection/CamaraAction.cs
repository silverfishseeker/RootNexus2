using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraAction : IBaseAction {

    private CameraController camara;
    public bool isDefault;
    public bool coordenadasRelativas;
    public Vector2 posicion;
    public float tamaño;
    public float tiempo;

    void Start(){
        camara = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }

    protected override void SubRun(){
        if(tiempo == 0)
            tiempo = 0.001f;
        if(isDefault)
            camara.DefaultCamera(tiempo);
        else{
            if (coordenadasRelativas)
                camara.StalkMaleciously(posicion.x, posicion.y, tamaño, tiempo);
            else
                camara.ChangeCamara(posicion.x, posicion.y, tamaño, tiempo);
        }

    }
}
