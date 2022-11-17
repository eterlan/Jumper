using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UITween
{
    public class UITween : SerializedMonoBehaviour
    {
        public Component        UITarget;
        public List<MethodInfo> methodInfos;
        
        [ValueDropdown(nameof(GetMethod))]
        public int           SelectMethod;


        [Button]
        public void InvokeTween()
        {
            
        }

        private void OnGUI()
        {
            
        }

        public IEnumerable GetMethod()
        {
            var            type = UITarget.GetType();
            methodInfos = type.GetExtensionMethods().ToList();
            var temp = new ValueDropdownList<int>();
            for (var i = 0; i < methodInfos.Count; i++)
            {
                var methodInfo = methodInfos[i];
                foreach (var parameterInfo in methodInfo.GetParameters())
                {
                }
                temp.Add(methodInfo.Name, i);
            }

            return temp;
        }
    }
}