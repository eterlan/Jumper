using UnityEngine;

namespace FSM
{
    public class Drop : PlayerFSMState
    {
        private ContactFilter2D m_enemyFilter;
        public Drop()
        {
            m_enemyFilter = new ContactFilter2D() { layerMask = LayerMask.GetMask("Enemy"), useLayerMask = true, useTriggers = true };
        }
        public override bool EnterCondition()
        {
            return !player.isGround && player.rb2d.velocity.y > -player.dropForce;
        }

        public override void Enter()
        {
            base.Enter();
            player.rb2d.velocity = new Vector2(player.rb2d.velocity.x, -player.dropForce);
        }

        public override void CheckSwitchCondition()
        {
            var collideWithEnemy = Physics2D.IsTouching(player.groundCheckCollider, m_enemyFilter);
            if (collideWithEnemy)
            {
                // freeze 击杀敌人的反馈
                player.remainJumpCount = player.jumpMaxCount ++;
            }
        }
    }
}