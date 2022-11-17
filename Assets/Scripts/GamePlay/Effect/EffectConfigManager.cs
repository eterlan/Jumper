using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using Sirenix.OdinInspector;
using UnityEngine;

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

        public EffectConfig GetEffectConfig(string effectName)
        {
            if (effectConfigs.TryGetValue(effectName, out var effect))
            {
                return effect;
            }
    
            Debug.Log($"效果配置集合中不存在{effectName}配置, 请检查名字");
            return null;
        }
        
        
        private void OnEnable()
        {
                   
        }
    }

    public enum EffectType
    {
        /// <summary>
        /// 生命值
        /// </summary>
        Hp,
        /// <summary>
        /// 耐力
        /// </summary>
        Sta
    }
    
    /// <summary>
    /// 效果配置文件
    /// </summary>
    [Serializable]
    public class EffectConfig
    {
        public float      duration;
        public float      triggerInterval;
        public EffectType effectType;
        public float      changeValue;

        public void ApplyEffect(ref float value)
        {
            
        }
    }
}