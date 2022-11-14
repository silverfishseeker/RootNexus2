using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateEngine : MonoBehaviour {
    public GameObject gameOverImage;
    //public GameObject healthBar;
    public HealthBarController hbc;
    public DialogueDisplayer dd;

    /*CUIDADO la referencia de avatar se instancia dentro de su 
    propio controlador y no en esta clase. Además Avatar no forma
    parte de GameBase. Eso quiere decir que el objeto puede
    dejar de exsistir y que de un error cuando se intente acceder
    a este atributo. Esto puede ser una fuente de posibles bugs.
    Mirar si en un futuro hay que meter Avatar en GameBase*/
    public GameObject avatar;
    
    //public List<GameObject> killMe;
    public static bool isPaused;

    private static Scene scene;

    public static GameStateEngine gse;

    void Start() {
        // singleton
        if (gse == null) {
            gse = this;
            DontDestroyOnLoad(gse);
        } else {
            Destroy(gameObject);
        }

        scene = SceneManager.GetActiveScene();
        Resume();
        gse.gameOverImage.SetActive(false);
    }

    public static void GameOver() {
        // foreach (GameObject go in gse.killMe) {
        //     go.SetActive(false);
        // }
        Pause();
        gse.gameOverImage.SetActive(true);
    }

    public static void Reload() {
        SceneManager.LoadScene(scene.name);
        gse.hbc.Reset();
        gse.gameOverImage.SetActive(false);
    }

    public static void ShutDown() {
        Application.Quit();
    }
    
    // https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/#pause_time_scale
    public static void Pause() {
        isPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }
    public static void Resume() {
        isPaused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    void Update() {
        if (Input.GetKeyUp("l")) {
            if (isPaused) Resume();
            else Pause();
        }
    }
}