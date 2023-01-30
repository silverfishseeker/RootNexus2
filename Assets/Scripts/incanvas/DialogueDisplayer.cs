using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DialogueDisplayer : MonoBehaviour {
    public const string NEXT_BUTTOM = "Jump";

    public GameObject textTMP;

    private GameObject cloudDialogue;
    private TextMeshProUGUI tmp;
    private bool isSpeaking;

    private INotificableDialogue notificate;

    void Start() {
        tmp = textTMP.GetComponent<TextMeshProUGUI>();
        isSpeaking = false;
        cloudDialogue = gameObject.transform.GetChild(0).gameObject;
        cloudDialogue.SetActive(false);
        enabled  = false;
    }
    
    void Update() {
        if (Input.GetButtonUp(NEXT_BUTTOM) && isSpeaking) {
            cloudDialogue.SetActive(false);
            isSpeaking = false;
            notificate.NotificateMe();
            enabled  = false;
        }
    }

    public void Show(string text, INotificableDialogue notificate){
        enabled  = true;
        this.notificate = notificate;
        tmp.text = text;
        isSpeaking = true;
        cloudDialogue.SetActive(true);
    }

}
