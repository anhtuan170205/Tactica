using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    private const float MIN_FOLLOW_OFFSET_Y = 2f;
    private const float MAX_FOLLOW_OFFSET_Y = 12f;
    private const float moveSpeed = 5f;
    private const float rotateSpeed = 100f;
    private const float zoomSpeed = 5f;
    private Vector3 targetFollowOffset;
    private CinemachineTransposer transposer;

    private void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = transposer.m_FollowOffset;
    }

    private void Update()
    {
        MoveCamera();
        RotateCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {
        Vector3 inputMoveDirection = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = 1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.z = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.z = -1f;
        }
        Vector3 moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void RotateCamera()
    {
        Vector3 inputRotateDirection = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q))
        {
            inputRotateDirection.y = 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            inputRotateDirection.y = -1f;
        }
        transform.Rotate(inputRotateDirection * rotateSpeed * Time.deltaTime);
    }
    private void ZoomCamera()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomSpeed; 
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomSpeed;
        }
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_OFFSET_Y, MAX_FOLLOW_OFFSET_Y);
        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, targetFollowOffset, zoomSpeed * Time.deltaTime);
    }
}
