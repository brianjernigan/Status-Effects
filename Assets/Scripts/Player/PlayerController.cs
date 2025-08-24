using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Player Components")]
    [SerializeField] private PlayerAnimations _playerAnimations;
    [SerializeField] private PlayerCombat _playerCombat;
    [SerializeField] private PlayerHealth _playerHealth;
    
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Movement Settings")] 
    [SerializeField] private float _maxMoveSpeed = 7f;
    public float MaxMoveSpeed => _maxMoveSpeed;
    [SerializeField] private float _acceleration = 40f;
    [SerializeField] private float _deceleration = 70f;
    [SerializeField] private float _inputDeadzone = 0.05f;
    public float CurrentSpeed { get; private set; }

    [Header("Jump Settings")] 
    [SerializeField] private float _jumpForce = 6f;
    [SerializeField] private float _maxJumpTime = 0.125f;
    [SerializeField] private float _jumpCutMultiplier = 0.25f;

    private bool _isJumping;
    private float _jumpTimeCounter;

    [Header("Ground Check")] 
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    
    public bool IsGrounded { get; private set; }
    
    [Header("Attacks")]
    public bool IsAttacking { get; set; }
    
    private PlayerControls _controls;
    private Vector2 _rawMovementInput;
    private float _movementInput;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        ConfigureReferences();
    }

    private void ConfigureReferences()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        if (_sprite == null) _sprite = GetComponentInChildren<SpriteRenderer>();
        
        _controls = new PlayerControls();
    }

    private void OnEnable()
    {
        _controls.Gameplay.Enable();
        
        _controls.Gameplay.Move.performed += ctx => _rawMovementInput = ctx.ReadValue<Vector2>();
        _controls.Gameplay.Move.canceled += _ => _rawMovementInput = Vector2.zero;
        
        _controls.Gameplay.Jump.performed += _ => StartJump();
        _controls.Gameplay.Jump.canceled += _ => EndJump();

        _controls.Gameplay.Attack.performed += _ => Attack();
    }

    private void OnDisable()
    {
        _controls.Gameplay.Disable();
    }

    private void Update()
    {
        CaptureMovementInput();
    }
    
    private void FixedUpdate()
    {
        CheckGrounded();
        HandleMovement();
        _playerAnimations.UpdateLocomotion();
        HandleFacing();
        HandleJump();
    }

    private void CheckGrounded()
    {
        IsGrounded = Physics2D.OverlapCircle(_groundCheckPoint.position, _groundCheckRadius, _groundLayer);
    }

    private void CaptureMovementInput()
    {
        var x = _rawMovementInput.x;
        _movementInput = Mathf.Abs(x) < _inputDeadzone ? 0f : Mathf.Sign(x) * Mathf.Min(Mathf.Abs(x), 1f);
    }

    private void HandleMovement()
    {
        var targetSpeed = _movementInput * MaxMoveSpeed;

        CurrentSpeed = _movementInput != 0f ? 
            Mathf.MoveTowards(CurrentSpeed, targetSpeed, _acceleration * Time.fixedDeltaTime): 
            Mathf.MoveTowards(CurrentSpeed, 0f, _deceleration * Time.fixedDeltaTime);

        _rb.linearVelocity = new Vector2(CurrentSpeed, _rb.linearVelocity.y);
    }

    private void HandleFacing()
    {
        if (Mathf.Abs(_movementInput) <= 0.01f) return;

        var isFacingRight = _movementInput > 0f;
        var scale = transform.localScale;

        if ((!isFacingRight || !(scale.x < 0f)) && (isFacingRight || !(scale.x > 0f))) return;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    private void StartJump()
    {
        if (!IsGrounded) return;
        
        _isJumping = true;
        _jumpTimeCounter = _maxJumpTime;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
    }

    private void EndJump()
    {
        if (_rb.linearVelocity.y > 0f)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * _jumpCutMultiplier);
        }

        _isJumping = false;
    }

    private void HandleJump()
    {
        if (!_isJumping) return;
        
        if (_jumpTimeCounter > 0f)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
            _jumpTimeCounter -= Time.fixedDeltaTime;
        }
        else
        {
            _isJumping = false;
        }
    }

    private void Attack()
    {
        _playerCombat.Attack();
    }
}
