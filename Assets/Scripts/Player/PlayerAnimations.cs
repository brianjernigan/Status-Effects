using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private PlayerController _playerController;

    [Header("Attack Colliders")] 
    [SerializeField] private CapsuleCollider2D _punch01Collider;

    private static readonly int SpeedXAbsHash = Animator.StringToHash("SpeedXAbs");
    private static readonly int GroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int VerticalVelocityHash = Animator.StringToHash("VerticalVelocity");

    private void Awake()
    {
        if (_animator == null) _animator = GetComponent<Animator>();
        if (_rb == null) _rb = GetComponentInParent<Rigidbody2D>();
        if (_playerController == null) _playerController = GetComponentInParent<PlayerController>();
    }

    public void UpdateLocomotion()
    {
        var absX = Mathf.Abs(_rb.linearVelocity.x);
        var normalized = _playerController.MaxMoveSpeed > 0f 
            ? absX / _playerController.MaxMoveSpeed 
            : absX;

        _animator.SetFloat(SpeedXAbsHash, normalized);
        _animator.SetBool(GroundedHash, _playerController.IsGrounded);
        _animator.SetFloat(VerticalVelocityHash, _rb.linearVelocity.y);
    }
}
