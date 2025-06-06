using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ProjectAutoSetup
{
    private const string SetupKey = "MyFramework.ProjectSetupDone";

    static ProjectAutoSetup()
    {
        if (EditorPrefs.GetBool(SetupKey, false)) return;

        SetupLayers();
        SetupTags();
        // и другие настройки…

        EditorPrefs.SetBool(SetupKey, true);
        Debug.Log("✔ Проект автоматически настроен");
    }

    static void SetupLayers()
    {
        /*AddLayer("Arrows");
        AddLayer("Enemies");
        // другие слои*/
    }

    static void AddLayer(string layerName)
    {
        var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        var layersProp = tagManager.FindProperty("layers");

        for (int i = 8; i < layersProp.arraySize; i++)
        {
            var prop = layersProp.GetArrayElementAtIndex(i);
            if (prop.stringValue == layerName)
                return;

            if (string.IsNullOrEmpty(prop.stringValue))
            {
                prop.stringValue = layerName;
                tagManager.ApplyModifiedProperties();
                return;
            }
        }

        Debug.LogWarning("⚠ Нет свободных слотов для слоя: " + layerName);
    }

    static void SetupTags()
    {
        /*// Пример: добавление тега
        AddTag("Collectible");*/
    }

    static void AddTag(string tag)
    {
        var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        var tagsProp = tagManager.FindProperty("tags");

        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            if (tagsProp.GetArrayElementAtIndex(i).stringValue == tag)
                return;
        }

        tagsProp.InsertArrayElementAtIndex(0);
        tagsProp.GetArrayElementAtIndex(0).stringValue = tag;
        tagManager.ApplyModifiedProperties();
    }

    /*
    static void SetupPhysics()
    {
        // Пример: отключить столкновения между слоями
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Arrows"), LayerMask.NameToLayer("Enemies"), true);
    }
    */
    
}
