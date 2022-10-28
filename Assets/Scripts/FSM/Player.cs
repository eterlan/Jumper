using System;
using UnityEngine;

namespace FSM
{
    public class Player : MonoBehaviour
    {
        // Collision
        public  Rigidbody2D     rb2d;
        public  ContactFilter2D groundFilter;

        // Animator
        private Animator animator;  
        private int      animOnGroundHash;
        private int      animJumpHash;
        private int      animDashHash;
        
        // Behaviour
        public bool    canJump;
        public int     jumpMaxCount = 2;
        public int     jumpCount;
        public Vector2 jumpForce = new(10, 0);

        public bool canDash;
        public bool dashCount;

        private void Awake()
        {
            rb2d         = GetComponent<Rigidbody2D>();
            animator     = GetComponentInChildren<Animator>();
            groundFilter = new ContactFilter2D {layerMask = LayerMask.GetMask("Ground"), useLayerMask = true};
            
            animOnGroundHash = Animator.StringToHash("OnGround");
            animJumpHash     = Animator.StringToHash("Jump");
            animDashHash     = Animator.StringToHash("Dash");
        }

        private void Update()
        {
            var isGround = rb2d.IsTouching(groundFilter);
            animator.SetBool(animOnGroundHash, isGround);
            if (isGround && rb2d.velocity.y < 0)
            {
                Reset();
            }

            Jump();
        }

        private void Jump()
        {
            var jumpPressed = Input.GetButtonDown("Jump") && jumpCount < jumpMaxCount;
            animator.SetBool(animJumpHash, jumpPressed);
            if (jumpPressed)
            {
                jumpCount++;
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                rb2d.AddForce(jumpForce, ForceMode2D.Impulse);
            }
        }

        private void Reset()
        {
            jumpCount = 0;
        }
    }
    // TEST 能同时处于 空中, 能移动, 能冲刺, 能下坠
}