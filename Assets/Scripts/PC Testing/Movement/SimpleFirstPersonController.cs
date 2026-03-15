using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SimpleFirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 0.1f;
    public float maxLookAngle = 80f;

    [Header("References")]
    public Transform cameraHolder;

    private CharacterController characterController;
    private float verticalVelocity;
    private float cameraPitch;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpPressed;
    private bool sprintHeld;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraHolder == null)
        {
            Debug.LogError("Camera Holder is not assigned.");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ReadInput();
        HandleMouseLook();
        HandleMovement();

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void ReadInput()
    {
        moveInput = Vector2.zero;
        lookInput = Vector2.zero;
        jumpPressed = false;
        sprintHeld = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) moveInput.y += 1f;
            if (Keyboard.current.sKey.isPressed) moveInput.y -= 1f;
            if (Keyboard.current.aKey.isPressed) moveInput.x -= 1f;
            if (Keyboard.current.dKey.isPressed) moveInput.x += 1f;

            jumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
            sprintHeld = Keyboard.current.leftShiftKey.isPressed;
        }

        if (Mouse.current != null)
        {
            lookInput = Mouse.current.delta.ReadValue();
        }

        moveInput = Vector2.ClampMagnitude(moveInput, 1f);
    }

    private void HandleMouseLook()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);

        cameraHolder.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
    }

    private void HandleMovement()
    {
        bool isGrounded = characterController.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        float currentSpeed = sprintHeld ? sprintSpeed : walkSpeed;

        characterController.Move(move * currentSpeed * Time.deltaTime);

        if (jumpPressed && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 verticalMove = new Vector3(0f, verticalVelocity, 0f);
        characterController.Move(verticalMove * Time.deltaTime);
    }
}