using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject healthBar;

    public float startx, starty;
    private float fuerza;
    public float fuerzaAndar;
    public float fuerzaSalto;
    public float coeficienteSaltoPared;
    //Al saltar se añade un impulso lateral adicional según la dirección
    public float fuerzaSaltoImpulsoLateral;
    public float fuerzaCarrera;
    public float rozamientoSuelo;
    public float rozamientoAire;

    private float costeHorizontal;
    public float costeAndarFps;
    public float costeCorrerFps;
    public float costeSalto;
    public float costeParedFps;
    public float costeCoefCaida;

    public LayerMask groundlayer;
    // Representa si está tocando "groundlayer" (en cualquier dirección)
    public bool onGround;
    public bool onLeftWall;
    public bool onRightWall;
    public bool onDowntWall;
    private bool wasOnDowntWall;
    public bool isGrabingWall;

    private float radius;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float gForce;
    private HealthBarController health;


    void Start() {
        transform.position = new Vector3(startx, starty, 0);
        radius = gameObject.GetComponent<CircleCollider2D>().radius;
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        gForce = rb.gravityScale;
        health  = healthBar.GetComponent<HealthBarController>();
        fuerza = fuerzaAndar;
        costeHorizontal = costeAndarFps;
    }
    
    private float timeTorce {
        get { return fuerza*Time.deltaTime; }
    }

    // Update is called once per frame
    void Update()
    {
        onLeftWall  = Physics2D.OverlapCircle(new Vector2(transform.position.x-radius, transform.position.y-0.8f*radius), 0.05f, groundlayer);
        onRightWall = Physics2D.OverlapCircle(new Vector2(transform.position.x+radius, transform.position.y-0.8f*radius), 0.05f, groundlayer);

        // Daño de caída
        onDowntWall = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y-2f*radius), 0.01f, groundlayer);
        if (onDowntWall && !wasOnDowntWall)
                health.Add(costeCoefCaida*rb.velocity.y);
        wasOnDowntWall = onDowntWall;

        
        Vector2 stepForce = new Vector2(0,0);
        
        if ((Input.GetKey("d")&&onRightWall) || (Input.GetKey("a")&&onLeftWall)) {
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
            
            health.Add(-costeParedFps);

        } else {
            isGrabingWall = false;
            rb.gravityScale = gForce;
        }

        if (Input.GetKey("space") & onGround) {
            fuerza = fuerzaCarrera;
            costeHorizontal = costeCorrerFps;
        } else if (Input.GetKeyUp("space")){
            fuerza = fuerzaAndar;
            costeHorizontal = costeAndarFps;
            if (onDowntWall || isGrabingWall) {
                float fuerzaY = fuerzaSalto * (isGrabingWall ? coeficienteSaltoPared : 1);

                float fuerzaX = 0;
                if (Input.GetKey("d"))
                    fuerzaX += fuerzaSaltoImpulsoLateral;
                if (Input.GetKey("a"))
                    fuerzaX += -fuerzaSaltoImpulsoLateral;

                stepForce += new Vector2(fuerzaX, fuerzaY);
                health.Add(-costeSalto);
            }
        }

        if (Input.GetKey("a")) {
            stepForce += new Vector2(-timeTorce, 0);
            health.Add(-costeHorizontal);
        }
        if (Input.GetKey("d")) {
            stepForce += new Vector2(timeTorce, 0);
            health.Add(-costeHorizontal);
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
