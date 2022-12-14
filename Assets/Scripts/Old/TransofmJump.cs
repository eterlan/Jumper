
using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

[SaveDuringPlay]
public class TransformJump : MonoBehaviour
{
    public  FaceDirection           faceDirection;
    private bool                    lockFaceDirection;
    public  GroundCheckWithSnapping groundCheck;
    public  float                   jumpForceY   = 20;
    public  float                   dropForce    = 20;
    public  float                   jumpForceX   = 20;
    public  float                   gravity      = -9.81f;
    public  float                   gravityScale = 5;
    [FormerlySerializedAs("xSpeed")]
    public float                   originXSpeed          = 5;
    public float                   xSpeedLerpSpeed = 0.2f;
    
    [SerializeField]
    private float yVelocity;

    [SerializeField]
    private float xRuntimeSpeed;

    public  BoxCollider2D bodyCollider;
    public float         playerHeight;
    private int           jumpCount = 2;
    private int           dashCount = 2;

    private Animator animator;
    private int      animOnGroundHash;
    private int      animJumpHash;
    private int      animDashHash;

    public  bool           lerpDash;
    public  float          dashLerp;
    public  AnimationCurve dashCurve;
    public  float          dashMaxSpeed;
    public  float          dashDuration;
    private float          dashTimeRemain;

    private int     m_groundLayer;
    private int     m_obstacleLayer;
    private Vector2 tryMoveDelta;

    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");

        
        if (!lockFaceDirection)
        {
            if (horizontalInput != 0)
            {
                faceDirection = horizontalInput > 0 ? FaceDirection.Right : FaceDirection.Left;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                faceDirection = FaceDirection.Left;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                faceDirection = FaceDirection.Right;
            }    
        }
        
        yVelocity += gravity * gravityScale * Time.deltaTime; 
        animator.SetBool(animOnGroundHash, groundCheck.isGrounded);
        if (groundCheck.isGrounded && yVelocity < 0)
        {
            Reset();
            transform.position = new Vector3(transform.position.x, groundCheck.surfacePosition.y + playerHeight, transform.position.z);
        }
        
        var jump = Input.GetKeyDown(KeyCode.Space) && jumpCount < 2; 
        animator.SetBool(animJumpHash, jump);
        if (jump) 
        {
            jumpCount++;
            yVelocity     = jumpForceY;
            xRuntimeSpeed = jumpForceX;
        }

        var drop = Input.GetMouseButtonDown(0) && !groundCheck.isGrounded; 
        if (drop)
        {
            yVelocity = -dropForce; 
        }
        
        xRuntimeSpeed = Mathf.Lerp(xRuntimeSpeed, originXSpeed, xSpeedLerpSpeed);
        var horizontalMovement = horizontalInput * xRuntimeSpeed;

        var dash = Input.GetMouseButtonDown(1) && dashCount < 2; 
        animator.SetBool(animDashHash, dash);
        if (dash)
        {
            dashCount++;
            dashTimeRemain = dashDuration;
            xRuntimeSpeed  = dashMaxSpeed;
        }
        
        var isDashing = dashTimeRemain > 0;
        lockFaceDirection = isDashing;
        if (isDashing)
        {
            dashTimeRemain -= Time.deltaTime;
            yVelocity      =  0;
            
            if (lerpDash)
            {
                xRuntimeSpeed = Mathf.Lerp(xRuntimeSpeed, originXSpeed, dashLerp);
            }
            else
            {
                var normalizedTime = 1 - dashTimeRemain / dashDuration;
                var sampleResult   = dashCurve.Evaluate(normalizedTime);
                xRuntimeSpeed = Mathf.Lerp(dashMaxSpeed, originXSpeed, sampleResult);
            }

            horizontalMovement = (faceDirection == FaceDirection.Right ? 1 : -1) * xRuntimeSpeed; 
        }


        tryMoveDelta = new Vector2(horizontalMovement, yVelocity) * Time.deltaTime;
        if (!CanMove(tryMoveDelta + (Vector2)transform.position, bodyCollider.size, out var col))
        {
            var closestPos = Physics2D.ClosestPoint(transform.position, col);
            var dist       = closestPos - (Vector2)transform.position;
            if (Mathf.Abs(dist.x) > Mathf.Abs(dist.y))
            {
                tryMoveDelta.x = 0;
            }
            else
            {
                tryMoveDelta.y = 0;
                yVelocity      = 0;
            }
        }
        transform.Translate(tryMoveDelta);

        // var prevPos       = transform.position;
        // var timeSinceLoad = Time.timeSinceLevelLoad;
        // transform.position = new Vector3(prevPos.x, 0.5f * gravity * timeSinceLoad * timeSinceLoad, prevPos.z); 
        void Reset()
        {
            jumpCount = 0;
            dashCount = 0;
            yVelocity = 0;
        }
    }

    private void Awake()
    {
        groundCheck      = GetComponentInChildren<GroundCheckWithSnapping>();
        //playerHeight = bodyCollider.size.y / 2;
        animator         = GetComponentInChildren<Animator>();
        animOnGroundHash = Animator.StringToHash("OnGround");
        animJumpHash     = Animator.StringToHash("Jump");
        animDashHash     = Animator.StringToHash("Dash");
        m_groundLayer    = LayerMask.NameToLayer("Ground");
        m_obstacleLayer  = LayerMask.NameToLayer("Obstacle");
    }

    private bool CanMove(Vector2 checkCenter, Vector2 size, out Collider2D col)
    {
        var filterLayer = 1 << m_groundLayer | 1 << m_obstacleLayer;
        col = Physics2D.OverlapBox(checkCenter, size, 0, filterLayer);
        if (col == null)
        {
            return true;
        }

        var colLayer = col.gameObject.layer;
        if (colLayer == m_groundLayer)
        {
            return yVelocity > 0;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(tryMoveDelta + (Vector2)transform.position, bodyCollider.size);
    }
}