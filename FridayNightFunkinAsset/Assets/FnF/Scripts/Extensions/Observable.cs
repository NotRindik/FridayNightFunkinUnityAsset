using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Observable<T>
{
    [SerializeField]
    private List<MonoBehaviour> listeners = new();

    public void Invoke(T data)
    {
        CollectGarbage();
        foreach (var listener in listeners)
        {
            if (listener is IObserver<T> typedListener)
            {
                typedListener.OnInvoke(data);
            }
        }
    }

    public void CollectGarbage()
    {
        listeners.RemoveAll(listener => listener == null);
    }

    public bool ContainsListener(MonoBehaviour mono)
    {
        return listeners.Contains(mono);
    }

    public void AddListener(MonoBehaviour mono)
    {
        if (!listeners.Contains(mono))
            listeners.Add(mono);
    }

    public void RemoveListener(MonoBehaviour mono)
    {
        listeners.Remove(mono);
    }
}
public interface IObserver { }

public interface IObserver<T> : IObserver
{
    void OnInvoke(T data);
}
