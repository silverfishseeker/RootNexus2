using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    private Transform reference;
    [Header("Si usas este componente, asegúrate que el objeto está en (0,0)")]
    public float xAxis;
    public float yAxis;
    
    void Start() {
        reference = GameStateEngine.gse.parallaxReferece;
    }

    void Update() {
        transform.position = new Vector3(
            reference.position.x*xAxis,
            reference.position.y*yAxis,
            transform.position.z);
    }
}
