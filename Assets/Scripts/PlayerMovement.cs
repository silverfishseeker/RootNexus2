using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
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

    private float timeTorce { get { return fuerza*Time.deltaTime; } }

    void Start() {
        transform.position = new Vector3(startx, starty, 0);
        radius = gameObject.GetComponent<CircleCollider2D>().radius;
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        gForce = rb.gravityScale;
        // Para que lo siguiente funcione es imprescindible que GameStateEngine se ejecute primero. esto se logra en Script Execution Order
        health  = GameStateEngine.gse.hbc;
        fuerza = fuerzaAndar;
        costeHorizontal = costeAndarFps;
    }
    

    // Update is called once per frame
    void Update() {
        if (GameStateEngine.isPaused)
            return;

        onLeftWall  = Physics2D.OverlapCircle(new Vector2(transform.position.x-radius, transform.position.y-0.8f*radius), 0.05f, groundlayer);
        onRightWall = Physics2D.OverlapCircle(new Vector2(transform.position.x+radius, transform.position.y-0.8f*radius), 0.05f, groundlayer);

        // Daño de caída
        onDowntWall = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y-2f*radius), 0.01f, groundlayer);
        if (onDowntWall && !wasOnDowntWall)
            health.Add(costeCoefCaida*rb.velocity.y);
        wasOnDowntWall = onDowntWall;

        
        Vector2 stepForce = new Vector2(0,0);
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if ((horizontal>0 && onRightWall)  ||  (horizontal<0 && onLeftWall)) {
            isGrabingWall = true;
            // Apagamos la grabedad para poder quedarnos quietos en una pared
            rb.gravityScale = 0;
            stepForce += new Vector2(0, timeTorce*vertical);

            // Fuerza hacia la pared para que no nos despeguemos de ella
            if (onRightWall)
                stepForce += new Vector2( 0.5f * timeTorce, 0);
            if (onLeftWall)
                stepForce += new Vector2(-0.5f * timeTorce, 0);
            
            health.AddDelta(-costeParedFps);

        } else {
            isGrabingWall = false;
            rb.gravityScale = gForce;
        }

        if (Input.GetButton("Jump") & onGround) {
            fuerza = fuerzaCarrera;
            costeHorizontal = costeCorrerFps;
        } else if (Input.GetButtonUp("Jump")){
            fuerza = fuerzaAndar;
            costeHorizontal = costeAndarFps;
            if (onDowntWall || isGrabingWall) {
                float fuerzaY = fuerzaSalto * (isGrabingWall ? coeficienteSaltoPared : 1);

                float fuerzaX = 0;
                fuerzaX += fuerzaSaltoImpulsoLateral*horizontal;

                stepForce += new Vector2(fuerzaX, fuerzaY);
                health.Add(-costeSalto);
            }
        }
        
        if (horizontal != 0) {
            stepForce += new Vector2(timeTorce*horizontal, 0);
            health.AddDelta(-costeHorizontal);
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
