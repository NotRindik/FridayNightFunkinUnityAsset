using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetValueFromCoroutines
{
    public Coroutine coroutine { get; private set; }

    public object result;

    private IEnumerator target;

    public GetValueFromCoroutines(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine("GetValueFromCoroutine");
    }

    private IEnumerator GetValueFromCoroutine()
    {
        while (target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
    }
}
