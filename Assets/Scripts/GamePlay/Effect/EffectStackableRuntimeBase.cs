using System;
using UnityEngine;

namespace GamePlay
{
    public class EffectStackableRuntimeBase : EffectRuntimeBase
    {
        
        public override void Execute()
        {
            base.Execute();
            Debug.Log("Apply Effect");
        }
    }
}