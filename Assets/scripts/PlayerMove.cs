using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Referências")]
    private Rigidbody2D rb;
    private Animator anim;
    private bool isInitialized = false;
    public bool hasControl = true;

    [Header("Movimento")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float acceleration = 20f;
    public float deceleration = 15f;

    [Header("Colisão")]
    public LayerMask collisionMask;
    public float collisionCheckDistance = 0.5f;

    private Vector2 rawInput;
    private bool isSprintingIntent;
    private Vector2 currentVelocity;

    void Start()
    {
        Initialize();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        if (rb == null || anim == null)
        {
            Debug.LogError("PlayerMove: Rigidbody2D ou Animator não encontrados!");
            return;
        }

        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 0f;

        Vector3 pos = transform.position;
        pos.z = -1f;
        transform.position = pos;

        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized) return;

        if (!hasControl || Keyboard.current == null)
        {
            rawInput = Vector2.zero;
            isSprintingIntent = false;
            return;
        }

        float x = 0f;
        float y = 0f;

        if (Keyboard.current.wKey.isPressed) y += 1f;
        if (Keyboard.current.sKey.isPressed) y -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.aKey.isPressed) x -= 1f;

        rawInput = new Vector2(x, y).normalized;
        isSprintingIntent = Keyboard.current.leftShiftKey.isPressed;
    }

    private void FixedUpdate()
    {
        if (!isInitialized) return;
        HandleMovement();
    }

    private void HandleMovement()
    {
        float targetSpeed = isSprintingIntent ? sprintSpeed : moveSpeed;
        Vector2 moveDirection = rawInput;

        if (rawInput.magnitude > 0.1f)
        {
            Vector2 targetVelocity = moveDirection * targetSpeed;

            if (!IsCollidingInDirection(moveDirection))
            {
                currentVelocity = Vector2.Lerp(
                    rb.linearVelocity,
                    targetVelocity,
                    acceleration * Time.fixedDeltaTime
                );
            }
            else
            {
                currentVelocity = Vector2.Lerp(
                    rb.linearVelocity,
                    Vector2.zero,
                    deceleration * Time.fixedDeltaTime
                );
            }
        }
        else
        {
            currentVelocity = Vector2.Lerp(
                rb.linearVelocity,
                Vector2.zero,
                deceleration * Time.fixedDeltaTime
            );
        }

        rb.linearVelocity = currentVelocity;

        if (anim != null)
        {
            anim.SetFloat("Speed", rawInput.magnitude);
            anim.SetBool("IsSprinting", isSprintingIntent && rawInput.magnitude > 0.1f);
        }
    }

    private bool IsCollidingInDirection(Vector2 direction)
    {
        if (direction.magnitude < 0.1f) return false;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction.normalized,
            collisionCheckDistance,
            collisionMask
        );

        return hit.collider != null;
    }
}
