using UnityEngine;

namespace FSM
{
    public class Falling : PlayerFSMState
    {
        private float m_velocityBeforeGrounded;
        public override void CheckSwitchCondition()
        {
            if (controller.isGround)
            {
                Debug.Log("Ground");
                var fallingDmg = CalculateFallingDamage(m_velocityBeforeGrounded);
                if (fallingDmg > 0)
                {
                    player.HP.Value -= fallingDmg;  
                }

                if (!player.playerFSM.IsState<Death>())
                {   
                    manager.SwitchState<OnGround>();
                }
            }

            // 
            var yVelocity = player.rb2d.velocity.y;
            if (yVelocity < m_velocityBeforeGrounded)
            {
                m_velocityBeforeGrounded = yVelocity;
            }
        }
        
        private float CalculateFallingDamage(float fallingSpeed)
        {
            var dmg = MathHelper.Remap(fallingSpeed, player.dmgMinVelocity, player.dmgMaxVelocity, 0, 100);
            return dmg;
        }
    }
    

}