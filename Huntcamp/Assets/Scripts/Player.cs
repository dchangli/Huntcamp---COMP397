using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Properties;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    // Set player as singleton
    public static Player Instance { get; private set; }

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
    [Header("Ground Detection")]
    [SerializeField] 
    Transform _groundCheck;
    [SerializeField] 
    float _groundRadius = 0.5f;
    [SerializeField] 
    LayerMask _groundMask;
    [SerializeField] 
    bool _isGrounded;
    private Vector2 _move; // Player performed movement
    private Vector2 _jump;
    private Vector2 _shoot;
    [SerializeField] 
    private float _sensitivity;
    [SerializeField] 
    private bool _invertVertical;
    [SerializeField] Joystick _moveJoystick;
    [SerializeField] Joystick _lookJoystick;
    [SerializeField] Button _jumpBtn;
    [SerializeField] Button _fireBtn;

    [Header("Player Death")]
    [SerializeField]
    private Transform _spawnPoint;
    public Action OnRespawn;
    public Action OnDeath;

    [Header("Player Health")]
    [SerializeField]
    private int _maxHealth = 4;
    public int _curHealth;
    [SerializeField]
    private Slider _healthSlider;

    [Header("Player Damage")]
    [SerializeField]
    private ParticleSystem _muzzleParticle;
    [SerializeField]
    private Transform _muzzlePosition;
    public Action<Enemy> OnDamage;
    [SerializeField]
    private GameObject _bulletObject;
    [SerializeField]
    private float _bulletSpeed = 10;
    private float _shootCooldown = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { return; };

        _rb = GetComponent<Rigidbody>();
        
        // Sets the health system
        _healthSlider.maxValue = _maxHealth;
        _curHealth = _maxHealth;

        // Loads the player controls in a separated method
        LoadPlayerControls();

        // Teleports player to the spawn point
        OnRespawn += OnPlayerRespawn;
        OnRespawn?.Invoke();
    }

    private void OnEnable()
    {
        _inputs.Enable();
        OnDeath += OnPlayerDeath;
        OnDamage += OnPlayerDamage;
    }

    private void OnDisable()
    {
        _inputs.Disable();
        OnRespawn -= OnPlayerRespawn;
        OnDeath -= OnPlayerDeath;
    }

    // Update is called once per frame
    void Update()
    {
        _shootCooldown -= Time.deltaTime;

        _healthSlider.value = _curHealth;
    }

    private void FixedUpdate()
    {
        _move = _moveJoystick.Direction;
        OnLookPerformed(_lookJoystick.Direction);
        //_jump = _jumpbtn;
        // Checks if the player is on the ground
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundRadius, _groundMask);

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

        // Subscribe events
        _inputs.Player.Movement.performed += OnMovementPerformed;
        _inputs.Player.Movement.canceled += (ctx) => _move = Vector2.zero;
        _inputs.Player.Jump.performed += (ctx) => OnJumpPerformed();
        _inputs.UI.Pause.performed += (ctx) => GameManager.Instance.PauseGame();
        _inputs.Player.Shoot.performed += (ctx) => OnPlayerShoot();

        _jumpBtn.onClick.AddListener(() => OnJumpPerformed());
        _fireBtn.onClick.AddListener(() => OnPlayerShoot());
    }
    private void OnLookPerformed(Vector2 look)
    {
        if (GameManager.Instance.IsGamePaused) return;
        transform.Rotate(Vector3.up * (look.x * _sensitivity), Space.World);

        Vector2 deltaRotation = new Vector2(0, look.y * _sensitivity);
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

    private void OnJumpPerformed()
    {
        if (GameManager.Instance.IsGamePaused || !_isGrounded) return;
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

    }
    
    private void OnPlayerRespawn()
    {
        GameSave gameSave = GameManager.Instance?.LoadGameSave();
        if (gameSave == null) this.transform.position = _spawnPoint.position;
        else
        {
            this.transform.position = gameSave.GetPlayerPosition();
            this._curHealth = gameSave.PlayerHealth;
        }
    }

    private void OnPlayerDeath()
    {
        DeathScreen.SetDeathScreen(true);
        _curHealth = _maxHealth;
        PlayerPrefs.SetString("GameData", "");
    }

    private void OnPlayerDamage(Enemy enemy)
    {
        _curHealth--;

        if (_curHealth <= 0) OnDeath.Invoke();
    }

    private void OnPlayerShoot()
    {
        if (_shootCooldown > 0) return;
        _muzzleParticle.Play();
        ShootBullet();
        _shootCooldown = 1;
    }

 
    private void ShootBullet()
    {
        GameObject bullet = Instantiate(_bulletObject, _muzzlePosition.position, _muzzlePosition.rotation);
        Vector3 shootDirection = _muzzlePosition.forward;
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(shootDirection * _bulletSpeed, ForceMode.Impulse);
    }
}
