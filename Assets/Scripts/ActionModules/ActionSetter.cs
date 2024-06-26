using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSetter : MonoBehaviour {

    public IBaseAction first;
    
    private IBaseAction current;

    protected void Start(){
        current = null;
    }

    public void Run(){
        GameStateEngine.Pause();
        current = first;
        current.Run();
    }

    protected void Update(){
        // con el bucle while podemos correr todas las acciones posibles en el mismo frame, si son demasiadas se puede cambiar con un if
        while(current != null && current.isFinished) {
            current = current.next;
            if (current == null){
                GameStateEngine.Resume();
            } else
                current.Run();
        }
    }
}
