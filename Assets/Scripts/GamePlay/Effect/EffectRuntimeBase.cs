using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay
{
    [Serializable]
    public abstract class EffectRuntimeBase
    {
        public string name;
        // trigger frequency
        // Source
        /// <summary>
        /// 持续时间
        /// 特殊值: 1 = 执行一次 / -1 = 一直执行
        /// </summary>
        public float duration;
        public float lifeTime;
        public float triggerInterval;

        public virtual void Init()
        {
            
        }
        
        [Button]
        public async void Test ()
        {
            while (lifeTime < duration || Mathf.Approximately(duration, -1))
            {
                Execute();
                await UniTask.Delay(TimeSpan.FromSeconds(triggerInterval));
                lifeTime += triggerInterval;
            }
        }
        
        public virtual void Execute()
        {
            
        }
        
        // parameter
        
    }
}