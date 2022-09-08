using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MazeCreator : MonoBehaviour
{

    public float startx = -10, starty = -10;

    public GameObject wallPrefab;

    void Start() {
        string path = "Assets/textFiles/maze.txt";
        //Read the text from directly from the test.txt file
        StreamReader sr = new StreamReader(path);
        float i = startx, j = starty;
        string line;
        while ((line = sr.ReadLine()) != null){
            i = startx;
            foreach (char c in line){
                if (c == '1')
                    Instantiate(wallPrefab, new Vector3(i, j, 0), new Quaternion());
                i++;
            }
            j--;
        }
        sr.Close();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
