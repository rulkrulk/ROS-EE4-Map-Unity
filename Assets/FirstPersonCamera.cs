using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform player;
    public float mouseSensitivity = 0.15f;
    public float maxHorizontalAngle = 90f;

    [Header("Camera Sway")]
    [Range(0f, 1f)] public float swayAmount = 0.3f;
    public float swaySmooth = 8f;

    [Header("Idle Sway (Breathing)")]
    public bool enableIdleSway = true;
    public float idleSwaySpeed = 1f;
    public float idleSwayAmount = 0.5f;

    // Private variables
    private float cameraVerticalRotation = 0f;
    private float cameraHorizontalRotation = 0f;
    private Vector2 lookInput;
    private Camera cam;

    // Sway
    private Vector2 currentSway;
    private float idleSwayTimer = 0f;

    // NEW: cursor state
    private bool cursorLocked = true;

    void Awake()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            cam = gameObject.AddComponent<Camera>();
        }
    }

    void Start()
    {
        LockCursor(true);
    }

    void Update()
    {
        HandleCursorToggle();

        if (Mouse.current != null)
        {
            lookInput = Mouse.current.delta.ReadValue();
        }

        // Only rotate camera if cursor is locked
        if (cursorLocked)
        {
            HandleMouseLook();
            HandleCameraSway();
        }
    }

    void HandleCursorToggle()
    {
        if (Keyboard.current != null && Keyboard.current.vKey.wasPressedThisFrame)
        {
            cursorLocked = !cursorLocked;
            LockCursor(cursorLocked);
        }
    }

    void LockCursor(bool locked)
    {
        Cursor.visible = !locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void HandleMouseLook()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        // Vertical rotation
        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        // Horizontal rotation
        cameraHorizontalRotation += mouseX;
        cameraHorizontalRotation = Mathf.Clamp(cameraHorizontalRotation, -maxHorizontalAngle, maxHorizontalAngle);

        player.localRotation = Quaternion.Euler(0f, cameraHorizontalRotation, 0f);
    }

    void HandleCameraSway()
    {
        Vector2 targetSway = new Vector2(lookInput.x, lookInput.y) * swayAmount;

        currentSway = Vector2.Lerp(currentSway, targetSway, Time.deltaTime * swaySmooth);

        float idleX = 0f;
        float idleY = 0f;

        if (enableIdleSway)
        {
            idleSwayTimer += Time.deltaTime * idleSwaySpeed;
            idleX = Mathf.Sin(idleSwayTimer) * idleSwayAmount;
            idleY = Mathf.Cos(idleSwayTimer * 0.5f) * idleSwayAmount * 0.5f;
        }

        transform.localRotation = Quaternion.Euler(
            cameraVerticalRotation - currentSway.y + idleY,
            currentSway.x + idleX,
            -currentSway.x * 0.5f
        );
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && cursorLocked)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
