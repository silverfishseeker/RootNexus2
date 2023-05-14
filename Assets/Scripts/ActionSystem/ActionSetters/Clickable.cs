using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Events;

public class Clickable : Activable {
    public float distancia;

    public GameObject resaltado;
    private SpriteRenderer rsp;
    private bool isNear => distancia > Vector2.Distance(gameObject.transform.position, GameStateEngine.gse.avatar.transform.position);

    protected void Start() {
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
        if (isNear) {
            Run();
        }
    }

}
