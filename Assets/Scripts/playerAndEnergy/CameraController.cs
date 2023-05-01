using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;

    private Transform pt;
    private Camera camara;
    private float defaultSize;

    private bool isInTransition;
    public bool isFollowMode = true;

    private GameObject goalCoords;
    public Vector3 goal{ get=>goalCoords.transform.position; set=>goalCoords.transform.position = value;}
    public float cameraSpeed = 1f;
    public float cameraSize = 5f;

    private Camera mainCamera;

    void Start() {
        pt = player.transform;
        mainCamera = GetComponent<Camera>();
        // defaultSize = camara.orthographicSize;
        goalCoords = new GameObject("camera goal coords");
        goalCoords.transform.position = new Vector3(0,0,transform.position.z);

    }

    void Update() {
        // if(Input.GetKey(KeyCode.H))
        //     camara.orthographicSize = camara.orthographicSize + 1*Time.deltaTime;

        // if(Input.GetKey(KeyCode.G))
        //     camara.orthographicSize = camara.orthographicSize - 1*Time.deltaTime;

        // if(Input.GetKey(KeyCode.J))
        //     camara.orthographicSize = defaultSize;

        // if (isInTransition){
        //     float distIncr = velocidad * Time.deltaTime;
        //     float newX = transform.position.x + distIncr * Mathf.Cos(direccion);
        //     float newY = transform.position.y + distIncr * Mathf.Sin(direccion);
        //     if (newX > xGoal)
        //         newX = xGoal;
        //     if (newY > yGoal)
        //         newY = yGoal;
            
        //     transform.position = new Vector3(newX, newY, transform.position.z);
        // }

        if (isFollowMode) {
            transform.position = new Vector3(pt.position.x, pt.position.y, transform.position.z);
        }else if (isInTransition) {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraSize, cameraSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, goal, cameraSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, goal) < 0.1f) {
                isInTransition = false;
            }
        }

        if(Input.GetKeyUp("h")){
            if (isFollowMode)
                ChangeCamara(0,0,0,0);
            else
                StalkMaleciously();
        }

    }

    public void StalkMaleciously(){
        isFollowMode = true;
    }

    public void ChangeCamara(float x, float y, float zoom, float seconds){
        goal = new Vector3(x, y, goal.z);
        isInTransition = true;
        isFollowMode = false;
        // xGoal = x;
        // yGoal = y;
        // float xD = x - transform.position.x;
        // float yD = y - transform.position.y;
        
        // velocidad = Mathf.Sqrt(xD*xD + yD*yD) / seconds;
        // direccion = Mathf.Atan(yD / xD);
    }
}
