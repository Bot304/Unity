using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;

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
    }

    void OnDisable()
    {
        inputActions.Player.Jump.performed -= Jump;
        inputActions.Disable();
    }

    void Update()
    {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        // Рассчитываем скорость движения
        float speedMagnitude = moveInput.magnitude * speed;

        // Передаём значение скорости в Animator
        animator.SetFloat("Speed", speedMagnitude);
        animator.SetBool("isGrounded", isGrounded);

        // Поворот персонажа в зависимости от направления движения
        if (moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0); // Поворачиваем вправо
        }
        else if (moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0); // Поворачиваем влево
        }

        Debug.Log("isGrounded: " + isGrounded);
    }


    void FixedUpdate()
    {
        // Применяем движение к Rigidbody
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

    // Метод для зеркалирования персонажа
    private void Flip()
    {
        facingRight = !facingRight; // Меняем направление

        // Меняем масштаб по оси X
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Делаем масштаб по X отрицательным/положительным
        transform.localScale = localScale;
    }
}
