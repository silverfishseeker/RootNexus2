using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausable : MonoBehaviour{
    public void Pause(){
        enabled = false;
    }
    public void Resume(){
        enabled = true;
    }

    protected void OnDestroy(){
        allInstances.Remove(this);
    }

    private static List<Pausable> allInstances = new List<Pausable>();

    protected void Start(){
        allInstances.Add(this);
    }

    public static void PauseAll(){
        foreach(Pausable p in allInstances)
            p.Pause();
    }

    public static void ResumeAll(){
        foreach(Pausable p in allInstances)
            p.Resume();
    }
}