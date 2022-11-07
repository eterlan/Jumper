using UnityEngine;

namespace FSM
{
    public class Death : PlayerFSMState
    {
        public override void Enter()
        {
            base.Enter();
            Debug.Log("YOU ARE DIED");
        }
    }
}