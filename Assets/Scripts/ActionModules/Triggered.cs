using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggered : Activable {

    public IBaseAction onExitAction;
    private bool hasCollided;
    private ActionSetter exitEvent;
    
    new void Start() {
        exitEvent = gameObject.AddComponent<ActionSetter>();
        exitEvent.first = onExitAction;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Sira" && hasCollided == false){
            hasCollided = true;
            Run();
        }   
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.tag == "Sira" && hasCollided == true){
            hasCollided = false;
            if(isAvalaible && onExitAction != null)
                exitEvent.Run();
        }
            
    }
    

}
