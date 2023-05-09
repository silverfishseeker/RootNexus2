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
            onPlay[clip].coroutine = StartCoroutine(WaitForClipToFinish(asrc));
        }

        if (!isAlready || asrc.volume != volume){
            onPlay[clip].isInTransition = true;
            onPlay[clip].goalVolumen = volume;
            onPlay[clip].secodnsLeft = transTime;
            onPlay[clip].pendiente = transTime == 0 ? 0 : (volume - asrc.volume) / transTime;
        }
    }

    private void SimpleDelete(AudioClip clip){
        Destroy(onPlay[clip].asrc);
        onPlay.Remove(clip);
    }

    public bool RemoveTrack(AudioClip clip, float transTime){
        if (!onPlay.ContainsKey(clip))
            return false;

        if (transTime == 0){
            StopCoroutine(onPlay[clip].coroutine);
            SimpleDelete(clip);
        } else{
            StopCoroutine(onPlay[clip].coroutine);
            onPlay[clip].coroutine = StartCoroutine(WaitForClipToVolumen0(onPlay[clip].asrc));
            onPlay[clip].isInTransition = true;
            onPlay[clip].goalVolumen = 0;
            onPlay[clip].secodnsLeft = transTime;
            onPlay[clip].pendiente = transTime == 0 ? 0 : (0 - onPlay[clip].asrc.volume) / transTime;
        }
        return true;
    }

    // Esto quita el sonido una vez acaba (loop = false)
    private IEnumerator WaitForClipToFinish(AudioSource asrc) {
        yield return new WaitUntil(() => !asrc.isPlaying && !asrc.loop);
        SimpleDelete(asrc.clip);
    }

    private IEnumerator WaitForClipToVolumen0(AudioSource asrc) {
        yield return new WaitUntil(() => asrc.volume == 0);
        SimpleDelete(asrc.clip);
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
