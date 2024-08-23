using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingFile : MonoBehaviour
{
    [SerializeField]private TextAsset fileName;

    private void Start()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        List<string> lines = FileManager.ReadTextAsset(fileName, false);

        foreach (string line in lines)
        {
            Debug.Log(line);
        }

        yield return null;
    }
}
