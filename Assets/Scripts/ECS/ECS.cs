#define ECS_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace ECS
{
    public class Ecs
    {
        public static Ecs                             instance = new();
        
        private       Dictionary<Type, IComponentMap> ecsRelations = new();

        public bool TryGetComponentData<T>(int entity, out T componentData) where T : IComponentData
        {
            if (!ecsRelations.TryGetValue(typeof(T), out var componentMap))
            {
                componentData = default;
                return false;
            }

            componentData = default;
            
            if (!((ComponentMap<T>)componentMap).TryGetComponentData(entity, out var s)) return false;
            componentData = (T)s;
            return true;
        }

        public bool TryAddComponent<T>(int entity, T data) where T : IComponentData
        {
            if (!ecsRelations.TryGetValue(typeof(T), out var map))
            {
                map = new ComponentMap<T>();
                ecsRelations.Add(typeof(T), map);
            }

            return ((ComponentMap<T>)map).TryAdd(entity, data);
        }

        public void Foreach<T>(Action<T> action) where T : IComponentData
        {
            if (!HasComponentMap<T>())
                return;

            var componentMap = ecsRelations[typeof(T)];
            ((ComponentMap<T>)componentMap).GetComponents().ForEach(c => action((T)c));
        }

        public void Foreach<T1, T2>(out int entity, Action<T1, T2> action) where T1 : IComponentData where T2 : IComponentData
        {
            e
            if (!HasComponentMap<T1>() || !HasComponentMap<T2>())
                return;

            var componentMapT1 = (ComponentMap<T1>)ecsRelations[typeof(T1)];
            var componentMapT2 = (ComponentMap<T2>)ecsRelations[typeof(T2)];
            var entitiesT1     = componentMapT1.GetEntities();
            var entities       = entitiesT1.Where(entityT1 => componentMapT2.Contains(entityT1));
            entities.ForEach(entity => action(componentMapT1[entity], componentMapT2[entity]));
        }
        
        public void Foreach<T1, T2, T3>(Action<T1, T2, T3> action) where T1 : IComponentData where T2 : IComponentData where T3 : IComponentData
        {
            if (!HasComponentMap<T1>() || !HasComponentMap<T2>() || !HasComponentMap<T3>())
                return;

            var componentMapT1 = (ComponentMap<T1>)ecsRelations[typeof(T1)];
            var componentMapT2 = (ComponentMap<T2>)ecsRelations[typeof(T2)];
            var componentMapT3 = (ComponentMap<T3>)ecsRelations[typeof(T3)];
            var entities     = componentMapT1.GetEntities();
            entities       = entities.Where(entityT1 => componentMapT2.Contains(entityT1) && componentMapT3.Contains(entityT1));
            
            entities.ForEach(entity => action(componentMapT1[entity], componentMapT2[entity], componentMapT3[entity]));
        }

        public bool HasComponentMap<T>()
        {
            if (ecsRelations.ContainsKey(typeof(T))) return true;
#if ECS_DEBUG
            Debug.LogWarning($"不存在该component: {typeof(T)}");
            return false;
#endif
        }
    }

    public class ComponentMap<T> : IComponentMap where T : IComponentData
    {
        public T this[int entity]
        {
            get => componentDataMap[entity];
            set => componentDataMap[entity] = value;
        }
        private Dictionary<int, T> componentDataMap;
    
        public ComponentMap()
        {
            componentDataMap = new Dictionary<int, T>();
        }
        public bool TryGetComponentData(int entity, out T componentData)
        {
            return componentDataMap.TryGetValue(entity, out componentData);
        }
    
        public bool Contains(int entity) => componentDataMap.ContainsKey(entity);

        public bool TryAdd(int entity, T componentData)
        {
            return componentDataMap.TryAdd(entity, componentData);
        }

        public IEnumerable<T> GetComponents()
        {
            return componentDataMap.Values;
        }

        public IEnumerable<int> GetEntities()
        {
            return componentDataMap.Keys;
        }

    }

    public interface IComponentMap
    {
        
    }
    public interface IComponentData
    {
        
    }
}