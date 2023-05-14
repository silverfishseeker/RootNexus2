using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMusicAction : IBaseAction {

    public AudioClip clip;
    public float transTime;
    
    protected override void SubRun(){
        if(transTime < 0){
            Debug.LogError("transTime debe de ser mayor que 0");
            return;
        }
        MusicRun();
    }
    protected abstract void MusicRun();
}
