using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour {

    private Dictionary<AudioClip, AudioSource> onPlay;

    void Start(){
        onPlay = new Dictionary<AudioClip, AudioSource>();
    }

    public void AddTrack(AudioClip clip){
            AudioSource asrc = gameObject.AddComponent<AudioSource>();
            asrc.clip = clip;
            asrc.loop = true;
            asrc.Play();
            onPlay[clip] = asrc;
    }

    public bool RemoveTrack(AudioClip clip){
        if (!onPlay.ContainsKey(clip))
            return false;
        Destroy(onPlay[clip]);
        onPlay.Remove(clip);
        return true;
    }
}
