using System;
using UnityEngine;

public class Tester : MonoBehaviour
{
    private void Start()
    {
        var a = new A();
        var b = a;
        a.a = 10;
        Debug.Log(b.a);

        var c = new A();
        var d = c;
        d.a = 12;
        Debug.Log(c.a);
    }

    private void Update()
    {
    }
}

public class A
{
    public float a;
}