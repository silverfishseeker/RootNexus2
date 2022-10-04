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
    private List<string> boxes;
    private int next = 0;

    IEnumerator Wait(int time){
        yield return new WaitForSeconds(time);
    }

    void Start() {
        tmp = textTMP.GetComponent<TextMeshProUGUI>();
        current = null;

        Display("ralph");
        //StartCoroutine(Wait(2));
        // Wait(2);
        Display("ralph");
        // Wait(2);
        // Display("ralph");
        // Wait(2);
    }

    bool Display(string fileName) {
        if (fileName != current)
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
        try {
            text = boxes[next++];
            return true;
        } catch (ArgumentOutOfRangeException) {
            return false;
        }
    }
}
