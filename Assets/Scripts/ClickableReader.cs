using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableReader : MonoBehaviour
{
    public string message;
    public GameObject avatar;
    public float distancia;

    private DialogueDisplayer dd;
    private GameObject resaltado;
    private SpriteRenderer rsp;

    void Start() {
        dd = GameStateEngine.gse.dd;
        resaltado = gameObject.transform.GetChild(0).gameObject;
        rsp = resaltado.GetComponent<SpriteRenderer>();
        resaltado.SetActive(false);
    }

    void OnMouseEnter() {
        resaltado.SetActive(true);
    }

    void OnMouseExit() {
        resaltado.SetActive(false);
    }

    void Update() {
        if (distancia > Vector2.Distance(gameObject.transform.position, avatar.transform.position)) {
            rsp.color = Color.white;
        } else {
            rsp.color = Color.gray;
        }
    }
    void OnMouseDown() {
        if (distancia > Vector2.Distance(gameObject.transform.position, avatar.transform.position)) {
            dd.Load(message);
            dd.Next();
        }
    }
}
