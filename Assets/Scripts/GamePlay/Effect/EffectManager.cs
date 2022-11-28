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
}