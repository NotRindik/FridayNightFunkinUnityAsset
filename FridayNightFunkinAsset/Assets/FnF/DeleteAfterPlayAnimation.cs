using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterPlayAnimation : MonoBehaviour
{
    public Animator animator;
    private float timeOfPlay;

    private void Start()
    {
        timeOfPlay = animator.GetCurrentAnimatorStateInfo(0).length;
    }
    private void FixedUpdate()
    {
        timeOfPlay -= Time.deltaTime;
    }
    private void Update()
    {
        if (timeOfPlay <= 0)
        {
            Destroy(gameObject);
        }
    }
}
