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

    private TextMeshProUGUI tmp;
    private string text{
        get{return tmp.text;}
        set{tmp.text = value;}
    }
    private string current;
    private List<string> boxes = new List<string>();
    private int next = 0;

    void Start() {
        tmp = textTMP.GetComponent<TextMeshProUGUI>();
        current = null;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);

        Load("ralph");
    }
    
    void Update() {
        if (Input.GetKeyUp("g"))
            Next();
    }

    public void Load(string fileName) {
        using(StreamReader reader = new StreamReader(speakingsPath + fileName + ".txt")) {
            boxes = new List<string>();
            string box = "";
            while (!reader.EndOfStream) {
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
    }

    public bool Next() {
        if (next >= boxes.Count) {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            return false;
        } else {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            text = boxes[next++];
            return true;
        }
    }

    public void Clear() {
            text = "";
    }

}
