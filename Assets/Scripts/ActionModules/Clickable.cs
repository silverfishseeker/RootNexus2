using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Events;

public class Clickable : Activable {
    public float distancia;

    private GameObject resaltado;
    private SpriteRenderer rsp;
    private bool isAvalaible => GameStateEngine.isntEitherPause && active;
    private bool isNear => distancia > Vector2.Distance(gameObject.transform.position, GameStateEngine.gse.avatar.transform.position);

    new protected void Start() {
        base.Start();
        resaltado = gameObject.transform.GetChild(0).gameObject;
        rsp = resaltado.GetComponent<SpriteRenderer>();
        resaltado.SetActive(false);
    }

    void OnMouseEnter() {
        if(isAvalaible)
            resaltado.SetActive(true);
    }

    void OnMouseExit() {
        resaltado.SetActive(false);
    }

    new void Update() {
        base.Update();
        if (isNear)
            rsp.color = Color.white;
        else
            rsp.color = Color.gray;
    }

    void OnMouseDown() {
        if (isAvalaible && isNear) {
            Run();
        }
    }

}