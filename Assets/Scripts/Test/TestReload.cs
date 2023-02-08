using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReload : MonoBehaviour
{
    public TestReload2 TestReload2;
    public TestReload3 testReload3;

    public int a = 3;
    private void Update()
    {
        //Debug.Log("bbbss");
        //Debug.Log(testReload3.a);
        Test();
        testReload3.A(); 
        Debug.Log(testReload3.d);
        Debug.Log(TestReload2.a);
        // Debug.Log(a);

    }

    private void Test()
    {
        Debug.Log("sssss");
        
    }
}