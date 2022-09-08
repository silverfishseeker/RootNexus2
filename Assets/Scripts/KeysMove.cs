using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeysMove : MonoBehaviour
{
    public float startx, starty;
    public float fuerza;
    public float fuerzaSalto;
    //public float rozamientoEscalada;
    public float rozamientoSuelo;
    public float rozamientoAire;

    private Rigidbody2D rb;
    public LayerMask groundlayer;
    // Representa si está tocando "groundlayer" (en cualquier dirección)
    public bool onGround;
    public bool onLeftWall;
    public bool onRightWall;
    public bool isGrabingWall;

    private float radius;
    private float gForce;


    void Start() {
        transform.position = new Vector3(startx, starty, 0);
        radius = gameObject.GetComponent<CircleCollider2D>().radius;

        rb = gameObject.GetComponent<Rigidbody2D>();
        gForce = rb.gravityScale;        
    }
    
    private float timeTorce {
        get { return fuerza*Time.deltaTime; }
    }

    // Update is called once per frame
    void Update()
    {
        rb.gravityScale = gForce;

        onLeftWall  = Physics2D.OverlapCircle(new Vector2(transform.position.x-radius, transform.position.y), 0.05f, groundlayer);
        onRightWall = Physics2D.OverlapCircle(new Vector2(transform.position.x+radius, transform.position.y), 0.05f, groundlayer);
        
        Vector2 stepForce = new Vector2(0,0);
        
        if (Input.GetKey("space") & (onRightWall | onLeftWall)) {
            isGrabingWall = true;
            rb.gravityScale = 0;
            //rb.drag = rozamientoEscalada;
            if (Input.GetKey("w"))
                stepForce += new Vector2(0, timeTorce);
            if (Input.GetKey("s"))
                stepForce += new Vector2(0, -timeTorce);
        } else if (Input.GetKeyUp("space")){
            isGrabingWall = false;
            if (onGround) {
                stepForce += new Vector2(0, fuerzaSalto);
            }
        }

        if (Input.GetKey("a")) {
            stepForce += new Vector2(-timeTorce, 0);
        }
        if (Input.GetKey("d")) {
            stepForce += new Vector2(timeTorce, 0);
        }

        rb.AddForce(stepForce);

        if (Input.GetKey("q")) {
            gameObject.transform.position = new Vector3(startx, starty, 0);
        }
    }
    
    bool IsInLayerMask(LayerMask mask, int layer) {
        return mask == (mask | (1 << layer));
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (IsInLayerMask(groundlayer, other.gameObject.layer)) {
            rb.drag = rozamientoSuelo;
            onGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (IsInLayerMask(groundlayer, other.gameObject.layer)) {
            rb.drag = rozamientoAire;
            onGround = false;
        }
    }
}
