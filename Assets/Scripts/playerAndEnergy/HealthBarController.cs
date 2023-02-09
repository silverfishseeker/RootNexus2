using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBarController : MonoBehaviour {

    private float inicialWidth;
    public float max;
    public float coeficiente;
    public float regeneration;
    public float curr;
    private bool cansado;

    public BigHealthBar bhb;
    private PlayerMovement player;
    
    void Start()  {
        player = GameStateEngine.gse.avatar.GetComponent<PlayerMovement>();
        inicialWidth = transform.localScale.x;
        Reset();
    }

    public void Reset(){
        curr = max;
        transform.localScale = new Vector3(inicialWidth, transform.localScale.y, transform.localScale.z);
    }

    public void Add(float value) {
        curr += value*coeficiente;
        if (curr <= 0) {
            bhb.Add(curr);
            curr = 0;
            player.Cansar();
            cansado = true;
        } else {
            if (curr > max)
                curr = max;
            float newWidth = curr / max * inicialWidth;
            transform.localScale = new Vector3(newWidth, transform.localScale.y, transform.localScale.z);
        }
    }

    public void AddDelta(float value) => Add(value*Time.deltaTime);

    void Update() {
        //regeneration
        if (GameStateEngine.isntPaused)
            Add(regeneration);

        if(cansado && curr == max){
            cansado = false;
            player.Descansar();
        }


        // Testing bar
        if (Input.GetKeyUp("u")) {
            curr = max;
            transform.localScale = new Vector3(inicialWidth, transform.localScale.y, transform.localScale.z);
        }
        if (Input.GetKeyUp("i")) {
            Add(-10f);
        }
        if (Input.GetKeyUp("o")) {
            Add(10f);
        }
    }
}
