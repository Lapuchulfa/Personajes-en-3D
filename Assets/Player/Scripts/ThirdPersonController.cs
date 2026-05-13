using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerSounds playerSounds;
    public GameObject runParticles;

    [Header("Movimiento")]
    public float speed = 6f;

    [Header("Particulas")]
    public Transform runParticlesPoint;

    [Header("Salto")]
    public float jumpForce = 7f;

    [Header("Gravedad")]
    public float gravityMultiplier = 2.5f;

    [Header("Referencias")]
    public Transform cameraTransform;
    public Animator animator;

    private Rigidbody rb;

    private bool isGrounded;
    private bool wasMoving;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        Jump();
        ApplyBetterGravity();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Direccion segun camara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Evitar inclinacion
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Movimiento relativo a camara
        Vector3 moveDirection = (forward * v + right * h).normalized;

        // Valor movimiento
        float movementAmount = moveDirection.magnitude;

        // Particulas al correr
        bool isMoving = movementAmount > 0.1f && isGrounded;

        if (isMoving && !wasMoving)
        {
            SpawnRunParticles();
        }

        wasMoving = isMoving;

        // Animaciones
        if (animator != null)
        {
            animator.SetFloat("correr", movementAmount);
            animator.SetBool("saltando", !isGrounded);
        }

        // Movimiento
        rb.MovePosition(
            rb.position + moveDirection * speed * Time.fixedDeltaTime
        );

        // Rotacion personaje
        Vector3 lookDirection = cameraTransform.forward;

        lookDirection.y = 0f;

        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookDirection),
                10f * Time.fixedDeltaTime
            );
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerSounds.AudioSaltar();

            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                jumpForce,
                rb.linearVelocity.z
            );

            isGrounded = false;
        }
    }

    void ApplyBetterGravity()
    {
        // Hace que la caida sea mas rapida y realista
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y *
                                 (gravityMultiplier - 1) * Time.deltaTime;
        }
    }

    void SpawnRunParticles()
    {
        if (runParticles == null || runParticlesPoint == null)
            return;

        Instantiate(
            runParticles,
            runParticlesPoint.position,
            runParticlesPoint.rotation
        );
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}