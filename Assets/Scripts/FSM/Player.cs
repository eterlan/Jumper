using System;
using Cinemachine;
using UnityEngine;

namespace FSM
{
    [SaveDuringPlay]
    public class Player : ControllerBase
    {
        //  Behaviour
        [Header("行为配置")]
        public float originXSpeed = 5;
        public float xSpeedLerpSpeed = 0.1f; 
        
        public int   jumpMaxCount       = 2;
        public float jumpCancelVelocity = 5;
        public float jumpVelocity       = 17;
        public float minJumpDuration    = 0.1f;
        public float jumpForceX         = 20;

        public float dropForce = 25;
        
        public float          dashLerp = 0.1f;
        public AnimationCurve dashCurve;
        public float          dashMaxSpeed = 20;
        public float          dashDuration = 0.2f;
        
        public float checkWallTolerance = 0.01f;


        [Header("运行时参数")]
        public int jumpCount;
        public  bool          canDash;
        public  int          dashCount;
        public bool          lockFaceDirection;
        // [NonSerialized][ShowInInspector]
        public  float         xRuntimeSpeed;
        public  FaceDirection faceDirection;   
        
        public FSMManager playerFSM; //= new FSMManager();
        public FSMManager groundStateFSM;
        
        // Component
        public BoxCollider2D groundCheckCollider;
        public Collider2D[]  contactsWithGround = new Collider2D[2];

        private void Awake() 
        {
            rb2d         = GetComponent<Rigidbody2D>();
            animator     = GetComponentInChildren<Animator>();
            groundFilter = new ContactFilter2D {layerMask = LayerMask.GetMask("Ground"), useLayerMask = true, useTriggers = true};
            
            animOnGroundHash = Animator.StringToHash("OnGround");
            animDashHash     = Animator.StringToHash("Dash");

            var idle = new Idle();
            playerFSM     = new FSMManager(this, new FsmState[]{idle, new Jumping(), new Dash(), new Drop()}, idle);
            
            var onGround = new OnGround();
            groundStateFSM = new FSMManager(this, new FsmState[] {onGround, new Falling()}, onGround);
        }

        private bool IsGround()
        {
            var count = Physics2D.GetContacts(groundCheckCollider, groundFilter, contactsWithGround);
            for (var i = 0; i < count; i++)
            {
                var closestPoint = contactsWithGround[i].ClosestPoint(groundCheckCollider.bounds.center);
                var diff         = (Vector2)groundCheckCollider.bounds.center - closestPoint;
                if (Mathf.Abs(diff.y - groundCheckCollider.bounds.extents.y) < checkWallTolerance)
                {
                    return true;
                    // Debug.Log("贴着地板"); 
                }
                // if (Mathf.Abs(diff.x - groundCheckCollider.bounds.extents.x) < checkWallTolerance)
                // {
                //     return true;
                //     //Debug.Log("贴着墙壁");
                // }
            }

            return false;
        }
        private void Update()
        {
            // isGround = rb2d.IsTouching(groundFilter);
            var isTouchingGroundOrWall = Physics2D.IsTouching(groundCheckCollider, groundFilter);
            isGround = isTouchingGroundOrWall && IsGround();
            animator.SetBool(animOnGroundHash, isGround);
            
            var jumpPressed = Input.GetButtonDown("Jump");
            if (jumpPressed) playerFSM.SwitchState<Jumping>(repeatEnter: true);
            var dashPressed = Input.GetMouseButtonDown(1);
            if (dashPressed) playerFSM.SwitchState<Dash>(repeatEnter: true);
            var dropPressed = Input.GetMouseButtonDown(0);
            if (dropPressed) playerFSM.SwitchState<Drop>();
            playerFSM.Update();
            groundStateFSM.Update();
            
            var horizontalInput = Input.GetAxis("Horizontal");
            if (!lockFaceDirection)
            {
                Move(horizontalInput);
                UpdateFaceDirection(horizontalInput);
            }
        }

        private void Move(float horizontalInput)
        {
            // multiplier
            xRuntimeSpeed   = Mathf.Lerp(xRuntimeSpeed, originXSpeed, xSpeedLerpSpeed);
            var xSignedSpeed = xRuntimeSpeed * horizontalInput; 
            rb2d.velocity = new Vector2(xSignedSpeed, rb2d.velocity.y);
            // Debug.Log(rb2d.velocity);
        }

        private void UpdateFaceDirection(float horizontalInput)
        {
            if (horizontalInput != 0) faceDirection        = horizontalInput > 0 ? FaceDirection.Right : FaceDirection.Left;
            if (Input.GetKeyDown(KeyCode.A)) faceDirection = FaceDirection.Left;
            if (Input.GetKeyDown(KeyCode.D)) faceDirection = FaceDirection.Right;
        }
        
        public override void OnGround()
        {
            jumpCount = 0;
            dashCount = 0;
            playerFSM.SwitchState<Idle>();
        }

        private void OnEnable()
        {
            playerFSM.SwitchState<Idle>();
            lockFaceDirection = false;
            
        }
    }
    // TEST 能同时处于 空中, 能移动, 能冲刺, 能下坠
    
}