using UnityEngine;
using static UnityEngine.ParticleSystem;
public class Movimiento : MonoBehaviour
{
    private Rigidbody2D rb;
    public Gradient gradient;
    [SerializeField] private float Acceleration = 5f;
    [SerializeField] private float floatVelocityLowGravityThreshold = 4f;
    [SerializeField] public float GroundFriction = 2f;
    [SerializeField] public float AirFriction = 2f;
    [SerializeField] private float MaxAccelerationGround = 15f;
    [SerializeField] private float MaxAccelerationAir = 15f;
    [SerializeField] private float GroundAcceleration = 10f;
    [SerializeField] private float AirAcceleration = 5f;
    [SerializeField] private float MaxSpeed = 10f;
    [SerializeField] private float JumpForce = 12f;
    [SerializeField] private float SustainedJumpForce = 8f;
    [SerializeField] private float MaxJumpDuration = 0.2f;
    [SerializeField] private int JumpLimit = 1;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private Transform groundCheckOrigin;
    [SerializeField] MovementStats stats;
    [SerializeField] public float peakGravity = 1f;
    [SerializeField] public float fallingGravity = 1f;
    [SerializeField] public float defaultGravity = 1f;


    public ParticleSystem particleSystem;
    public ParticleSystem particleSystem2; // Partículas cuando no quedan más saltos
    public ParticleSystem groundParticles;

    private bool isGrounded;
    private bool onPreviousFloor;
    private int numberJumps;
    private bool wantsJump;
    private bool wantsSustainedJump;
    private bool isJumping;
    private float timeJump;
    private Vector2 inputMovement;
    private float gradValue = 1f;
    private float timeParticles;
    private float targetSpeed;
    private float newVelocityX;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gradient.Evaluate(1);
        particleSystem2.Stop();
        groundParticles.Stop();
        onPreviousFloor = false;
        timeParticles = 0f;
        

    }

    private void Update()
    {


        // Lectura de entrada para movimiento horizontal
        inputMovement = Vector2.zero;
        if (Input.GetKey(KeyCode.D)) inputMovement += Vector2.right;
        if (Input.GetKey(KeyCode.A)) inputMovement += Vector2.left;

        // Manejo del salto
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (numberJumps < stats.JumpLimit)
            {
                wantsJump = true;
                timeJump = 0;
            }
            else
            {
                // Si no quedan más saltos, activar partículas de advertencia
                if (!particleSystem2.isPlaying)
                {
                    particleSystem2.Play();
                }
            }
        }
        else if (Input.GetKey(KeyCode.W) && isJumping)
        {
            wantsSustainedJump = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            wantsSustainedJump = false;
            isJumping = false;
        }

        // Verificar si el personaje está en el suelo
        isGrounded = Physics2D.Raycast(groundCheckOrigin.position, Vector2.down, stats.GroundCheckDistance, groundLayer);

        // Manejo del aterrizaje
        if (isGrounded && !onPreviousFloor)
        {
            HandleLanding();
        }

        // Manejo del temporizador para detener las partículas del suelo
        if (timeParticles > 0)
        {
            timeParticles -= Time.deltaTime;
            if (timeParticles <= 0 && groundParticles.isPlaying)
            {
                groundParticles.Stop();
            }
        }

        // Actualizar el estado anterior del suelo
        onPreviousFloor = isGrounded;
    }
    public void UpdateStat(MovementStats newStat)
    {
        stats = newStat;
    }

    private void FixedUpdate()
    {

        ApplyMovement();
        ApplyJump();
        GravitySummit();
        ApplyJump();
    }

    private void ApplyMovement()
    {
        targetSpeed = inputMovement.x * stats.MaxSpeed;
        newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, stats.Acceleration * Time.fixedDeltaTime);
        // nuevaVelocidadX = rb.linearVelocity.x + (velocidadObjetivo - rb.linearVelocity.x) * stats.aceleracion * Time.fixedDeltaTime;
        // if (nuevaVelocidadX > velocidadMaxima)
        // {
        //     nuevaVelocidadX = velocidadMaxima;
        // }           
        // else if (nuevaVelocidadX < -velocidadMaxima)
        // {
        //    nuevaVelocidadX = -velocidadMaxima;
        // }
        //  rb.linearVelocity = new Vector2(nuevaVelocidadX, rb.linearVelocity.y);

        if (isGrounded)
        {



            if (inputMovement.x == 0)
            {

                newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, 0, stats.GroundFriction * Time.fixedDeltaTime);
            }
            else
            {

                newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, stats.Acceleration * Time.fixedDeltaTime);
            }


            rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);

            newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, stats.GroundFriction * Time.fixedDeltaTime);
            if (stats.GroundAcceleration >= MaxAccelerationGround)
            {

                if ((rb.linearVelocity.x < 0 ? -rb.linearVelocity.x : rb.linearVelocity.x) > MaxAccelerationAir)
                {
                    rb.linearVelocity = new Vector2((rb.linearVelocity.x < 0 ? -1 : 1) * MaxAccelerationAir, rb.linearVelocity.y);
                }

            }
            
        }
        else
        {
            if (inputMovement.x == 0)
            {

                newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, 0, stats.AirFriction * Time.fixedDeltaTime);
            }
            else
            {

                newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, stats.Acceleration * Time.fixedDeltaTime);
            }


            rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
            newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, stats.AirAcceleration * Time.fixedDeltaTime);
            if (stats.AirAcceleration >= MaxAccelerationAir)
            {

                if ((rb.linearVelocity.x < 0 ? -rb.linearVelocity.x : rb.linearVelocity.x) > MaxAccelerationAir)
                {
                    rb.linearVelocity = new Vector2((rb.linearVelocity.x < 0 ? -1 : 1) * MaxAccelerationAir, rb.linearVelocity.y);
                }

            }
        }
        rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
    }

    private void ApplyJump()
    {
        if (wantsJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.JumpForce);
            wantsJump = false;
            isJumping = true;
            numberJumps++;


            gradValue = Mathf.Clamp01(1f - (float)numberJumps / stats.JumpLimit);
            spriteRenderer.color = gradient.Evaluate(gradValue);
        }

        if (isJumping && wantsSustainedJump && timeJump < stats.MaxJumpDuration)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.SustainedJumpForce);
            timeJump += Time.fixedDeltaTime;
        }
    }

    private void HandleLanding()
    {

        particleSystem2.Stop();

        if (!groundParticles.isPlaying)
        {
            groundParticles.Play();
            timeParticles = 1f;
        }

        numberJumps = 0;

        // Reiniciar gradiente
        gradValue = 1f;
        spriteRenderer.color = gradient.Evaluate(gradValue);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckOrigin.position, 0.01f);
        Gizmos.DrawLine(groundCheckOrigin.position, groundCheckOrigin.position + Vector3.down * groundCheckDistance);
    }
    private void GravitySummit()
    {
        if (rb.linearVelocityY >= floatVelocityLowGravityThreshold)
        {
            increasedGravity();


        }
        else if (rb.linearVelocity.y < -floatVelocityLowGravityThreshold)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startColor = Color.blue;
            rb.gravityScale = stats.FallingGravity;

        }
        if (rb.linearVelocityY >= -floatVelocityLowGravityThreshold && rb.linearVelocityY <= floatVelocityLowGravityThreshold)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startColor = Color.green;
            rb.gravityScale = stats.PeakGravity;

        }
    }
    private void increasedGravity()
    {
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startColor = Color.yellow;
        rb.gravityScale = stats.DefaultGravity;
    }

}