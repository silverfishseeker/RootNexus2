using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMusicPropertiesAction : AbstractMusicAction {
    [Tooltip("La emisora 0 puede tener cualquier número de clips, el resto sólo uno.")]
    public int emisora;
    [Range(0f, 1f)]
    public float volume; //volume
    [Range(-1f, 1f)]
    public float stereoPan; //panStereo
    [Range(0f, 1.1f)]
    public float reverbZoneMix; //reverbZoneMix
}
