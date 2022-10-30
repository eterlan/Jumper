namespace FSM
{
    public class OnGround : PlayerFSMState
    {
        public override void Enter()
        {
            base.Enter();
            player.jumpCount = 0;
            player.dashCount = 0;
        }
    }
}