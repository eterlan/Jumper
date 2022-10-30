namespace FSM
{
    public class Falling : FsmState
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void CheckSwitchCondition()
        {
            if (controller.isGround)
            {
                manager.SwitchState<OnGround>();
            }
        }
    }
}