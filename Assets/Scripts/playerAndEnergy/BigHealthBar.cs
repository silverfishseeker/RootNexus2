using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class BigHealthBar : MonoBehaviour {
    private Image healthBarImage;
    private HealthBarController hbc;
    public float max;
    public float current;
    public float coeficienteContinuo;
    public float coeficienteGolpe;
    public float regeneration;
    public float highRegeneration;
    private float currRegen;

    //Color
    private Color baseColor;
    public Color maxColor;
    private Color diffColor => maxColor-baseColor;
    public float coeficienteCambioColor;
    public float desplazamientoCambioColor;
    public float duracionColor;

    private float currIncr = 0;
    private float correction => 1f/(1f+(float)Math.Exp(coeficienteCambioColor*desplazamientoCambioColor)); // para asegurar la función en (0,0)

    void Start(){
        hbc = GameStateEngine.gse.hbc;
        healthBarImage = GetComponent<Image>();
        baseColor = healthBarImage.color;
        current = max;
        currRegen = regeneration;
    }
    public void Add(float value) {
        currIncr += value*coeficienteGolpe;
        current += value*coeficienteGolpe;
    }

    public void IncreaseRegeneration() {
        currRegen = highRegeneration;
    }
    public void DecreaseRegeneration() {
        currRegen = regeneration;
    }

    void Update() {
        float f = GameStateEngine.isntInventory ?
            ((hbc.curr-hbc.max) * coeficienteContinuo + currRegen) * Time.deltaTime :
            currRegen;
        current += f;
        currIncr += f;
        if (current <= 0) {
            GameStateEngine.GameOver();
        } else if (current > max)
            current = max;
        healthBarImage.fillAmount = current/max; // 0,1

        //color
        f = -( // función sigmoide
            (1f / (1f+(float)Math.Exp(
                coeficienteCambioColor*(desplazamientoCambioColor-currIncr)
            ))) * (1f+correction)-correction
        );
        healthBarImage.color = new Color(f,f,f,1) * diffColor + baseColor;
        currIncr *=duracionColor;
    }
}