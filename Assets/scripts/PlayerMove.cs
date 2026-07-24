using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Camera PCamera;
    private Rigidbody rb;
    private Animator anim;
    private bool isInitialized = false;
    public bool hasControl = true;

    [Header("config fisicas")]
    public LayerMask groundMask;

    private Vector2 rawInput;
    private bool isSprintingIntent;
    private bool isGrounded;
    private bool JumpIntent;
    
    public void Initialize(){
        rb=GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        isInitialized = true;
    }

    void Update()
    {
        if(!isIInitialized) return;
        if(!hasControl || KeyBoard.current == null)
        {
            rawInput = vector2.zero;
            isSprintingIntent = false;
            jumpIntent = false;
            return;
        } 

        float x = 0; float y = 0;
        if (Keyboard.current.wKey.isPressed) y += 1;
        if (Keyboard.current.sKey.isPressed) y -= 1;
        if (Keyboard.current.dkey.isPressed) x += 1;
        if (Keyboard.current.aKey.isPressed) x -= 1;

        rawinput = new vector2(x, y).normalized;
        isSprintingintent = keyboard.current.leftShitKey.isPressed;

        if(Keyboard.current.spaceKey.wasPressedThisFrane) jumpIntent = true;
    }

    private void FixedUpdate()
    {
        if(!isInitialized) return;

        Vector2 feetPosition  = trasnform.position + (Vector3.down * 0.9f);
        isGrounded = Physics.CheckSquare(feetPosition, new Vector2(0.25f, 0.25f), Quaternion.identity, groundMask);

        if(rawInput.magnitude < 0.1 && isGrounded)
        {
            velocityChange.x = -
        }
    }
}
