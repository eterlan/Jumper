namespace FSM
{
    public class OnGround : PlayerFSMState
    {
        public override void CheckSwitchCondition()
        {
            if (!player.isGround)
            {
                manager.SwitchState<Falling>();
            }
        }
    }
}