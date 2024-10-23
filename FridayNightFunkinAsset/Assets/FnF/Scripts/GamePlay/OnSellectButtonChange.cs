using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnSellectButtonChange : MonoBehaviour
{
    public UnityEvent OnButtonChange;

    private GameObject lastSelectedGameObject;

    private void Update()
    {
        if(lastSelectedGameObject != EventSystem.current.currentSelectedGameObject && !Input.GetKeyDown(KeyCode.Return))
        {
            lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            OnButtonChange?.Invoke();
        }
    }
}
