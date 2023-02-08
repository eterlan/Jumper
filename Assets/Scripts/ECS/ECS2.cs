// using System;
// using System.Collections;
// using System.Collections.Generic;
//
// namespace ECS
// {
//     public class Ecs
//     {
//         private Dictionary<Type, IComponentMap> ecsRelations;
//     
//         public bool TryGetComponentData<T>(int entity, out T componentData) where T : IComponentData
//         {
//             if (!ecsRelations.TryGetValue(typeof(T), out var componentMap))
//             {
//                 componentData = default;
//                 return false;
//             }
//
//             componentData = default;
//             var tMap = (ComponentMap<T>)componentMap; 
//             if (!tMap.TryGetComponentData(entity, out var s)) return false;
//             componentData = s;
//             return true;
//         }
//
//         public bool TryAddComponent<T>(int entity, T data) where T : IComponentData
//         {
//             ComponentMap<T> s;
//             if (!ecsRelations.TryGetValue(typeof(T), out var ss))
//             {
//                 s = new ComponentMap<T>();
//                 ecsRelations.Add(typeof(T), s);
//             }
//
//             s = (ComponentMap<T>)ss ;
//             return s.TryAddComponent(entity, data);
//         }
//     }
//
//
//     public class ComponentMap<T> : IComponentMap where T : IComponentData
//     {
//         private Dictionary<int, T> componentDataMap;
//     
//         public ComponentMap()
//         {
//             this.componentDataMap = new Dictionary<int, T>();
//         }
//         public bool TryGetComponentData(int entity, out T componentData)
//         {
//             if (componentDataMap.TryGetValue(entity, out componentData))
//             {
//                 return true;
//             }
//     
//             return false;
//         }
//     
//         public bool HasComponent(int entity) => componentDataMap.ContainsKey(entity);
//
//         public bool TryAddComponent(int entity, T data)
//         {
//             return componentDataMap.TryAdd(entity, data);
//         }
//     }
//
//     public interface IComponentMap
//     {
//         
//     }
//     
//     public interface IComponentData
//     {
//         
//     }
// }