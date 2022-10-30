using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public GameObject gameBase_access;
    private GameStateEngine gse;

    private float inicialWidth;
    public float max;
    public float coeficiente;

    public float curr;

    private RectTransform rt;

    void Start()  {
        gse = gameBase_access.GetComponent<GameStateEngine>();

        rt = gameObject.GetComponent<RectTransform>();
        curr = max;
        inicialWidth = rt.localScale.x;
    }

    private bool UpdateBar(){
        if (curr <= 0) {
            gse.GameOver();
            return false;
        }

        if (curr > max)
            curr = max;
        float newWidth = curr / max * inicialWidth;
        rt.localScale = new Vector3(newWidth, rt.localScale.y, rt.localScale.z);
        return true;
    }

    public bool Set(float value) {
        curr = value;
        return UpdateBar();
    }

    public bool Add(float value) {
        curr += value*coeficiente;
        return UpdateBar();
    }

    public bool Mult(float value) {
        curr *= value*coeficiente;
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
