using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public GameObject dynamitePrefab; // Префаб динамита
    public Transform throwPoint; // Точка, откуда будет вылетать динамит
    public float throwForce = 10f; // Сила броска

    private Rigidbody rb;
    private bool isGrounded;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Animator animator;

    private bool facingRight = true; // Для отслеживания направления персонажа

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Jump.performed += Jump;
        inputActions.Player.Throw.performed += ThrowDynamite; // Добавляем бросок динамита
    }

    void OnDisable()
    {
        inputActions.Player.Jump.performed -= Jump;
        inputActions.Player.Throw.performed -= ThrowDynamite;
        inputActions.Disable();
    }

    void Update()
    {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        float speedMagnitude = moveInput.magnitude * speed;
        animator.SetFloat("Speed", speedMagnitude);
        animator.SetBool("isGrounded", isGrounded);

        if (moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            facingRight = true;
        }
        else if (moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            facingRight = false;
        }
    }

    void FixedUpdate()
    {
        // Устанавливаем движение персонажа по оси X
        rb.linearVelocity = new Vector3(moveInput.x * speed, rb.linearVelocity.y, 0);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void ThrowDynamite(InputAction.CallbackContext context)
    {
        if (context.performed && dynamitePrefab != null)
        {
            GameObject dynamite = Instantiate(dynamitePrefab, throwPoint.position, Quaternion.identity);
            Rigidbody dynamiteRb = dynamite.GetComponent<Rigidbody>();

            Vector3 throwDirection = facingRight ? Vector3.right : Vector3.left;
            dynamiteRb.AddForce(throwDirection * throwForce + Vector3.up * 2f, ForceMode.Impulse);
        }
    }

    // Метод для применения силы взрыва на игрока
    public void ApplyExplosionForce(Vector3 explosionPosition, float explosionRadius, float explosionForce)
    {
        // Проверяем, находится ли игрок в радиусе взрыва
        float distance = Vector3.Distance(explosionPosition, transform.position);

        if (distance < explosionRadius)
        {
            // Вычисляем направление от взрыва
            Vector3 direction = transform.position - explosionPosition;
            direction.Normalize();

            // Применяем силу от взрыва
            float force = (1 - (distance / explosionRadius)) * explosionForce;
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}
