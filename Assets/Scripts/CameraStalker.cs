using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStalker : MonoBehaviour {
    public GameObject player;

    private Transform pt;

    // Start is called before the first frame update
    void Start() {
        pt = player.transform;
    }

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(pt.position.x, pt.position.y, pt.position.y-1);
    }
}
