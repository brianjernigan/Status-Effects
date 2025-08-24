using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Animator _animator;

    [Header("Colliders")] 
    [SerializeField] private CapsuleCollider2D _punchOneCollider;
    [SerializeField] private CapsuleCollider2D _punchTwoCollider;
    [SerializeField] private CapsuleCollider2D _punchThreeCollider;
    
    [Header("Combo Settings")] 
    [SerializeField] private float _comboResetTime = 1f;

    private int _attackIndex;
    private float _lastAttackTime;

    private static readonly int AttackIndexHash = Animator.StringToHash("AttackIndex");

    public void Attack()
    {
        if (!_animator.GetBool("IsGrounded")) return;
        
        if (Time.time - _lastAttackTime > _comboResetTime)
        {
            _attackIndex = 0;
        }
        
        _attackIndex = Mathf.Clamp(_attackIndex + 1, 1, 3);
        _lastAttackTime = Time.time;
        
        _animator.SetInteger(AttackIndexHash, _attackIndex);
    }

    public void ResetCombo()
    {
        _attackIndex = 0;
        _animator.SetInteger(AttackIndexHash, 0);
        
        DisableAllColliders();
    }

    public void StartPunchOneFrames()
    {
        ToggleCollider(_punchOneCollider, true);
        ToggleCollider(_punchTwoCollider, false);
        ToggleCollider(_punchThreeCollider, false);
    }

    public void EndPunchOneFrames()
    {
        ToggleCollider(_punchOneCollider, false);
    }

    public void StartPunchTwoFrames()
    {
        ToggleCollider(_punchTwoCollider, true);
        ToggleCollider(_punchOneCollider, false);
        ToggleCollider(_punchThreeCollider, false);
    }

    public void EndPunchTwoFrames()
    {
        ToggleCollider(_punchTwoCollider, false);
    }

    public void StartPunchThreeFrames()
    {
        ToggleCollider(_punchThreeCollider, true);
        ToggleCollider(_punchOneCollider, false);
        ToggleCollider(_punchTwoCollider, false);
    }

    public void EndPunchThreeFrames()
    {
        ToggleCollider(_punchThreeCollider, false);
    }

    private void ToggleCollider(CapsuleCollider2D capsule, bool state)
    {
        capsule.enabled = state;
    }

    private void DisableAllColliders()
    {
        _punchOneCollider.enabled = false;
        _punchTwoCollider.enabled = false;
        _punchThreeCollider.enabled = false;
    }
}
