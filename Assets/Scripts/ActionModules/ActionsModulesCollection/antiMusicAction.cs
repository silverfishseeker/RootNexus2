using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antiMusicAction : IBaseAction {
    
    public AudioClip clip;
    
    protected override void SubRun(){
        GameStateEngine.gse.radio.RemoveTrack(clip);
    }
}
