using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateEngine : MonoBehaviour {
    // STATIC
    public static bool isntPaused;
    public static bool isntInventory;
    public static bool isntEitherPause => isntPaused && isntInventory;
    public static GameStateEngine gse;

    public static void GameOver() {
        Pause();
        gse.gameOverImage.SetActive(true);
    }

    public static void LoadScene(string nombreEscena, int entrada = 0){
        gse.pc.SaveToCache();
        gse.idEntrada = entrada;
        SceneManager.LoadScene(nombreEscena);
        // Mirar OnSceneLoaded()
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
        gse.bhb.IncreaseRegeneration();
        GeneralPause();
    }
    
    public static void CerrarInventario(){
        isntInventory = true;
        gse.inventario.SetActive(false);
        gse.bhb.DecreaseRegeneration();
        GeneralResume();
    }

    // NON STATIC
    public IBaseAction accionPrimera;
    private ActionSetter actionSetter;
    public GameObject gameOverImage;
    public HealthBarController hbc;
    public BigHealthBar bhb;
    public DialogueDisplayer dd;
    public Canvas canvas;
    public GameObject inventario;
    public ObjetosInventario oi;
    public GameObject avatar;
    public PersonajesCache pc;
    public Radio radio;
    private int idEntrada = 0;

    void OnEnable() {
        // singleton
        if (gse != null) {
            Destroy(gameObject);
            return;
        }
        gse = this;
        DontDestroyOnLoad(gse);

        SceneManager.sceneLoaded += OnSceneLoaded;
        oi.PreStart();
        gameOverImage.SetActive(false);
        CerrarInventario();
        Resume();

        if (accionPrimera != null){
            actionSetter = gameObject.AddComponent<ActionSetter>();
            actionSetter.first = accionPrimera;
            actionSetter.Run();
        }
        
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        pc.LoadToScene();

        if(idEntrada == 0)
            avatar.transform.position = new Vector2(0,0);
        else {
            foreach (EntradaDeNivel edn in FindObjectsOfType<EntradaDeNivel>()){
                if(edn.id == idEntrada){
                    avatar.transform.position = edn.GetComponent<Transform>().position; 
                    avatar.GetComponent<SpriteRenderer>().flipX = edn.facingLeft;
                    edn.Run();
                    return;
                }
            }
            Debug.LogError("No existe una entrada con el id: "+idEntrada);
        }
    }

    void Update(){ //BORRAR
        if (Input.GetKeyUp("l")) {
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
