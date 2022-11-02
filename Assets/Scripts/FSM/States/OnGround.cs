namespace FSM
{
    public class OnGround : Everywhere
    {
        public override void Enter()
        {
            base.Enter();
            controller.OnGround();
        }

        public override void CheckSwitchCondition()
        {
            if (!controller.isGround)
            {
                manager.SwitchState<Falling>();
            }
        }
    }
}