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

        Load("ralph");
        //StartCoroutine(Wait(2));
        // Wait(2);
        // Wait(2);
        // Display("ralph");
        // Wait(2);
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
            foreach (string s in boxes)
                Debug.Log(s);
        }
    }

    public bool Next() {
        if (next > boxes.Count)
            return false;
        if (next == boxes.Count)
            text = "";
        else
            text = boxes[next++];
        return true;
    }

    public void Clear() {
            text = "";
    }

}
