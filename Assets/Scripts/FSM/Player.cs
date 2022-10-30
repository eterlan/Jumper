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

        [Header("运行时参数")]
        public int jumpCount;
        public  bool          canDash;
        public  int          dashCount;
        public bool          lockFaceDirection;
        // [NonSerialized][ShowInInspector]
        public  float         xRuntimeSpeed;
        public  FaceDirection faceDirection;   
        
        public FSMManager fsmManager; //= new FSMManager();
        
        // Component
        public Collider2D groundCheckCollider;

        private void Awake() 
        {
            rb2d         = GetComponent<Rigidbody2D>();
            animator     = GetComponentInChildren<Animator>();
            groundFilter = new ContactFilter2D {layerMask = LayerMask.GetMask("Ground"), useLayerMask = true, useTriggers = true};
            
            animOnGroundHash = Animator.StringToHash("OnGround");
            animDashHash     = Animator.StringToHash("Dash");

            var onGround = new OnGround();
            fsmManager       = new FSMManager(this, new FsmState[]{onGround, new Jumping(), new Dash(), new Falling()}, onGround);
        }

        private void Update()
        {
            // isGround = rb2d.IsTouching(groundFilter);
            isGround = Physics2D.IsTouching(groundCheckCollider, groundFilter);
            animator.SetBool(animOnGroundHash, isGround);

            var jumpPressed = Input.GetButtonDown("Jump");
            if (jumpPressed) fsmManager.SwitchState<Jumping>(repeatEnter: true);
            var dashPressed = Input.GetMouseButtonDown(1);
            if (dashPressed) fsmManager.SwitchState<Dash>(repeatEnter: true);
            
            fsmManager.Update();
            
            var horizontalInput = Input.GetAxis("Horizontal");
            if (!lockFaceDirection)
            {
                Move(horizontalInput);
                UpdateFaceDirection(horizontalInput);
            }
            Drop();
        }

        private void Drop()
        {
            var drop = Input.GetMouseButtonDown(0) && !isGround; 
            if (drop)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, -dropForce); 
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

    }
    // TEST 能同时处于 空中, 能移动, 能冲刺, 能下坠
}