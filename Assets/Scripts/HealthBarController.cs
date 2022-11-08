using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{

    private float inicialWidth;
    public float max;
    public float coeficiente;

    public float curr;

    private RectTransform rt;

    void Start()  {
        rt = gameObject.GetComponent<RectTransform>();
        inicialWidth = rt.localScale.x;
        Reset();
    }

    public void Reset(){
        curr = max;
        rt.localScale = new Vector3(inicialWidth, rt.localScale.y, rt.localScale.z);
    }

    private void Set(float value) {
        if (!GameStateEngine.isPaused) {
            curr = value*coeficiente;
            if (curr <= 0) {
                GameStateEngine.GameOver();
            } else {
                if (curr > max)
                    curr = max;
                float newWidth = curr / max * inicialWidth;
                rt.localScale = new Vector3(newWidth, rt.localScale.y, rt.localScale.z);
            }
        }
    }

    public void Add(float value) {
        Set(curr + value);
    }

    public void AddDelta(float value) {
        Set(curr + value*Time.deltaTime);
    }

    public void Mult(float value) {
        Set(curr * value);
    }

    void Update()
    {
        // Testing bar
        if (Input.GetKeyUp("u")) {
            Set(max);
        }
        if (Input.GetKeyUp("i")) {
            Add(-10f);
        }
        if (Input.GetKeyUp("o")) {
            Add(10f);
        }
        if (Input.GetKeyUp("p")) {
            Mult(0.5f);
        }
    }
}
