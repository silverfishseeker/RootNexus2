using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Math.Pow

public class PlayerMovement : MonoBehaviour {

    // Fuerzas de movimiento
    private float generalForce=1;//afecta a todas las demás
    [Header("Fuerzas de movimiento")]
    public float fuerza; // fuerza base de desplazamiento (vertical y horizontal)
    public float coeficienteAgacharse;
    private bool isAgachado;
    public float coeficienteEscalar;
    public float coeficienteAire;
    public float fuerzaSalto;
    public float fuerzaSaltoImpulsoLateral; //Al saltar se añade un impulso lateral adicional según la dirección
    public float coeficienteSaltoPared;
    public float coeficienteSaltoImpulsoLateralPared; // El salto desde una pared te desprende de ella
    public float rozamientoSuelo; // mientras toque a a wall
    public float rozamientoAire;

    //-----------------------
    [Header("Costes de energía")]
    public float costeAndarFps;
    public float costeSalto;
    public float costeParedFps;
    public float costeExpCaida;
    public float costeCoefCaida;

    //-----------------------
    [Header("Colliders")]
    public LayerMask maskWall;
    public Collider2D groundCollider;
    public Collider2D rightWallCollider;
    private Collider2D leftWallCollider; // copia del right

    // Estados de tocamientos
    public bool onLeftWall => leftWallCollider.IsTouchingLayers(maskWall);
    public bool onRightWall => rightWallCollider.IsTouchingLayers(maskWall);
    public bool onDowntWall => groundCollider.IsTouchingLayers(maskWall);
    public bool isTouchingWall => onLeftWall || onRightWall || onDowntWall;//GetComponent<Collider2D>().IsTouchingLayers(maskWall); // si el collider original del objeto está tocando el wall
    private bool wasOnDowntWall;
    private Vector2 lastVelocity;
    public bool isGrabingWall;

    //-----------------------
    [Header("Parámetros animación")]
    public float animationXSpeedCoef;
    public float animationYSpeedCoef;
    public float animationTresholdBaseExp;
    public float animationTresholdCoef;
    
    //-----------------------
    [Header("Propiedades de físicas")]
    public float jumpBufferMax; //0+, inf
    public float jumpBufferCoef; //1+, inf
    public float jumpBuffer => fakeJumpBuffer-1;
    public float jumpBufferRalentizacion;
    [HideInInspector]
    public float fakeJumpBuffer =1;
    private bool isJumpBuffer;
    public float redSpeedCoef; // 0+, 1
    private bool isntCansado;//
    [Tooltip("La ralentizacion por caida depende del daño de caída")]
    public bool isRalentizacionCaida;//
    public float coeficienteRalentizacionCaida; // 0+, inf
    public float persistenciaRalentizacionCaida; // 0+, 1
    private float currentCaidaRalentizacion = 1;


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
        rb.drag = onDowntWall ? rozamientoSuelo : rozamientoAire;

        // ralentizacion por caída
        // se multiplica por todo al final
        if(currentCaidaRalentizacion > 0.999)
            currentCaidaRalentizacion = 1;
        else if(currentCaidaRalentizacion < 1)
            currentCaidaRalentizacion += (1-currentCaidaRalentizacion) * (1-persistenciaRalentizacionCaida) * Time.deltaTime;

        // Daño de caída
        if (onDowntWall && !wasOnDowntWall){
            float fallDamage = ((float)Math.Pow(Math.Abs(lastVelocity.y), costeExpCaida))*costeCoefCaida;
            health.Add(-fallDamage);
            // función inversa que pasa por (0,1)
            currentCaidaRalentizacion = coeficienteRalentizacionCaida / (fallDamage+coeficienteRalentizacionCaida);
        }
        wasOnDowntWall = onDowntWall;
        lastVelocity = rb.velocity;


        Vector2 stepForce = new Vector2(0,0);
        
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        // Escalar
        if (isntCansado  && ((inputHorizontal>0 && onRightWall)  ||  (inputHorizontal<0 && onLeftWall))) {
            isGrabingWall = true;
            rb.drag = rozamientoSuelo;
            // Apagamos la grabedad para poder quedarnos quietos en una pared
            rb.gravityScale = 0;
            stepForce += new Vector2(0, timeTorce*inputVertical*coeficienteEscalar);

            // Fuerza hacia la pared para que no nos despeguemos de ella
            // if (onRightWall)
            //     stepForce += new Vector2( 0.0f * timeTorce, 0);
            // if (onLeftWall)
            //     stepForce += new Vector2(-0.5f * timeTorce, 0);
            
            health.AddDelta(-costeParedFps);

        } else {
            isGrabingWall = false;
            rb.gravityScale = gForce;
        }

        // Agacharse
        // bool wasAgachado = isAgachado;
        // isAgachado = Input.GetButton("Run") && onDowntWall;
        // if(wasAgachado != isAgachado) {
        //     if(isAgachado){
        //         transform.localScale = new Vector3(1f, 0.5f, 1f);
        //         transform.position += new Vector3(0f, -0.5f, 0f);
        //     } else {
        //         transform.localScale = new Vector3(1f, 1f, 1f);
        //         transform.position += new Vector3(0f, 0f, 0f);
        //     }
        // }


        // Saltar
        if(isTouchingWall){
            if (Input.GetButton("Jump")){
                if (jumpBuffer < jumpBufferMax)
                    fakeJumpBuffer *= jumpBufferCoef;
                isJumpBuffer = true;

            }else {
                isJumpBuffer = false;
                if (Input.GetButtonUp("Jump") && isntCansado){
                    if (onDowntWall || isGrabingWall){
                        float xJumpForce = fuerzaSaltoImpulsoLateral*inputHorizontal;
                        float yJumpForce = fuerzaSalto;
                        if(isGrabingWall){
                            xJumpForce*=-coeficienteSaltoImpulsoLateralPared; // negativo para despegarse de la pared
                            yJumpForce*=coeficienteSaltoPared;
                        }
                        stepForce += new Vector2(xJumpForce, yJumpForce)*jumpBuffer;
                        health.Add(-costeSalto);
                    }
                    fakeJumpBuffer = 1;
                }
            }

        }
        
        // Andar
        if (inputHorizontal != 0) {
            sr.flipX = inputHorizontal<0;
            stepForce += new Vector2(timeTorce*inputHorizontal
                    *(onDowntWall?1:coeficienteAire)
                , 0);

            health.AddDelta(-costeAndarFps);
        }

        rb.AddForce(stepForce * generalForce 
            * (isRalentizacionCaida ? currentCaidaRalentizacion : 1)
            * (isAgachado ? coeficienteAgacharse : 1)
            * (isJumpBuffer ? jumpBufferRalentizacion : 1) // ralentizamos al men si tiene Jump pulsado
        );

        // Animación
        // if (isGrabingWall)
        //     animator.speed = (float)Math.Abs(rb.velocity.y) * animationYSpeedCoef;
        // else if (rb.velocity.x != 0)
        //     animator.speed = (float)Math.Abs(rb.velocity.x) * animationXSpeedCoef;
        // else
        //     animator.speed = 1;

        animator.SetFloat("xVelocity", 
            (float) (1 - Math.Pow(animationTresholdBaseExp, -Math.Abs(rb.velocity.x*animationTresholdCoef)))
        );
        animator.SetBool("isClimbing", isGrabingWall);
    }
}
