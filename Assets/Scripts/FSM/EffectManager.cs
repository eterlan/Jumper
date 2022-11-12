using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FSM
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

    public abstract class Effect
    {
        // trigger frequency
        // Source
        /// <summary>
        /// 持续时间
        /// 特殊值: 1 = 执行一次 / -1 = 一直执行
        /// </summary>
        public float duration;
        public float lifeTime;
        public float triggerFrequency;

        public virtual void Init()
        {
            List<int> s;
        }

        public async void Test ()
        {
            while (lifeTime < duration || Mathf.Approximately(duration, -1))
            {
                Execute();
                await UniTask.Delay(TimeSpan.FromSeconds(triggerFrequency));
                lifeTime += triggerFrequency;
            }
        }
        
        public virtual void Execute()
        {
            
        }
        
        // parameter
        
    }

    public class DelayAdd : Effect
    {
        public override void Execute()
        {
            base.Execute();
        }
    }
}