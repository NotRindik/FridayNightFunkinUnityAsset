
using UnityEngine;

public abstract class OnGameStateChange : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
    }
    protected virtual void OnDisable()
    {
        GameStateManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }

    protected virtual void OnDestroy()
    {
        GameStateManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }
    protected virtual void OnGameStateChanged(GameState currenState)
    {
    }
} 