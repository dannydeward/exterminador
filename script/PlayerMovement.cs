using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movementInput;

    [Header("Punto de Inicio (Spawn)")]
    public Transform spawnPoint;

    [Header("Punto de Ataque Automático")]
    public Transform firePoint;
    public float distanciaAlFrente = 1.0f;

    // 👇 NUEVO: Hacemos pública la última dirección para que GasAttack la lea
    public Vector2 UltimaDireccion { get; private set; } = Vector2.down;

    [Header("Animación")]
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (spawnPoint != null)
        {
            rb.position = spawnPoint.position;
        }
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            float moveX = 0f;
            float moveY = 0f;
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) moveY = 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) moveY = -1f;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) moveX = -1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) moveX = 1f;

            movementInput = new Vector2(moveX, moveY);

            // 👇 Solo actualizamos la dirección si hay movimiento
            if (movementInput != Vector2.zero)
            {
                UltimaDireccion = movementInput.normalized;
            }

            if (firePoint != null)
            {
                firePoint.localPosition = UltimaDireccion * distanciaAlFrente;
            }

            if (animator != null)
            {
                animator.SetFloat("moveX", moveX);
                animator.SetFloat("moveY", moveY);
                animator.SetBool("IsMoving", movementInput != Vector2.zero);
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + movementInput.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }
}