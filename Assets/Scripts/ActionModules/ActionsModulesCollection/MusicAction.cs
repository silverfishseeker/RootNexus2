using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAction : IBaseAction {
    
    public AudioClip clip;
    
    protected override void SubRun(){
        GameStateEngine.gse.radio.AddTrack(clip);
    }
}
