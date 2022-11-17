using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public class EffectCarrier : MonoBehaviour
    {
        [SerializeReference]
        public List<EffectRuntimeBase> effects;
        
    }
}