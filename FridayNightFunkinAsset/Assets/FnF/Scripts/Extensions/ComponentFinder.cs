using FridayNightFunkin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentFinder
{
    public static T FindComponentAndCheckChilds<T>(GameObject objectForStartSearch) where T : Component
    {
        T result = null;
        if (objectForStartSearch.TryGetComponent(out T playAnimPerBeat))
        {
            result = playAnimPerBeat;
        }
        else
        {
            for (int i = 0; i < objectForStartSearch.transform.childCount; i++)
            {
                if (objectForStartSearch.transform.GetChild(i).TryGetComponent(out T playAnimPerBeatChild))
                {
                    result = playAnimPerBeatChild;
                }
                else
                {
                    Debug.LogWarning($"No playAnimPerBeat {objectForStartSearch}");
                }
            }
        }
        return result;
    }
}
