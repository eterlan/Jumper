using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FSM;
using UnityEngine;

namespace GamePlay
{
    public class EffectManager
    {
        public ControllerBase owner;

        public EffectManager(ControllerBase owner)
        {
            this.owner = owner;
            
        }
    }

    // TEST 被添加的一瞬间就开始执行 
    

    public class DelayAdd : EffectRuntimeBase
    {
        public override void Execute()
        {
            base.Execute();
        }
    }
}