using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class FNFDependencyInstaller
{
    [MenuItem("FNFMaker/Download Dependencies")]
    public static void InstallDeps()
    {
        string manifestPath = Path.Combine(Application.dataPath, "../Packages/manifest.json");
        string json = File.ReadAllText(manifestPath);
        JObject manifest = JObject.Parse(json);

        var deps = manifest["dependencies"] as JObject;

        Dictionary<string, string> requiredDeps = new()
        {
            { "com.unity.timeline", "1.7.6" },
            { "com.unity.inputsystem", "1.7.0" },
            { "com.unity.render-pipelines.universal", "14.0.10" },
            { "com.unity.cinemachine", "2.9.7" }
        };

        bool changed = false;

        foreach (var kvp in requiredDeps)
        {
            if (deps[kvp.Key] == null)
            {
                deps[kvp.Key] = kvp.Value;
                changed = true;
                Debug.Log($"✅ Добавлена зависимость: {kvp.Key} ({kvp.Value})");
            }
        }

        if (changed)
        {
            File.WriteAllText(manifestPath, manifest.ToString());
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Готово", "Зависимости успешно установлены!", "Ок");
        }
        else
        {
            EditorUtility.DisplayDialog("Ничего не изменилось", "Все зависимости уже установлены", "Ок");
        }
    }
}