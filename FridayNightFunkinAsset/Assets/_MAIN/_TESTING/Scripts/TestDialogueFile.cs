using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using VISUALNOVEL;

public class TestDialogueFile : MonoBehaviour
{
    [SerializeField] private TextAsset fileToRead = null;
    void Start()
    {
        StartConversation();
    }

    void StartConversation()
    {
        string fullPath = AssetDatabase.GetAssetPath(fileToRead);

        int resourcesIndex = fullPath.IndexOf("Resources/");
        string relativePath = fullPath.Substring(resourcesIndex + 10);

        string filePath = Path.ChangeExtension(relativePath, null);

        VNManager.instance.LoadFile(filePath);
    }
}