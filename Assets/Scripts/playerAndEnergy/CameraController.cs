using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    const float ENOUGH_DISTANCE = 0.01f;
    const float Z_DISPLACEMENT = -1;

    public GameObject player;

    private GameObject stalkPoint;
    private Camera camara;

    private bool isInTransition;
    private bool isFollowMode = true;

    private float transTime;
    private Vector3 pos {get=>transform.position; set=>transform.position=value;}
    private bool isStaticGoal;
    private Vector3 goalVal;
    private Vector3 goal {
        get => isStaticGoal ? goalVal : stalkPoint.transform.position;
        set {
            isStaticGoal = true;
            goalVal = value;
        }
    }
    private float defaultSize;
    private float size {get=>camara.orthographicSize; set=>camara.orthographicSize=value;}
    private float goalSize;


    void Start() {
        camara = GetComponent<Camera>();
        defaultSize = camara.orthographicSize;
        isFollowMode = true;

        stalkPoint = new GameObject("stalkPoint");
        stalkPoint.transform.parent = player.transform;
        stalkPoint.transform.localPosition = new Vector3(0,0,Z_DISPLACEMENT);
    }

    public void DefaultCamera(float seconds){
        StalkMaleciously(0, 0, defaultSize, seconds);
    }

    public void StalkMaleciously(float x, float y, float zoom, float seconds) {
        stalkPoint.transform.localPosition = new Vector3(x,y,Z_DISPLACEMENT);
        ChangeCamara(new Vector3(), zoom, seconds);
        isStaticGoal = false;
        isFollowMode = true;
    }

    public void ChangeCamara(float x, float y, float zoom, float seconds) => ChangeCamara(new Vector3(x, y, Z_DISPLACEMENT), zoom, seconds);
    
    public void ChangeCamara(Vector3 pos, float zoom, float seconds) {
        if(seconds < 0){
            Debug.LogError("No se pudo hacer la transición de cámara porque el tiempo es menor a 0.");
            return;
        }
        goal = pos;
        goalSize = zoom;
        transTime = seconds;

        isInTransition = true;
        isFollowMode = false;
    }

    void Update() {
        if (isInTransition) {  
            //pos  = pos  + (goal    -pos )/transTime*Time.deltaTime; // cinemática
            float timeCoeff = Time.deltaTime / transTime;
            pos  = Vector3.Lerp(pos, goal, Vector3.Distance(goal,pos) * timeCoeff);
            size = Mathf.Lerp(size, goalSize, Mathf.Abs(goalSize-size) * timeCoeff);            

            transTime-=Time.deltaTime;
            if (Vector3.Distance(pos, goal) < ENOUGH_DISTANCE)
                isInTransition = false;
            else if (transTime < 0)
                transTime+=Time.deltaTime;

        }else if (isFollowMode) {
            pos = stalkPoint.transform.position;
        }
    }
}
