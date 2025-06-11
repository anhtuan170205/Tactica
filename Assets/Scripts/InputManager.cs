using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private PlayerControls playerControls;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    public Vector2 GetMouseScreenPosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public bool IsMouseButtonDown()
    {
        return Mouse.current.leftButton.isPressed;
    }

    public Vector2 GetCameraMoveVector()
    {
        return playerControls.Player.CameraMovement.ReadValue<Vector2>();
    }

    public float GetCameraRotateAmount()
    {
        return playerControls.Player.CameraRotation.ReadValue<float>();
    }

    public float GetCameraZoomAmount()
    {
        return playerControls.Player.CameraZoom.ReadValue<float>();
    }
}
