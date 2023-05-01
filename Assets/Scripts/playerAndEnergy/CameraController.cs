using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    const float ENOUGH_DISTANCE =0.01f;

    public GameObject player;

    private Transform pt;
    private Camera camara;
    private float defaultSize;

    private bool isInTransition;
    private bool isFollowMode = true;

    private Vector3 goal;
    private Vector3 pos {get=>transform.position; set=>transform.position=value;}
    private float transTime;
    //public float cameraSize = 5f;

    private Camera mainCamera;

    void Start() {
        pt = player.transform;
        mainCamera = GetComponent<Camera>();
        // defaultSize = camara.orthographicSize;
        goal = new Vector3(pos.x,pos.y,pos.z);

    }

    void Update() {
        // if(Input.GetKey(KeyCode.H))
        //     camara.orthographicSize = camara.orthographicSize + 1*Time.deltaTime;

        // if(Input.GetKey(KeyCode.G))
        //     camara.orthographicSize = camara.orthographicSize - 1*Time.deltaTime;

        // if(Input.GetKey(KeyCode.J))
        //     camara.orthographicSize = defaultSize;

        if (isFollowMode) {
            pos = new Vector3(pt.position.x, pt.position.y, pos.z);
        }else if (isInTransition) {
            pos = pos + (goal-pos)/transTime*Time.deltaTime; // cinemática
            transTime-=Time.deltaTime;
            if (Vector3.Distance(pos, goal) < ENOUGH_DISTANCE)
                isInTransition = false;
        }

        if(Input.GetKeyUp("h")){
            if (isFollowMode)
                ChangeCamara(0,0,0,2);
            else
                StalkMaleciously();
        }

    }

    public void StalkMaleciously(){
        isFollowMode = true;
    }

    public void ChangeCamara(float x, float y, float zoom, float seconds){
        if(seconds <= 0){
            Debug.LogError("No se pudo hacer la transición de cámara porque el tiempo es menor o igual a 0.");
            return;
        }
        goal = new Vector3(x, y, goal.z);
        isInTransition = true;
        isFollowMode = false;
        transTime = seconds;
    }
}
