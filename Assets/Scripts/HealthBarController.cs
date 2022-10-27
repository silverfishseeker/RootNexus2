using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{

    private float inicialWidth;
    public float max;

    public float curr;

    private RectTransform rt;

    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        curr = max;
        inicialWidth = rt.localScale.x;
    }

    private bool UpdateBar(){
        if (curr > max)
            curr = max;
        float newWidth = curr / max * inicialWidth;
        rt.localScale = new Vector3(newWidth, rt.localScale.y, rt.localScale.z);
        return curr >= 0;
    }

    public bool Set(float value) {
        curr = value;
        return UpdateBar();
    }

    public bool Add(float value) {
        curr += value;
        return UpdateBar();
    }

    public bool Mult(float value) {
        curr *= value;
        return UpdateBar();
    }

    void Update()
    {
        // Testing bar
        if (Input.GetKeyUp("u")) {
            curr = max;
            UpdateBar();
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
