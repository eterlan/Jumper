using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(FloatVariable), menuName = "变量/" + nameof(FloatVariable))]
public class FloatVariable : ScriptableObject
{
    [SerializeField]
    private float value;

    public event Action<float, float> OnValueChanged;  

    public float Value
    {
        get
        {
            return value;
        }
        set
        {
            if (!Mathf.Approximately(value, this.value))
            {
                OnValueChanged?.Invoke(this.value, value);
            }
        }
    }
}