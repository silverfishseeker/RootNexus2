using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAction : AbstractMusicPropertiesAction {
    
    public AudioClip clip;
    public bool loop; //loop
    
    protected override void MusicRun(){
        GameStateEngine.gse.radio.AddOrChangeTrack(clip,emisora,loop,volume,stereoPan,reverbZoneMix,transTime);
    }
}
