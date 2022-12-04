using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    int score = 0;
    public static ScoreManager scoreManager;
    public Text socreText;

    void Start() {
        if (scoreManager == null) {
            scoreManager = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    void Update() {
        if (socreText == null) {
            socreText = GameObject.Find("Text").GetComponent<Text>();
            socreText.text = ""+score;
        }
    }

    public void raiseScore (int incr) {
        score += incr;
        Debug.Log(score);
        socreText.text = ""+score;
        if (score == 3) {
            SceneManager.LoadScene("SampleScene2");
        }
    }
}
