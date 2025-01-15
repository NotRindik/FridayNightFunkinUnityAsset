using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BootstrapManager : MonoBehaviour
{
    private static BootstrapManager _instance;
    public static BootstrapManager Instance => _instance;

    private Dictionary<System.Type, IBootstrap> bootstrapDictionary = new Dictionary<System.Type, IBootstrap>();

    private void Awake()
    {
        bootstrapDictionary.Clear();
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(this);
            return;
        }
        _instance = this;

        if (!Application.isPlaying)
        {
            InitializeEditorMode();
        }
        else
        {
            InitializePlayMode();
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private void InitializeEditorMode()
    {
        InitChildBoots();
    }

    private void InitializePlayMode()
    {
        InitChildBoots();
    }

    private void InitChildBoots()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            IBootstrap[] bootstraps = child.GetComponents<IBootstrap>();

            foreach (IBootstrap bootstrap in bootstraps)
            {
                bootstrap.Initialize();

                System.Type type = bootstrap.GetType();
                if (!bootstrapDictionary.ContainsKey(type))
                {
                    bootstrapDictionary.Add(type, bootstrap);
                }
                else
                {
                    Debug.LogWarning($"Duplicate bootstrap of type {type} found in {child.name}!");
                }
            }
        }
    }

    public T GetBootstrap<T>() where T : IBootstrap
    {
        if (bootstrapDictionary.TryGetValue(typeof(T), out IBootstrap bootstrap))
        {
            return (T)bootstrap;
        }
        Debug.LogWarning($"Bootstrap of type {typeof(T)} not found!");
        return default;
    }
}

public interface IBootstrap
{
    public void Initialize();
}