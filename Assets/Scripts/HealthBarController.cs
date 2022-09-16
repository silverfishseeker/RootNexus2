using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{

    public float inicialWidth;
    public float max;
    public float current;

    private RectTransform rt;

    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        current = max;
    }

    private bool UpdateBar(){
        if (current > max)
            current = max;
        float newWidth = current / max * inicialWidth;
        rt.sizeDelta = new Vector2 (newWidth, rt.sizeDelta.y);
        return current >= 0;
    }

    /// <summary>
    /// Adds value to current
    /// </summary>
    /// <returns> true if result above 0, false otherwise </returns>
    public bool Add(float value) {
        current += value;
        return UpdateBar();
    }


    /// <summary>
    /// Multiplies current by value
    /// </summary>
    /// <returns> true if result above 0, false otherwise </returns>
    public bool Mult(float value) {
        current *= value;
        return UpdateBar();
    }

    void Update()
    {
        // Testing bar
        if (Input.GetKeyUp("u")) {
            current = max;
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
