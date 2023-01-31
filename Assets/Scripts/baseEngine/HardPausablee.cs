using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardPausable : Pausable {
    new void OnDestroy(){
        base.OnDestroy();
        allHardInstances.Remove(this);
        Debug.Log(name);
    }

    private static List<HardPausable> allHardInstances = new List<HardPausable>();
    
    new protected void Start() {
        base.Start();
        allHardInstances.Add(this);
    }

    new public static void PauseAll(){
        
        foreach(Pausable p in allHardInstances)
            p.Pause();
    }

    new public static void ResumeAll(){
        foreach(Pausable p in allHardInstances)
            p.Resume();
    }
}
