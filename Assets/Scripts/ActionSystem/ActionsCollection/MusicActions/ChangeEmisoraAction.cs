using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmisoraAction : AbstractMusicPropertiesAction {
    
    protected override void MusicRun(){
        GameStateEngine.gse.radio.ChangeEmisora(emisora,volume,stereoPan,reverbZoneMix,transTime);
    }
}
