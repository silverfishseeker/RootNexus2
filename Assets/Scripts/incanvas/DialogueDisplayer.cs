using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DialogueDisplayer : MonoBehaviour {
    public const string NEXT_BUTTOM = "Jump";


    public TextMeshProUGUI tmp;
    public GameObject opcionDialogo;
    public float desplazamiento;
    private GameObject cloudDialogue;
    private bool isSpeaking;
    private bool isOptions;
    private Stack<GameObject> opciones = new Stack<GameObject>();
    [HideInInspector]
    public IBaseAction chosen;
    public IBaseAction invalidReference;
    private INotificableDialogue notificate;

    void Start() {
        isSpeaking = false;
        isOptions = false;
        cloudDialogue = gameObject.transform.GetChild(0).gameObject;
        cloudDialogue.SetActive(false);
        enabled  = false;
        chosen = invalidReference;
    }
    
    void Update() {
        DialogueOption doCurr;
        if (Input.GetButtonUp(NEXT_BUTTOM) && isSpeaking && (!isOptions || 
                ((
                    chosen = 
                        ((doCurr = ExclusivityManager.Current<DialogueOption>()) != null ? 
                            doCurr.action :
                            invalidReference
                        )
                ) != invalidReference)
            )) { // Admito que me he pasado un poco en este if
            // En resumen entramos en el if si el espacio presionado, está hablando y, si estamos en modo opciones, entonces necisitamos una acción escogida correctamente
            cloudDialogue.SetActive(false);
            isSpeaking = false;
            isOptions = false;

            while(opciones.Count > 0)
                Destroy(opciones.Pop());

            notificate.NotificateMe();
            enabled  = false;
            chosen = invalidReference;
        }
    }

    public void Show(string text, INotificableDialogue notificate){
        enabled  = true;
        this.notificate = notificate;
        tmp.text = text;
        isSpeaking = true;
        cloudDialogue.SetActive(true);
    }

    public void ShowOptions(string text, List<string> textos, List<IBaseAction> acciones, List<AbstractConditionAction> condiciones, INotificableDialogue notificate){
        if (acciones.Count != acciones.Count)
            throw new ArgumentException("textos y acciones deben de tener la misma longitud");
        Show(text, notificate);
        isOptions = true;
        int incrementoDesplazamiento = 0;
        for(int i = 0; i < textos.Count; i++)
            if (i >= condiciones.Count || condiciones[i] == null || condiciones[i].Condition()){ // nótese que aquí ignoramos los valores de next y alternativa en la action
                GameObject go = Instantiate(opcionDialogo, transform);
                go.GetComponent<DialogueOption>().SetTextAndAction(textos[i], acciones[i]);
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    go.GetComponent<RectTransform>().anchoredPosition.x,
                    go.GetComponent<RectTransform>().anchoredPosition.y+desplazamiento*(incrementoDesplazamiento++));
                opciones.Push(go);
            } 
    }
}
