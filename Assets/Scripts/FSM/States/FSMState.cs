using System;
using GamePlay;

namespace FSM
{
    [Serializable]
    public abstract class FsmState
    {
        protected FSMManager     manager;
        protected ControllerBase controller;
        public    Action         onEnterState = delegate {  };
        public    Action         onExitState = delegate {  };

        public void Init(FSMManager manager, ControllerBase controller)
        {
            this.manager    = manager;
            this.controller = controller;
        }
        public virtual bool EnterCondition()
        {
            return true;
        }
        
        public virtual void Enter()
        {
            onEnterState.Invoke();
        }

        public virtual void Exit()
        {
            onExitState.Invoke();
        }

        public virtual void Update()
        {
            // 最好只有一个Exit. 而且由于不确定退出要切换到什么状态, 因此还是手动来吧..
            CheckSwitchCondition();
        }

        public virtual void CheckSwitchCondition()
        {

        }
    }

    public abstract class PlayerFSMState : FsmState
    {
        private   Player m_player;
        protected Player player => m_player ? m_player : m_player = (Player)controller;
    }
}