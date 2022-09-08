using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        //gameObject.SetActive(false);
        Destroy(transform.parent.gameObject);
        ScoreManager.scoreManager.raiseScore(1);

    }
}
