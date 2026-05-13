using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerSounds playerSounds;

    [Header("Movimiento")]
    public float speed = 6f;

    [Header("Salto")]
    public float jumpForce = 7f;

    [Header("Referencias")]
    public Transform cameraTransform;
    public Animator animator;

    private Rigidbody rb;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        Jump();
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

        // Sonido correr

 


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
        }
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

