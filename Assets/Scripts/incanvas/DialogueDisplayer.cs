using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DialogueDisplayer : MonoBehaviour
{
    public GameObject textTMP;

	public string speakingsPath;
    public int linesPerBox;
    public int charsPerLine;


    private GameObject cloudDialogue;
    private TextMeshProUGUI tmp;
    private string text{
        get{return tmp.text;}
        set{tmp.text = value;}
    }
    private string current;
    private List<string> boxes = new List<string>();
    private int next = 0;
    private bool isSpeaking;
    private bool justStopedSpeaking = false;

    void Start() {
        tmp = textTMP.GetComponent<TextMeshProUGUI>();
        current = null;
        isSpeaking = false;
        cloudDialogue = gameObject.transform.GetChild(0).gameObject;
        cloudDialogue.SetActive(false);
    }
    
    void Update() {
        if (Input.GetButtonUp("Jump")) {
            if (isSpeaking)
                Next();
        } else if (justStopedSpeaking){
            GameStateEngine.Resume();
            justStopedSpeaking = false;
        }

    }

    public void Load(string fileName) {
        using(StreamReader reader = new StreamReader(speakingsPath + fileName + ".txt")) {
            boxes = new List<string>();
            string box = "";
            while (true) {
                if(reader.EndOfStream) {
                    boxes.Add(box);
                    break;
                }

                string line = reader.ReadLine();
                if (line == "/") {
                    boxes.Add(box);
                    box = "";
                } else {
                    box+=line+"\n";
                }
            }
            current = fileName;
            next = 0;
        }
        GameStateEngine.Pause();
        isSpeaking = true;
    }

    public void Next() {
        if (next >= boxes.Count) {
            cloudDialogue.SetActive(false);
            isSpeaking = false;
            justStopedSpeaking = true;
        } else {
            cloudDialogue.SetActive(true);
            text = boxes[next++];
        }
    }

    public void Clear() {
            text = "";
    }

}
