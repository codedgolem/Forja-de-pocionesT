using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float rayDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    public Animator animacion;

    private bool isGrounded;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer);
        isGrounded = hit.collider != null;
    }

    private void Update()
    {
        animacion.SetFloat("Movimiento", moveInput.x * speed);

        
        if (moveInput.x != 0)
        {
            Vector2 escala = transform.localScale;

            if (moveInput.x > 0) // derecha
                escala.x = -Mathf.Abs(escala.x);
            else // izquierda
                escala.x = Mathf.Abs(escala.x);

            transform.localScale = escala;
        }
        animacion.SetBool("ensuelo", isGrounded);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        
    }
}