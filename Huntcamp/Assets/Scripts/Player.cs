using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Player Controller")]
    // New Input System instance
    private PlayerControls _inputs = null;
    private Rigidbody _rb;
    [SerializeField] private GameObject _head;

    [Header("Character Movement")]
    [SerializeField] 
    private float _speed;
    [SerializeField] 
    private float _jumpForce = 3.0f;
    private Vector2 _move; // Player performed movement

    private Quaternion _rotate;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        // Loads the player controls in a separated method
        LoadPlayerControls();
    }

    [SerializeField] private float _sensitivity;
    [SerializeField] private bool _invertVertical;
    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {

        // Updates the movement of the player
        UpdateMovmeent();
    }

    private void UpdateMovmeent()
    {
        Vector3 movement = new Vector3(_move.x, 0.0f, _move.y) * _speed * Time.fixedDeltaTime;
        transform.Translate(movement);
    }

    private void LoadPlayerControls()
    {
        // Initializes the controls and enables it
        _inputs = new PlayerControls();
        _inputs.Enable();

        // Subscribe events
        _inputs.Player.Movement.performed += OnMovementPerformed;
        _inputs.Player.Movement.canceled += (ctx) => _move = Vector2.zero;
        _inputs.Player.Jump.performed += OnJumpPerformed;
        _inputs.Player.Look.performed += OnLookPerformed;
        _inputs.UI.Pause.performed += (ctx) => GameManager.Instance.PauseGame();
    }
    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsGamePaused) return;
        transform.Rotate(Vector3.up * (context.ReadValue<Vector2>().x * _sensitivity), Space.World);

        Vector2 deltaRotation = new Vector2(0, context.ReadValue<Vector2>().y * _sensitivity);
        deltaRotation.y *= _invertVertical ? 1.0f : -1.0f;
        float pitchAngle = _head.transform.localEulerAngles.x;
        if (pitchAngle > 180) pitchAngle -= 360;
        pitchAngle = Mathf.Clamp(pitchAngle + deltaRotation.y, -90.0f, 90.0f);
        _head.transform.localRotation = Quaternion.Euler(pitchAngle, 0.0f, 0.0f);
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsGamePaused) return;
        Vector2 moveContext = context.ReadValue<Vector2>();
        _move = moveContext;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsGamePaused) return;
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
}
