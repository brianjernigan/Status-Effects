using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Rigidbody2D _rb;
    
    private PlayerControls _controls;
    private Vector2 _moveInput;

    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void OnEnable()
    {
        _controls.Gameplay.Enable();
        _controls.Gameplay.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Gameplay.Move.canceled += _ => _moveInput = Vector2.zero;
        _controls.Gameplay.Jump.performed += _ => Jump();
    }

    private void OnDisable()
    {
        _controls.Gameplay.Disable();
    }

    private void FixedUpdate()
    {
        var velocity = new Vector2(_moveInput.x * _moveSpeed, _rb.linearVelocity.y);
        _rb.linearVelocity = velocity;
    }

    private void Jump()
    {
        Debug.Log("Jumping");
    }
}
