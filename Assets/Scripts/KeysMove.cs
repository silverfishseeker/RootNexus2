using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeysMove : MonoBehaviour
{
    public float startx, starty;
    public float fuerza;
    public float fuerzaSalto;
    public float rozamientoSuelo;
    public float rozamientoAire;

    public LayerMask groundlayer;
    // Representa si está tocando "groundlayer" (en cualquier dirección)
    public bool onGround;
    public bool onLeftWall;
    public bool onRightWall;
    public bool isGrabingWall;

    private float radius;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float gForce;


    void Start() {
        transform.position = new Vector3(startx, starty, 0);
        radius = gameObject.GetComponent<CircleCollider2D>().radius;
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        gForce = rb.gravityScale;        
    }
    
    private float timeTorce {
        get { return fuerza*Time.deltaTime; }
    }

    // Update is called once per frame
    void Update()
    {
        onLeftWall  = Physics2D.OverlapCircle(new Vector2(transform.position.x-radius, transform.position.y-0.8f*radius), 0.05f, groundlayer);
        onRightWall = Physics2D.OverlapCircle(new Vector2(transform.position.x+radius, transform.position.y-0.8f*radius), 0.05f, groundlayer);
        
        Vector2 stepForce = new Vector2(0,0);
        
        if ((Input.GetKey("d")&onRightWall) | (Input.GetKey("a")&onLeftWall)) {
            isGrabingWall = true;
            // Apagamos la grabedad para poder quedarnos quietos en una pared
            rb.gravityScale = 0;
            if (Input.GetKey("w"))
                stepForce += new Vector2(0, timeTorce);
            if (Input.GetKey("s"))
                stepForce += new Vector2(0, -timeTorce);

            // Fuerza hacia la pared para que no nos despeguemos de ella
            if (onRightWall)
                stepForce += new Vector2( 0.5f * timeTorce, 0);
            if (onLeftWall)
                stepForce += new Vector2(-0.5f * timeTorce, 0);

        } else {
            isGrabingWall = false;
            rb.gravityScale = gForce;
        }


        if (Input.GetKeyUp("space") & onGround){
            stepForce += new Vector2(0, fuerzaSalto);
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
