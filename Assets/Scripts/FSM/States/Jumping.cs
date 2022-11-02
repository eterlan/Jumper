using UnityEngine;

namespace FSM
{
    public class Jumping : PlayerFSMState
    {
        // Behaviour
        // public bool isJumping;
        // public bool canJump;

        public float jumpingElapsedTime;
        public bool  cacheJumpCancel;
        
        private int m_animJumpHash;


        public Jumping()
        {
            m_animJumpHash = Animator.StringToHash("Jump");
        }
        public override bool EnterCondition()
        {
            return player.jumpCount < player.jumpMaxCount;
        }

        public override void Enter()
        {
            // 进入跳跃状态
            base.Enter();
            player.animator.SetTrigger(m_animJumpHash);
            player.jumpCount++;
            player.rb2d.velocity = new Vector2(player.rb2d.velocity.x, player.jumpVelocity);
            jumpingElapsedTime   = 0;
            player.dashCount     = 0;
            player.xRuntimeSpeed = player.jumpForceX; 
        }

        public override void Update()
        {
            base.Update();
            jumpingElapsedTime += Time.deltaTime;
        
            Cancel();
            void Cancel()
            {
                var jumpReleased = Input.GetButtonUp("Jump");
                var askForCancel = (jumpReleased || cacheJumpCancel) && player.rb2d.velocity.y > 0;
                if (!askForCancel) return;
                
                var canCancel = jumpingElapsedTime > player.minJumpDuration;
                if (canCancel) 
                {
                    player.rb2d.velocity = new Vector2(player.rb2d.velocity.x, player.jumpCancelVelocity);
                    cacheJumpCancel      = false;
                }
                else
                {
                    cacheJumpCancel = true;
                }
            }
        }

        public override void CheckSwitchCondition()
        {
            if (player.rb2d.velocity.y < 0)
            {
                manager.SwitchState<Idle>(); 
            }
        }

        public override void Exit()
        {
            base.Exit();
            // 退出跳跃中状态
            cacheJumpCancel    = false;
            jumpingElapsedTime = 0;
        }
    }
}