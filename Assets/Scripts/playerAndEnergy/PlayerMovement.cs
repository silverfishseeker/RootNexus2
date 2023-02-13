using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Math.Pow

public class PlayerMovement : MonoBehaviour {

    // Fuerzas de movimiento
    private float generalForce=1;//afecta a todas las demás
    private float fuerza; // fuerza base de desplazamiento (vertical y horizontal)
    public float fuerzaAndar; // también escalar
    public float fuerzaCarrera;
    public float coeficienteEscalar;
    public float fuerzaSalto;
    public float fuerzaSaltoImpulsoLateral; //Al saltar se añade un impulso lateral adicional según la dirección
    public float coeficienteSaltoPared;
    public float coeficienteSaltoImpulsoLateralPared; // El salto desde una pared te desprende de ella

    public float rozamientoSuelo; // mientras toque a a wall
    public float rozamientoAire;
    
    // Propiedades de físicas
    private float jumpBuffer = 1;
    public float jumpBufferMax; //0+, inf
    public float jumpBufferCoef; //1+, inf
    public float redSpeedCoef; // 0+, 1
    private bool isntCansado;//
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
        Descansar();
    }

    public void Cansar(){
        isntCansado = false;
        animator.SetBool("isRed", true);
        generalForce *= redSpeedCoef;
    }

    public void Descansar(){
        isntCansado = true;
        animator.SetBool("isRed", false);
        generalForce = 1;
    }

    // Se pausa en GameStateEngine
    void Update() {        

        rb.drag = isTouchingWall ? rozamientoSuelo : rozamientoAire;

        // Daño de caída
        if (onDowntWall && !wasOnDowntWall){
            float fallDamage = ((float)Math.Pow(Math.Abs(lastVelocity.y), costeExpCaida))*costeCoefCaida;
            health.Add(-fallDamage);
        }
        wasOnDowntWall = onDowntWall;
        lastVelocity = rb.velocity;


        Vector2 stepForce = new Vector2(0,0);
        
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        // Escalar
        if (((inputHorizontal>0 && onRightWall)  ||  (inputHorizontal<0 && onLeftWall))  && isntCansado) {
            isGrabingWall = true;
            // Apagamos la grabedad para poder quedarnos quietos en una pared
            rb.gravityScale = 0;
            stepForce += new Vector2(0, timeTorce*inputVertical*coeficienteEscalar);

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
        if(isTouchingWall){
            if (Input.GetButton("Jump")){
                if (jumpBuffer < jumpBufferMax)
                    jumpBuffer *= jumpBufferCoef;
                inputHorizontal = 0; // Para que esto tenga sentido andar debe de calcularse luego

            }else if (Input.GetButtonUp("Jump") && isntCansado){
                if (onDowntWall) {
                    stepForce += new Vector2(fuerzaSaltoImpulsoLateral, fuerzaSalto*jumpBuffer);
                    health.Add(-costeSalto);
                } else if (isGrabingWall) {
                    stepForce += new Vector2(-fuerzaSaltoImpulsoLateral*coeficienteSaltoImpulsoLateralPared*inputHorizontal, fuerzaSalto*coeficienteSaltoPared);
                    health.Add(-costeSalto);
                }
                jumpBuffer = 1;
            }

        }
        
        // Andar
        if (inputHorizontal != 0) {
            sr.flipX = inputHorizontal<0;
            stepForce += new Vector2(timeTorce*inputHorizontal, 0);
            health.AddDelta(-costeHorizontal);
        }

        rb.AddForce(stepForce*generalForce);

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