using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public float BPM = 100;

    public float shakeIntensity = 1f;
    public float shakeTime = 1f;
    public float shakeIteration = 10f;

    private float timer;
    private float timerBeat;
    private float BPS => BPM / 60;

    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float baseXpos;
    private float baseYpos;
    private float baseZpos;

    private bool shakeCam;

    private void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        baseXpos = cinemachineVirtualCamera.transform.position.x;
        baseYpos = cinemachineVirtualCamera.transform.position.y;
        baseZpos = cinemachineVirtualCamera.transform.position.z;
    }
    private IEnumerator Shake()
    {
        for (int i = 0; i < shakeIteration; i++)
        {
            if (shakeCam)
            {
                yield return new WaitForSeconds(0.01f);
                float shakeX = Random.Range(-1, 1f);
                float shakeY = Random.Range(-1, 1f);
                cinemachineVirtualCamera.transform.position = new Vector3(baseXpos + shakeX * shakeIntensity, baseYpos + shakeY * shakeIntensity, baseZpos);
            }
        }
        timer = shakeTime;
    }

    private void StopShake()
    {
        cinemachineVirtualCamera.transform.position = new Vector3(baseXpos, baseYpos, baseZpos);
        shakeCam = false;
        timer = 0;
    }
    private void FixedUpdate()
    {
        timerBeat += Time.deltaTime;

        if (timerBeat >= 1/BPS)
        {
            timerBeat = 0;
            shakeCam = true;
            StartCoroutine(Shake());
        }
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer<= 0)
            {
                StopShake();
            }
        }
    }
}
