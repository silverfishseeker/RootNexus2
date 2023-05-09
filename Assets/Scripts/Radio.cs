using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour {

    public void AddTrack(AudioClip clip){
            AudioSource asrc = gameObject.AddComponent<AudioSource>();
            asrc.clip = clip;
            asrc.loop = true;
            asrc.Play();
    }
}
