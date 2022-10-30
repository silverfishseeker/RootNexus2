using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateEngine : MonoBehaviour
{
    public GameObject myself;
    public GameObject gameOverImage;
    public List<GameObject> killMe;

    private Scene scene;

    // Start is called before the first frame update
    void Start() {
		//Le indico que no se destruya al cargar otra escena 
		DontDestroyOnLoad(myself);
        gameOverImage.SetActive(false);
        scene = SceneManager.GetActiveScene();

        // everyoneElse = new List<GameObject>();
        // foreach(GameObject go in scene.GetRootGameObjects()){
        //     if (!(go.Equals(myself) || excludedObjects.Contains(go))) {
        //         everyoneElse.Add(go);
        //     }
        // }
    }

    public void GameOver() {
        foreach (GameObject go in killMe) {
            go.SetActive(false);
        }
        gameOverImage.SetActive(true);
    }

    public void Reload() {
        SceneManager.LoadScene(scene.name);
        //gameOverImage.SetActive();
    }

    public void ShutDown() {
        Application.Quit();
    }
}
