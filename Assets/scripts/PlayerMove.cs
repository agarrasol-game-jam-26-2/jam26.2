using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Camera PCamera;
    private Rigidbody rb;
    private Animator anim;
    private bool isInitialized = false;

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
        
    }
}
