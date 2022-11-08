using UnityEngine;

namespace FSM
{
    public class Falling : PlayerFSMState
    {
        private float m_velocityLastFrame;
        public override void CheckSwitchCondition()
        {
            Debug.Log(player.rb2d.velocity.y);
            if (controller.isGround)
            {
                Debug.Log("Ground");
                var fallingDmg = CalculateFallingDamage(m_velocityLastFrame);
                if (fallingDmg > 0)
                {
                    player.HP.Value -= fallingDmg;  
                }

                if (!player.playerFSM.IsState<Death>())
                {   
                    manager.SwitchState<OnGround>();
                }
            }
            
            m_velocityLastFrame = player.rb2d.velocity.y;
        }
        
        private float CalculateFallingDamage(float fallingSpeed)
        {
            var dmg = MathHelper.Remap(fallingSpeed, player.dmgMinVelocity, player.dmgMaxVelocity, 0, 100);
            return dmg;
        }
    }
    

}