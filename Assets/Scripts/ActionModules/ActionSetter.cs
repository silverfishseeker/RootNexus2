using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSetter : MonoBehaviour {

    public IBaseAction first;
    
    public IBaseAction current;

    void Start(){
        enabled = false;
        current = null;
    }

    public void Run(){
        GameStateEngine.Pause();
        current = first;
        current.Run();
        enabled = true;
    }

    void Update(){
        if(current != null && current.isFinished) {
            current = current.next;
            if (current == null){
                enabled = false;
                GameStateEngine.Resume();
            } else
                current.Run();
        }
    }
}
