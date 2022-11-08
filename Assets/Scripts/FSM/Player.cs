using System;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Variable;

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

        public int            dashMaxCount = 2;
        public float          dashLerp     = 0.1f;
        public AnimationCurve dashCurve;
        public float          dashMaxSpeed = 20;
        public float          dashDuration = 0.2f;
        
        public float checkWallTolerance = 0.01f;

        // 根据速度决定镜头距离
        public Transform              focusTargetPos;
        public CinemachineTargetGroup targetGroup;
        public float                  clampTargetYMin = -20;
        public float                  clampTargetYMax = -5;
        public float                  clampMultiplier = 1;
        public float                  dampSpeed       = 0.2f;
        
        // 坠落伤害
        public float dmgMinVelocity = -40;
        public float dmgMaxVelocity = -100;
        public float playerMaxHp    = 100;
        
        // 伤害UI
        [FormerlySerializedAs("hpHighlightUITransitionDuration")]
        public float hpSlowTransitionDuration = 0.1f;
        [FormerlySerializedAs("hpCurrentUITransitionDuration")]
        public float hpFastTransitionDuration   = 0.2f;
        [FormerlySerializedAs("hpUIHighLight")]
        public Image hpUIReduceHighlight;
        public Image hpUICurrent;
        public Image hpUIAddHighlight;

        [Header("运行时参数")]
        public FloatVariable HP;
        public int remainJumpCount;
        public  bool          canDash;
        public  int          remainDashCount;
        public bool          lockFaceDirection;
        // [NonSerialized][ShowInInspector]
        public  float         xRuntimeSpeed;
        public  FaceDirection faceDirection;   
        
        public FSMManager playerFSM; //= new FSMManager();
        
        // Component
        public BoxCollider2D groundCheckCollider;
        public Collider2D[]  contactsWithGround = new Collider2D[2];

        
        private float dampTargetY;


        private void Awake()
        {
            targetGroup  = (Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera).GetCinemachineComponent<CinemachineFramingTransposer>().FollowTargetGroup;
            rb2d         = GetComponent<Rigidbody2D>();
            animator     = GetComponentInChildren<Animator>();
            groundFilter = new ContactFilter2D {layerMask = LayerMask.GetMask("Ground"), useLayerMask = true, useTriggers = true};
            
            animOnGroundHash = Animator.StringToHash("OnGround");
            animDashHash     = Animator.StringToHash("Dash");

            var idle = new OnGround();
            playerFSM         =  new FSMManager(this, new FsmState[]{idle, new Falling(), new Jumping(), new Dash(), new Drop(), new Death()}, idle);
            HP.Value          =  playerMaxHp;
            HP.OnValueChanged += (old, @new) =>
            {
                Debug.Log($"old: {old}, new: {@new}");
                UpdateHp(old, @new);
                if (@new < 0)
                {
                    playerFSM.SwitchState<Death>();
                }
            }; 

            focusTargetPos.localPosition = new Vector3(0, clampTargetYMax, 0);
            dampTargetY                  = clampTargetYMax;
        }

        private bool IsGround()
        {
            contactsWithGround.Initialize();
            var count = Physics2D.GetContacts(groundCheckCollider, groundFilter, contactsWithGround);
            for (var i = 0; i < count; i++)
            {
                var bounds       = groundCheckCollider.bounds;
                var closestPoint = contactsWithGround[i].ClosestPoint(bounds.center);
                var diff         = (Vector2)bounds.center - closestPoint;
                var tolerance    = bounds.extents.y;
                if (Mathf.Abs(diff.y - tolerance) <= tolerance)
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

            animator.SetBool(animOnGroundHash, isGround);
            
            var jumpPressed = Input.GetButtonDown("Jump");
            if (jumpPressed) playerFSM.SwitchState<Jumping>(repeatEnter: true);
            var dashPressed = Input.GetMouseButtonDown(1);
            if (dashPressed) playerFSM.SwitchState<Dash>(repeatEnter: true);
            var dropPressed = Input.GetMouseButtonDown(0);
            if (dropPressed) playerFSM.SwitchState<Drop>();
            
            
            var isTouchingGroundOrWall = Physics2D.IsTouching(groundCheckCollider, groundFilter) && rb2d.velocity.y <= 0;
            
            isGround = isTouchingGroundOrWall && IsGround();
            if (isGround) OnGround();
            //Debug.Log($"isGround: {isGround}, rb2d.pos: {rb2d.position}, rb2d.velocity: {rb2d.velocity}, tranPOs: {transform.position}");
            playerFSM.Update();
            
            var horizontalInput = Input.GetAxis("Horizontal");
            if (!lockFaceDirection)
            {
                Move(horizontalInput);
                UpdateFaceDirection(horizontalInput);
            }
            
            UpdateLookDownTarget();
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

        private void UpdateLookDownTarget()
        {  
            var targetY = Mathf.Clamp(rb2d.velocity.y * clampMultiplier, clampTargetYMin, clampTargetYMax);
            dampTargetY = Mathf.Lerp(dampTargetY, targetY, dampSpeed);
            var targetPos = new Vector3(0, dampTargetY , 0);
            focusTargetPos.localPosition = targetPos;
        
        }
        public override void OnGround()
        {
            remainJumpCount = jumpMaxCount;
            remainDashCount = dashMaxCount;
        }

        private void OnEnable()
        {
            playerFSM.SwitchState<OnGround>();
            lockFaceDirection = false;
        }

        // TODO HP UI
        [Button]
        private void UpdateHp(float oldHp, float newHp)
        {
            var normalizedHpTarget     = newHp / playerMaxHp;
            if (newHp < oldHp)
            {
                hpUICurrent.DOFillAmount(normalizedHpTarget, hpFastTransitionDuration);
                hpUIReduceHighlight.DOFillAmount(normalizedHpTarget, hpSlowTransitionDuration);
                hpUIAddHighlight.DOFillAmount(normalizedHpTarget, hpFastTransitionDuration);
            }
            else
            {
                hpUICurrent.DOFillAmount(normalizedHpTarget, hpSlowTransitionDuration);
                hpUIAddHighlight.DOFillAmount(normalizedHpTarget, hpFastTransitionDuration);
                hpUIReduceHighlight.DOFillAmount(normalizedHpTarget, hpFastTransitionDuration);
            }
            
        }


    }
    // TEST 能同时处于 空中, 能移动, 能冲刺, 能下坠
    
}