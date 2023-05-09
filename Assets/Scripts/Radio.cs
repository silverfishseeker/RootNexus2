using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour {

    private class PlayingAudio{
        public AudioSource asrc;
        public Coroutine coroutine;
        public bool isInTransition;
        public float goalVolumen;
        public float secodnsLeft;
        public float pendiente;
    }
    
    private Dictionary<AudioClip, PlayingAudio> onPlay = new Dictionary<AudioClip, PlayingAudio>();

    private void PrepareTransicion(AudioClip clip, float volume, float transTime){
        onPlay[clip].isInTransition = true;
        onPlay[clip].goalVolumen = volume;
        onPlay[clip].secodnsLeft = transTime;
        onPlay[clip].pendiente = transTime == 0 ? 0 : (volume - onPlay[clip].asrc.volume) / transTime;
    }

    private void SimpleDelete(AudioClip clip){
        Destroy(onPlay[clip].asrc);
        onPlay.Remove(clip);
    }
    
    private IEnumerator WaitForClipCond(AudioClip clip, System.Func<bool> WaitFunction) {
        yield return new WaitUntil(WaitFunction);
        SimpleDelete(clip);
    }

    public void AddTrack(AudioClip clip, bool loop, float volume,
            float stereoPan, float reverbZoneMix, float transTime){

        bool isAlready = onPlay.ContainsKey(clip);
        AudioSource asrc = isAlready ? onPlay[clip].asrc : gameObject.AddComponent<AudioSource>();

        asrc.clip = clip;
        asrc.loop = loop;
        asrc.panStereo = stereoPan;
        asrc.reverbZoneMix = reverbZoneMix;

        if (!isAlready){
            asrc.volume = 0;
            asrc.Play();
            onPlay[clip] = new PlayingAudio();
            onPlay[clip].asrc = asrc;
            onPlay[clip].coroutine = StartCoroutine(WaitForClipCond(clip,
                () => !asrc.isPlaying && !asrc.loop
            ));
        }

        if (!isAlready || asrc.volume != volume)
            PrepareTransicion(clip, volume, transTime);
    }

    public bool RemoveTrack(AudioClip clip, float transTime){
        if (!onPlay.ContainsKey(clip))
            return false;

        StopCoroutine(onPlay[clip].coroutine);
        if (transTime == 0){
            SimpleDelete(clip);
        } else{
            onPlay[clip].coroutine = StartCoroutine(WaitForClipCond(clip,
                () => onPlay[clip].asrc.volume == 0
            ));
            PrepareTransicion(clip, 0, transTime);
        }
        return true;
    }

    void Update(){
        float currT = Time.deltaTime;
        foreach (KeyValuePair<AudioClip, PlayingAudio> kvp in onPlay) {
            PlayingAudio pa = kvp.Value;
            if (pa.isInTransition){
                pa.asrc.volume = pa.asrc.volume + currT*pa.pendiente;
                pa.secodnsLeft -= currT;
                if (pa.secodnsLeft <= 0){
                    pa.asrc.volume = pa.goalVolumen;
                    pa.isInTransition = false;
                }
            }
        }
    }
    
}
