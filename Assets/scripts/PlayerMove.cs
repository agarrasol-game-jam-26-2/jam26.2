using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Referências")]
    private Rigidbody rb;
    private Animator anim;
    private bool isInitialized = false;
    public bool hasControl = true;

    [Header("Movimento")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float acceleration = 20f;
    public float deceleration = 15f;

    private Vector2 rawInput;
    private bool isSprintingIntent;
    private Vector3 currentVelocity;

    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        if (rb == null || anim == null)
        {
            Debug.LogError("PlayerMove: Rigidbody ou Animator não encontrados!");
            return;
        }

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

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
        Vector3 moveDirection = new Vector3(rawInput.x, 0f, rawInput.y);

        if (rawInput.magnitude > 0.1f)
        {
            Vector3 targetVelocity = moveDirection * targetSpeed;
            currentVelocity = Vector3.Lerp(
                new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z),
                targetVelocity,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            currentVelocity = Vector3.Lerp(
                new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z),
                Vector3.zero,
                deceleration * Time.fixedDeltaTime
            );
        }

        rb.linearVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

        if (anim != null)
        {
            anim.SetFloat("Speed", rawInput.magnitude);
            anim.SetBool("IsSprinting", isSprintingIntent && rawInput.magnitude > 0.1f);
        }
    }
}
