using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteInEditMode]
public unsafe class TestSurviveSingleton
{
    public GCHandle handle;
    private TestSurviveSingleton _instance;
    public TestSurviveSingleton instance {
        get
        {
            if (_instance == null)
            {
                _instance = new TestSurviveSingleton();
                handle = GCHandle.Alloc(_instance, GCHandleType.Pinned);
            }
            return _instance;
        }
        private set { }
    }
}
