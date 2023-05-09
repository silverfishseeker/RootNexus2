using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAction : AbstractMusicAction {
    
    public bool loop; //loop
    [Range(0f, 1f)]
    public float volume; //volume
    [Range(-1f, 1f)]
    public float stereoPan; //panStereo
    [Range(0f, 1.1f)]
    public float reverbZoneMix; //reverbZoneMix
    
    protected override void MusicRun(){
        GameStateEngine.gse.radio.AddOrChangeTrack(clip,loop,volume,stereoPan,reverbZoneMix,transTime);
    }
}
