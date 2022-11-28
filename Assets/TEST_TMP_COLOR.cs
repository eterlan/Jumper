using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TEST_TMP_COLOR : MonoBehaviour
{
    [OnValueChanged(nameof(color1))]
    public Color   color1;
    
    [OnValueChanged(nameof(color32))]
    public Color32 color32;
    
    public TextMeshPro tmp;


    public void ChangeColor1()
    {
        tmp.color = color1;
    }

    public void ChangeColor32()
    {
        tmp.color = color32;
    }
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
