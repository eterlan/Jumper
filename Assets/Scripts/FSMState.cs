
using System;

public abstract class IFsmState
{
    public Action<string> OnEnterState;
    public Action OnExitState;

    public virtual void Enter()
    {
        OnEnterState(ToString());
    }

    public virtual void Exit()
    {
        OnExitState();
    }
}

public class OnGround : IFsmState
{
    public override void Enter()
    {
        base.Enter();
    }
}

public class InAIr : IFsmState
{
    public override void Enter()
    {
        base.Enter();
    }
}