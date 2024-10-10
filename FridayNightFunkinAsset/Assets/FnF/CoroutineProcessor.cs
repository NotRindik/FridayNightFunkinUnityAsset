using System;
using System.Collections;
using System.Collections.Generic;

public class CoroutineProcessor
{
    private readonly List<IEnumerator> coroutines = new List<IEnumerator>();

    public void AddCoroutine(IEnumerator coroutine)
    {
        coroutines.Add(coroutine);
    }

    public void Update()
    {
        for (int i = coroutines.Count - 1; i >= 0; i--)
        {
            if (!coroutines[i].MoveNext())
            {
                coroutines.RemoveAt(i);
            }
        }
    }
}
