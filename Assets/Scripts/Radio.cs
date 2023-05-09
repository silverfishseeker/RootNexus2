using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour {

    private Dictionary<AudioClip, AudioSource> onPlay = new Dictionary<AudioClip, AudioSource>();
    private Dictionary<AudioClip, Coroutine> coroutines = new Dictionary<AudioClip, Coroutine>();

    public void AddTrack(AudioClip clip, bool loop, float volume,
            float stereoPan, float reverbZoneMix, float transTime){

        bool isAlready = onPlay.ContainsKey(clip);
        AudioSource asrc = isAlready ? onPlay[clip] : gameObject.AddComponent<AudioSource>();

        asrc.clip = clip;
        asrc.loop = loop;
        asrc.volume = volume;
        asrc.panStereo = stereoPan;
        asrc.reverbZoneMix = reverbZoneMix;
        
        if (!isAlready){
            asrc.Play();
            coroutines[clip] = StartCoroutine(WaitForClipToFinish(asrc));
            onPlay[clip] = asrc;
        }
    }

    public bool RemoveTrack(AudioClip clip){
        if (!onPlay.ContainsKey(clip))
            return false;
        Destroy(onPlay[clip]);
        onPlay.Remove(clip);
        StopCoroutine(coroutines[clip]);
        coroutines.Remove(clip);
        return true;
    }

    // Esto quita el sonido una vez acaba
    private IEnumerator WaitForClipToFinish(AudioSource asrc) {
        yield return new WaitUntil(() => !asrc.isPlaying && !asrc.loop);
        Debug.Log("terminated");
        Destroy(asrc);
        onPlay.Remove(asrc.clip);
    }
}
