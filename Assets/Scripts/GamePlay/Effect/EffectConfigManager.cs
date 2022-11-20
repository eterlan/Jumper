using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay
{
    /// <summary>
    /// 效果配置文件集合
    /// </summary>
    [CreateAssetMenu(fileName = nameof(EffectConfigManager), menuName = "GamePlay/" + nameof(EffectConfigManager))]
    public class EffectConfigManager : SerializedScriptableObject
    {
        [SerializeField]
        private Dictionary<string, EffectConfig> effectConfigs;

        public bool GetEffectConfig(string effectName, out EffectConfig config)
        {
            if (effectConfigs.TryGetValue(effectName, out config))
            {
                return true;
            }
    
            Debug.Log($"效果配置集合中不存在{effectName}配置, 请检查名字");
            return false;
        }
        
        
        private void OnEnable()
        {
                   
        }
    }

    /// <summary>
    /// 效果配置文件
    /// </summary>
    [Serializable]
    public class EffectConfig
    {
        public float      duration;
        public float      triggerInterval;
        [FormerlySerializedAs("effectType")]
        public StatType StatType;
        public float      changeValue;

        public void ApplyEffect(ref float value)
        {
            
        }
    }
}