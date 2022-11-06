using UnityEngine;

namespace FSM
{
    public class Falling : PlayerFSMState
    {
        public override void CheckSwitchCondition()
        {
            if (controller.isGround)
            {
                manager.SwitchState<Idle>();
            }
        }
    }
}