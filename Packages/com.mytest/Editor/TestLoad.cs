using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class TestLoad
{
    static TestLoad()
    {
        Debug.LogError("TestLoad");
        EditorApplication.delayCall += Initialize;
    }

    static void Initialize()
    {
        var asset = AssetDatabase.LoadAssetAtPath<MyAsset>("Assets/AATest/MyAsset.asset");
        if (asset == null)
        {
            Debug.LogError("asset is null");
        }else {
            Debug.LogError("asset is not null");
        }
    }
}
