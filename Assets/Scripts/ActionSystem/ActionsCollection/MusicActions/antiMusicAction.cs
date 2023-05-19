using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antiMusicAction : AbstractMusicAction {
    
    public AudioClip clip;
    
    protected override void MusicRun(){
        GameStateEngine.gse.radio.RemoveTrack(clip, transTime);
    }
}
