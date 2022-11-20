using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Variable;

namespace GamePlay
{
    public class EffectCarrier : MonoBehaviour
    {
        public EffectConfigManager effectConfigManager;
        // [SerializeReference]
        // public List<EffectRuntimeBase> effects;
        [SerializeReference]
        public Dictionary<StatType, FloatVariable> carrierStats;

        public bool FindStat(StatType type, out FloatVariable var)
        {
            if (carrierStats.TryGetValue(type, out var)) return true;
            Debug.Log($"{this}上没有找到{type}属性, 请检查是否添加");
            return false;

        }
        
        // 返回一个Task是因为这个东西是可等待的
        [Button]
        public async void Apply(string effectName)
        {
            if (!effectConfigManager.GetEffectConfig(effectName, out var config)) return;
            if (!FindStat(config.StatType, out var value)) return;
            var lifeTime = 0f;
            while (lifeTime < config.duration || Mathf.Approximately(config.duration, -1))
            {
                value.Value += config.changeValue;
                Debug.Log(value.Value );
                
                await UniTask.Delay(TimeSpan.FromSeconds(config.triggerInterval));
                lifeTime += config.triggerInterval;
            }
        }
    }
}