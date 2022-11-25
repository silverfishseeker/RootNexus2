using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Events;

public abstract class IClickable : MonoBehaviour
{
    public float distancia;

    private GameObject resaltado;
    private SpriteRenderer rsp;
    public bool active = true;

    protected void Start() {
        resaltado = gameObject.transform.GetChild(0).gameObject;
        rsp = resaltado.GetComponent<SpriteRenderer>();
        resaltado.SetActive(false);
    }

    void OnMouseEnter() {
        if(active)
            resaltado.SetActive(true);
    }

    void OnMouseExit() {
        resaltado.SetActive(false);
    }

    void Update() {
        if (distancia > Vector2.Distance(gameObject.transform.position, GameStateEngine.gse.avatar.transform.position)) {
            rsp.color = Color.white;
        } else {
            rsp.color = Color.gray;
        }
    }

    void OnMouseDown() {
        if (active && distancia > Vector2.Distance(gameObject.transform.position, GameStateEngine.gse.avatar.transform.position)) {
            Action();
        }
    }

    public abstract void Action();
}
