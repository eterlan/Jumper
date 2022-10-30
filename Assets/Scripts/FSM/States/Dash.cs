using UnityEngine;

namespace FSM
{
    public class Dash : PlayerFSMState
    {
        private float m_dashTimeRemain;
        private int   m_animDashHash;

        public Dash()
        {
            m_animDashHash = Animator.StringToHash("Dash");
        }
        public override void Enter()
        {
            base.Enter();
            player.animator.SetTrigger(m_animDashHash);
            player.dashCount++;
            m_dashTimeRemain     = player.dashDuration;
            player.xRuntimeSpeed = player.dashMaxSpeed;

            player.lockFaceDirection = true;
        }

        public override void Exit()
        {
            base.Exit();
            player.lockFaceDirection = false;
        }

        public override void Update()
        {
            base.Update();
            m_dashTimeRemain -= Time.deltaTime;
            
            var normalizedTime = 1 - m_dashTimeRemain / player.dashDuration;
            var sampleResult   = player.dashCurve.Evaluate(normalizedTime);
            player.xRuntimeSpeed = Mathf.Lerp(player.dashMaxSpeed, player.originXSpeed, sampleResult);
            var signedXSpeed = (player.faceDirection == FaceDirection.Right ? 1 : -1) * player.xRuntimeSpeed;
            player.rb2d.velocity = new Vector2(signedXSpeed, 0);
        }

        public override bool EnterCondition()
        {
            return player.dashCount < 2;
        }

        public override void CheckSwitchCondition()
        {
            if (m_dashTimeRemain < 0) manager.SwitchState<Falling>();
        }
    }
}