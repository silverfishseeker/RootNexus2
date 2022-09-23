using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DialogueDisplayer : MonoBehaviour
{
    public GameObject textTMP;

	public string speakingsPath;

    private TextMeshProUGUI tmp;
    private string text{
        get{return tmp.text;}
        set{tmp.text = value;}
    }
    void Start()
    {
        tmp = textTMP.GetComponent<TextMeshProUGUI>();
        Debug.Log(text);
        string path = "Assets/UI/Dialogue/Speakings/Example.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path); 
        text = reader.ReadToEnd();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
