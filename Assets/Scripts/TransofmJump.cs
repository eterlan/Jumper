
using UnityEngine;

[RequireComponent(typeof(GroundCheckWithSnapping))]
public class TransformJump : MonoBehaviour
{
    public GroundCheckWithSnapping groundCheck;
    public float                   jumpForceY      =20;
    public float                   jumpForceX      =20;
    public float                   gravity         = -9.81f;
    public float                   gravityScale    = 5;
    public float                   xSpeed          = 5;
    public float                   xSpeedLerpSpeed = 0.2f;
    
    [SerializeField]
    private float yVelocity;

    [SerializeField]
    private float xRuntimeSpeed;
    
    private float playerHeight;
    private int   jumpCount;

    private Animator animator;
    private int      animOnGroundHash;
    private int      animJumpHash;
    
    void Update()
    {
        yVelocity += gravity * gravityScale * Time.deltaTime;
        animator.SetBool(animOnGroundHash, groundCheck.isGrounded);
        if (groundCheck.isGrounded && yVelocity < 0)
        {
            jumpCount          = 0;
            yVelocity           = 0;
            transform.position = new Vector3(transform.position.x, groundCheck.surfacePosition.y + playerHeight, transform.position.z);
        }

        var jump = Input.GetKeyDown(KeyCode.Space) && jumpCount < 2; 
        animator.SetBool(animJumpHash, jump);
        if (jump) 
        {
            jumpCount ++;
            yVelocity     = jumpForceY;
            xRuntimeSpeed = jumpForceX;
        }

        xRuntimeSpeed = Mathf.Lerp(xRuntimeSpeed, xSpeed, xSpeedLerpSpeed);
        var horizontalInput    = Input.GetAxis("Horizontal");
        var horizontalMovement = horizontalInput * xRuntimeSpeed * Time.deltaTime;

        transform.Translate(new Vector3(horizontalMovement, yVelocity, 0) * Time.deltaTime);
    }

    private void Awake()
    {
        groundCheck      = GetComponent<GroundCheckWithSnapping>();
        playerHeight     = transform.GetComponent<BoxCollider2D>().size.y / 2;
        animator         = GetComponentInChildren<Animator>();
        animOnGroundHash = Animator.StringToHash("OnGround");
        animJumpHash     = Animator.StringToHash("Jump");
    }
}