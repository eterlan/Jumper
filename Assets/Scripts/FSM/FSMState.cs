using System;

namespace FSM
{
    public abstract class FsmState
    {
        public Action<string> OnEnterState;
        public Action         OnExitState;

        public virtual void Enter()
        {
            OnEnterState(ToString());
        }

        public virtual void Exit()
        {
            OnExitState();
        }

        public virtual void Update(float deltaTime)
        {
            
        }
    }

    public class OnGround : FsmState
    {
        public override void Enter()
        {
            base.Enter();
        }
    }

    public class InAIr : FsmState
    {
        public override void Enter()
        {
            base.Enter();
        }
    }
}