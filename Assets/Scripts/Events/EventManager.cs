using System.Collections.Generic;
using UnityEngine.Events;

namespace Events
{
    public class EventManager
    {
        public CustomEvent sth;
    }

    public class CustomEvent : UnityEvent
    {
    }

    public class Enemy
    {
        public int            ID;
        public InstancedFloat HP;
        
    }

    public class UI
    {
        public int            ID;
        public InstancedFloat EnemyHP;

        public UI(int id)
        {
            ID = id;
        }

        public float          hp => EnemyHP.values[ID];

    }

    public class EnemyManager
    {
        public Dictionary<int, Enemy> enemies;
        public Enemy CreateEnemy()
        {
            var enemy = new Enemy
            {
                ID = enemies.Count
            };
            return enemy;
        }
    }

    public class InstancedFloat
    {
        public Dictionary<int, float> values;
        
    } 
}