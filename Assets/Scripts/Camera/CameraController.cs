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
    private const float smoothZoomSpeed = 5f;
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
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();
        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void RotateCamera()
    {
        Vector3 inputRotateDirection = new Vector3(0, 0, 0);
        inputRotateDirection.y = InputManager.Instance.GetCameraRotateAmount();
        transform.Rotate(inputRotateDirection * rotateSpeed * Time.deltaTime);
    }
    private void ZoomCamera()
    {
        targetFollowOffset.y -= InputManager.Instance.GetCameraZoomAmount() * zoomSpeed;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_OFFSET_Y, MAX_FOLLOW_OFFSET_Y);
        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, targetFollowOffset, smoothZoomSpeed * Time.deltaTime);
    }
}
