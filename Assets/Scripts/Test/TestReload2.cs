using System;
using UnityEngine;

public class TestReload2 : MonoBehaviour
{
    public string a;
    private void Update()
    {
        // Debug.Log("asd");
        a = "999dss";
    }
}

[Serializable]
public class TestReload3
{
    public int    a = 3;
    public int    b = 5;
    public int    c = 2;
    public string d = "sada" , e = "qw";  
    public void A()
    {
        Debug.Log("00s0");
    }
}