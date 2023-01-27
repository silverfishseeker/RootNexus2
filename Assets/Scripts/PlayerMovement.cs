using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Math.Pow

public class PlayerMovement : MonoBehaviour {
    public float startx, starty;

    // Fuerzas de movimiento
    private float fuerza; // fuerza base de desplazamiento (vertical y horizontal)
    public float fuerzaAndar; // también escalar
    public float fuerzaCarrera;
    public float fuerzaSalto;
    public float fuerzaSaltoImpulsoLateral; //Al saltar se añade un impulso lateral adicional según la dirección
    public float coeficienteSaltoPared;
    public float coeficienteSaltoImpulsoLateralPared; // El salto desde una pared te desprende de ella
    public float rozamientoSuelo; // mientras toque a a wall
    public float rozamientoAire;

    // Costes de energía
    private float costeHorizontal;
    public float costeAndarFps;
    public float costeCorrerFps;
    public float costeSalto;
    public float costeParedFps;
    public float costeBaseExpCaida;
    public float costeCoefCaida;

    // Colliders
    public Collider2D wall;
    public Collider2D groundCollider;
    public Collider2D rightWallCollider;
    private Collider2D leftWallCollider; // copia del right

    // Estados de tocamientos
    public bool isTouchingWall => gameObject.GetComponent<Collider2D>().IsTouching(wall); // si el collider original del objeto está tocando el wall
    public bool onLeftWall => leftWallCollider.IsTouching(wall);
    public bool onRightWall => rightWallCollider.IsTouching(wall);
    public bool onDowntWall => groundCollider.IsTouching(wall);
    private bool wasOnDowntWall;
    public bool isGrabingWall;

    // Accesos a comopones propios
    private Rigidbody2D rb;
    private float gForce; // no hay gravedad agarrado a una pared
    private SpriteRenderer sr;
    private HealthBarController health;

    private float timeTorce { get { return fuerza*Time.deltaTime; } }

    void Start() {
        transform.position = new Vector3(startx, starty, 0);

        leftWallCollider = Instantiate(rightWallCollider, transform);
        leftWallCollider.offset = new Vector2(-leftWallCollider.offset.x, leftWallCollider.offset.y);

        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        gForce = rb.gravityScale;
        // Para que lo siguiente funcione es imprescindible que GameStateEngine se ejecute primero. esto se logra en Script Execution Order
        health  = GameStateEngine.gse.hbc;
        GameStateEngine.gse.avatar = gameObject;

        fuerza = fuerzaAndar;
        costeHorizontal = costeAndarFps;
    }

    // Update is called once per frame
    void Update() {
        if (GameStateEngine.isPaused)
            return;

        rb.drag = isTouchingWall ? rozamientoSuelo : rozamientoAire;

        // Daño de caída
        if (onDowntWall && !wasOnDowntWall) {
            float f = ((float)Math.Pow(costeBaseExpCaida, rb.velocity.y*rb.velocity.y)-1f)*costeCoefCaida;
            health.Add(-f);
        }
        wasOnDowntWall = onDowntWall;

        Vector2 stepForce = new Vector2(0,0);
        
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        // Escalar
        if ((inputHorizontal>0 && onRightWall)  ||  (inputHorizontal<0 && onLeftWall)) {
            isGrabingWall = true;
            // Apagamos la grabedad para poder quedarnos quietos en una pared
            rb.gravityScale = 0;
            stepForce += new Vector2(0, timeTorce*inputVertical);

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

        // Acelerar
        if (Input.GetButton("Run") && isTouchingWall) {
            fuerza = fuerzaCarrera;
            costeHorizontal = costeCorrerFps;
        } else {
            fuerza = fuerzaAndar;
            costeHorizontal = costeAndarFps;
        }
        
        // Saltar
        if (Input.GetButtonDown("Jump") && isTouchingWall) {
            // BUG: si se pulsa la tecla lo suficiente rápido (como en 1 o 2 frames) se puede realizar doble salto, pero es demasiado difícil para un humano
            // esto es culpa de que isTouchingWall permanece activo por demasiado tiempo tras saltar
            if (onDowntWall) {
                stepForce += new Vector2(fuerzaSaltoImpulsoLateral*inputHorizontal, fuerzaSalto);
                health.Add(-costeSalto);
            } else if (isGrabingWall) {
                stepForce += new Vector2(
                    -fuerzaSaltoImpulsoLateral*coeficienteSaltoImpulsoLateralPared*inputHorizontal,
                    fuerzaSalto*coeficienteSaltoPared);
                health.Add(-costeSalto);
            }
        }
        
        // Andar
        if (inputHorizontal != 0) {
            stepForce += new Vector2(timeTorce*inputHorizontal, 0);
            health.AddDelta(-costeHorizontal);
        }

        rb.AddForce(stepForce);

        // BORRAR
        if (Input.GetKey("q")) {
            gameObject.transform.position = new Vector3(startx, starty, 0);
        }
    }
}
