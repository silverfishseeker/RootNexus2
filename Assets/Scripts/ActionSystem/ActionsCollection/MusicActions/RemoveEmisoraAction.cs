using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveEmisoraAction : AbstractMusicAction {
    
    public int emisora;

    protected override void MusicRun(){
       GameStateEngine.gse.radio.RemoveEmisora(emisora, transTime);
    }
}
