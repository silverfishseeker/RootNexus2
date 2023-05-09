using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAction : IBaseAction {
    
    public AudioClip clip;
    public bool loop; //loop
    [Range(0f, 1f)]
    public float volume; //volume
    [Range(-1f, 1f)]
    public float stereoPan; //panStereo
    [Range(0f, 1.1f)]
    public float reverbZoneMix; //reverbZoneMix
    public float transTime;
    
    protected override void SubRun(){
        GameStateEngine.gse.radio.AddTrack(clip,loop,volume,stereoPan,reverbZoneMix,transTime);
    }
}
