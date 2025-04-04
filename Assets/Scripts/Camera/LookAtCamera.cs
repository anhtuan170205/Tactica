using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private bool invert;
    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }
    private void LateUpdate()
    {
        if (invert)
        {
            Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCamera * -1);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }
}
