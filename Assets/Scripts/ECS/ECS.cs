using System;
using System.Collections.Generic;

namespace ECS
{
    public class Ecs
    {
        private Dictionary<Type, ComponentMap<IComponentData>> ecsRelations;

        public bool TryGetComponentData<T>(int entity, out T componentData) where T : IComponentData
        {
            if (!ecsRelations.TryGetValue(typeof(T), out var componentMap))
            {
                componentData = default;
                return false;
            }

            componentData = default;
            if (!componentMap.TryGetComponentData(entity, out var s)) return false;
            componentData = (T)s;
            return true;
        }
    }

    public class ComponentMap<T> where T : IComponentData
    {
        private Dictionary<int, T> componentDataMap;

        public bool TryGetComponentData(int entity, out T componentData)
        {
            if (componentDataMap.TryGetValue(entity, out componentData))
            {
                return true;
            }

            return false;
        }

        public bool HasComponent(int entity) => componentDataMap.ContainsKey(entity);
        
        
    }

    public interface IComponentData
    {
        
    }
}