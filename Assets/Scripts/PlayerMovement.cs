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
    public float costeExpCaida;
    public float costeCoefCaida;

    // Colliders
    public LayerMask maskWall;
    public Collider2D groundCollider;
    public Collider2D rightWallCollider;
    private Collider2D leftWallCollider; // copia del right

    // Estados de tocamientos
    public bool isTouchingWall => GetComponent<Collider2D>().IsTouchingLayers(maskWall); // si el collider original del objeto está tocando el wall
    public bool onLeftWall => leftWallCollider.IsTouchingLayers(maskWall);
    public bool onRightWall => rightWallCollider.IsTouchingLayers(maskWall);
    public bool onDowntWall => groundCollider.IsTouchingLayers(maskWall);
    private bool wasOnDowntWall;
    private Vector2 lastVelocity;
    public bool isGrabingWall;

    // Parámetros animación
    public float animationXSpeedCoef;
    public float animationYSpeedCoef;
    public float animationTresholdBaseExp;
    public float animationTresholdCoef;

    // Accesos a comopones propios
    private Rigidbody2D rb;
    private float gForce; // no hay gravedad agarrado a una pared
    private SpriteRenderer sr;
    private HealthBarController health;
    private Animator animator;

    private float timeTorce { get { return fuerza*Time.deltaTime; } }

    void Start() {
        transform.position = new Vector3(startx, starty, 0);

        leftWallCollider = Instantiate(rightWallCollider, transform);
        leftWallCollider.offset = new Vector2(-leftWallCollider.offset.x, leftWallCollider.offset.y);

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gForce = rb.gravityScale;
        // Para que lo siguiente funcione es imprescindible que GameStateEngine se ejecute primero. esto se logra en Script Execution Order
        health  = GameStateEngine.gse.hbc;
        GameStateEngine.gse.avatar = gameObject;

        fuerza = fuerzaAndar;
        costeHorizontal = costeAndarFps;
    }

    // Se pausa en GameStateEngine
    void Update() {        

        rb.drag = isTouchingWall ? rozamientoSuelo : rozamientoAire;

        // Daño de caída
        if (onDowntWall && !wasOnDowntWall)
            health.Add(-((float)Math.Pow(Math.Abs(lastVelocity.y), costeExpCaida))*costeCoefCaida);
        wasOnDowntWall = onDowntWall;
        lastVelocity = rb.velocity;

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
            sr.flipX = inputHorizontal<0;
            stepForce += new Vector2(timeTorce*inputHorizontal, 0);
            health.AddDelta(-costeHorizontal);
        }

        rb.AddForce(stepForce);

        // Animación

        if (isGrabingWall)
            animator.speed = (float)Math.Abs(rb.velocity.y) * animationYSpeedCoef;
        else if (rb.velocity.x != 0)
            animator.speed = (float)Math.Abs(rb.velocity.x) * animationXSpeedCoef;
        else
            animator.speed = 1;

        animator.SetFloat("xVelocity", 
            (float) (1 - Math.Pow(animationTresholdBaseExp, -Math.Abs(rb.velocity.x*animationTresholdCoef)))
        );
        animator.SetBool("isClimbing", isGrabingWall);
    }
}
