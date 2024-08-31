using System;
using System.Collections.Generic;
using UnityEngine;

public interface IService
{

}

public class ServiceLocator : MonoBehaviour
{
    private readonly Dictionary<string, object> _services = new Dictionary<string, object>();
    public static ServiceLocator instance { get; private set; }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

    public T Get<T>() where T : IService
    {
        string key = typeof(T).Name;

        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"{key} is not registered");
            throw new InvalidOperationException();
        }

        return (T) _services[key];
    }

    public void Register<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        if( _services.ContainsKey(key))
        {
            Debug.LogError("you alredy register It");
            return;
        }

        _services.Add(key, service);
    }

    public void UnRegister<T>() where T : IService
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError("This key is Empty, are u registered it before?");
            return;
        }

        _services.Remove(key);
    }
}
