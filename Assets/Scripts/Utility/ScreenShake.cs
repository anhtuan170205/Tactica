using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }
    private CinemachineImpulseSource impulseSource;
    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShakeCamera(float intensity = 1f)
    {
        impulseSource.GenerateImpulse(intensity);
    }

}
