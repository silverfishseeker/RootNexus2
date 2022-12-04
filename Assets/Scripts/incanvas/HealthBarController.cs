using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBarController : MonoBehaviour {

    private float inicialWidth;
    public float max;
    public float coeficiente;
    public float curr;
    public Color baseColor;
    public Color maxColor;
    private Color diffColor => maxColor-baseColor;
    public float coeficienteCambioColor;
    public float desplazamientoCambioColor;
    public float duracionColor;

    private float currIncr = 0;
    private float correction => 1f/(1f+(float)Math.Exp(coeficienteCambioColor*desplazamientoCambioColor)); // para asegurar la función en (0,0)
    private RectTransform rt;

    void Start()  {
        rt = gameObject.GetComponent<RectTransform>();
        inicialWidth = rt.localScale.x;
        Reset();
    }

    public void Reset(){
        curr = max;
        rt.localScale = new Vector3(inicialWidth, rt.localScale.y, rt.localScale.z);
    }

    public void Add(float value) {
        curr += value*coeficiente;
        currIncr += value*coeficiente;
        if (curr <= 0) {
            GameStateEngine.GameOver();
        } else {
            if (curr > max)
                curr = max;
            float newWidth = curr / max * inicialWidth;
            rt.localScale = new Vector3(newWidth, rt.localScale.y, rt.localScale.z);
        }
    }

    public void AddDelta(float value) => Add(value*Time.deltaTime);

    // función sigmoide
    private float SigmoidInOO(float value) =>
        (1f / (1f+(float)Math.Exp(
            coeficienteCambioColor*(desplazamientoCambioColor-value)
        ))) * (1f+correction)-correction;
    

    void Update() {
        float f = SigmoidInOO(-currIncr);
        GetComponent<Image>().color = new Color(f,f,f,1) * diffColor + baseColor;
        currIncr *=duracionColor;

        // Testing bar
        if (Input.GetKeyUp("u")) {
            curr = max;
            rt.localScale = new Vector3(inicialWidth, rt.localScale.y, rt.localScale.z);
        }
        if (Input.GetKeyUp("i")) {
            Add(-10f);
        }
        if (Input.GetKeyUp("o")) {
            Add(10f);
        }
    }
}
