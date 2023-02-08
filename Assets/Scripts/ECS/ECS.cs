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
//             componentData = (T)s;
//             return true;
//         }
//
//         public bool TryAddComponent<T>(int entity) where T : IComponentData
//         {
//             ComponentMap<T> s;
//             if (!ecsRelations.ContainsKey(typeof(T)))
//             {
//                 s = new ComponentMap<IComponentData>();
//                 ecsRelations.Add(typeof(T), newComponentMap);
//             }
//         }
//
//         public void TEST()
//         {
//             IEnumerable<String> strings = new List<String>();
//             IEnumerable<Object> objects = strings;
//
//             Dictionary<string, string> a = objects;
//
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
//         public bool             HasComponent(int entity) => componentDataMap.ContainsKey(entity);
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