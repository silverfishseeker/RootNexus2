using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateEngine : MonoBehaviour {

    public ActionSetter ass; // Temporal


    public GameObject gameOverImage;
    //public GameObject healthBar;
    public HealthBarController hbc;
    public DialogueDisplayer dd;
    public Canvas canvas;
    public GameObject inventario;
    public ObjetosInventario oi;

    /*CUIDADO la referencia de avatar se instancia dentro de su 
    propio controlador y no en esta clase. Además Avatar no forma
    parte de GameBase. Eso quiere decir que el objeto puede
    dejar de exsistir y que de un error cuando se intente acceder
    a este atributo. Esto puede ser una fuente de posibles bugs.
    Mirar si en un futuro hay que meter Avatar en GameBase*/
    public GameObject avatar;
    
    public static bool isntPaused;
    public static bool isntInventory;
    public static bool isntEitherPause => isntPaused && isntInventory;

    public static GameStateEngine gse;

    void LoadGame() {
        oi.PreStart();
        Resume();
    }

    void Start() {
        // singleton
        if (gse != null) {
            Destroy(gameObject);
        } else {
            gse = this;
            DontDestroyOnLoad(gse);
            LoadGame();
        }
        gse.gameOverImage.SetActive(false);
        CerrarInventario();
    }

    public int nextId = 0;
    public int GetNewId() => nextId++;

    public static void GameOver() {
        Pause();
        gse.gameOverImage.SetActive(true);
    }

    public static void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gse.hbc.Reset();
        gse.gameOverImage.SetActive(false);
        Resume();
    }

    public static void ShutDown() {
        Application.Quit();
    }
    

    // https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/#pause_time_scale
    private static void GeneralPause(){
        Time.timeScale = 0f;
        AudioListener.pause = true;
        gse.avatar.GetComponent<PlayerMovement>().enabled = false;
    }
    private static void GeneralResume(){
        Time.timeScale = 1f;
        AudioListener.pause = false;
        gse.avatar.GetComponent<PlayerMovement>().enabled = true;
    }

    public static void Pause() {
        isntPaused = false;
        GeneralPause();
    }
    public static void Resume() {
        isntPaused = true;
        if(isntInventory)
            GeneralResume();
    }

    public static void AbrirInventario(){
        isntInventory = false;
        gse.inventario.SetActive(true);
        gse.hbc.IncreaseRegeneration();
        GeneralPause();
    }
    
    public static void CerrarInventario(){
        isntInventory = true;
        gse.inventario.SetActive(false);
        gse.hbc.DecreaseRegeneration();
        GeneralResume();
    }

    void Update() {
        if (Input.GetKeyUp("l")) {
            if (isntPaused) Pause();
            else Resume();
        }
        if (isntPaused && Input.GetKeyUp("e")) {
            if (isntInventory) AbrirInventario();
            else CerrarInventario();
        }
        
        if (Input.GetKeyUp("z")) { // Temporal
            ass.Run();
        }
    }
}
