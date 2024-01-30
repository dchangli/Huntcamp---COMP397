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
        transform.Rotate(Vector3.up * (context.ReadValue<Vector2>().x * 1), Space.World);

        float input = context.ReadValue<Vector2>().y;
        float rotationAmount = input * -1;

        // Apply the rotation to the transform if needed
        _head.transform.Rotate(Vector3.left * rotationAmount, Space.Self);
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
