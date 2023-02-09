using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateEngine : MonoBehaviour {

    public GameObject gameOverImage;
    //public GameObject healthBar;
    public HealthBarController hbc;
    public DialogueDisplayer dd;
    public Canvas canvas;
    public GameObject inventario;
    public ObjetosInventario oi;
    public GameObject avatar;
    
    public static bool isntPaused;
    public static bool isntInventory;
    public static bool isntEitherPause => isntPaused && isntInventory;

    public static GameStateEngine gse;

    private static bool justLoaded;
    private static string idEntrada;

    void Start() {
        // singleton
        if (gse != null) {
            Destroy(gameObject);
        } else {
            gse = this;
            DontDestroyOnLoad(gse);
            oi.PreStart();
            Resume();
        }
        gse.gameOverImage.SetActive(false);
        CerrarInventario();
    }

    public static void GameOver() {
        Pause();
        gse.gameOverImage.SetActive(true);
    }

    public static void LoadScene(string nombreEscena, string entrada = ""){
        idEntrada = entrada;
        SceneManager.LoadScene(nombreEscena);
        justLoaded = true;
        gse.enabled = true;
        Resume();
    }

    public static void Reload() {
        gse.hbc.Reset();
        gse.gameOverImage.SetActive(false);
        LoadScene(SceneManager.GetActiveScene().name);
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
        GeneralPause();
    }
    
    public static void CerrarInventario(){
        isntInventory = true;
        gse.inventario.SetActive(false);
        GeneralResume();
    }

    void Update() {
        if (justLoaded){
            //enabled = false //descomentar en versión final
            justLoaded = false;

            GameObject[] entradas = GameObject.FindGameObjectsWithTag("entrada");

            if(idEntrada == "")
                gse.avatar.transform.position = new Vector2(0,0);
            else {
                bool isAny = false;
                foreach (GameObject go in entradas){
                    EntradaDeNivel edn = go.GetComponent<EntradaDeNivel>();
                    if(edn.nombre == idEntrada){
                        gse.avatar.transform.position = edn.GetComponent<Transform>().position; 
                        gse.avatar.GetComponent<SpriteRenderer>().flipX = edn.facingLeft;
                        isAny = true;
                        break;
                    }
                }
                if( !isAny)
                    Debug.LogError("No existe una entrada con el id especificado");
            }
        }
        if (Input.GetKeyUp("l")) {//descomentar arriba al borrar
                Debug.Log(SceneManager.GetActiveScene().name);
            if (isntPaused) Pause();
            else Resume();
        }
        if (isntPaused && Input.GetKeyUp("e")) {
            if (isntInventory) AbrirInventario();
            else CerrarInventario();
        }
    }
}
