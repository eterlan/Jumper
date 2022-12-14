using System;
using GamePlay;
using UnityEngine;

namespace FSM
{
    [Serializable]
    public class FSMManager
    {
        public ControllerBase controllerBase;
        public FsmState[]     states;
        public FsmState       currentState;
        public FsmState       prevState;
        public FSMManager(ControllerBase controller, FsmState[] states, FsmState defaultState)
        {
            controllerBase = controller;
            this.states    = states;
            foreach (var state in states)
            {
                state.Init(this, controller);
            }
            currentState = defaultState;
            prevState    = currentState;
            currentState.Enter();
        }

        public void Update()
        {
            if (prevState != currentState)
            {
                prevState.Exit(); 
                Debug.Log($"退出{prevState.GetType() }");
                currentState.Enter();
                prevState = currentState;
                Debug.Log($"进入{currentState.GetType()}");
            }
            currentState.Update();
        }
        
        public bool IsState<T>() where T : FsmState
        {
            return currentState is T;
        }
        public bool TryGetState<T>(out FsmState state) where T : FsmState
        {
            for (var i = 0; i < states.Length; i++)
                if (states[i] is T)
                {
                    state = states[i];  
                    return true;
                }
            state = null;
            return false;
        }

        public FsmState SwitchState<T>(bool repeatEnter = false) where T : FsmState
        {
            if (!repeatEnter && currentState is T)
            {
                return currentState;
            }
            
            if (TryGetState<T>(out var newState))
            {
                if (newState.EnterCondition())
                {
                    prevState    = currentState;
                    currentState = newState;
                    return currentState;
                }

                Debug.Log($"{controllerBase}不满足进入{typeof(T)}的条件");
                return currentState;
            }

            Debug.LogError($"在{controllerBase}上找不到指定状态{typeof(T)}");
            return currentState;
        }
    }

}