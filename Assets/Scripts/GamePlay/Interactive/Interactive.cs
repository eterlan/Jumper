using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GamePlay
{
    public abstract class Interactive : MonoBehaviour 
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
             
        }
    }

    public abstract class Effect
    {
        public async void Trigger()
        {
            await UniTask.Delay(TimeSpan.FromSeconds())
        }
    }

    public class RepeatEffect
    {
        
    }
    
    public class TimerSystem : MonoBehaviour
    {
        
    }
}