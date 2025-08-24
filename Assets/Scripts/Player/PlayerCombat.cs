using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Animator _animator;

    [Header("Combo Settings")] 
    [SerializeField] private float _comboResetTime = 1f;

    private int _attackIndex;
    private float _lastAttackTime;

    private static readonly int AttackIndexHash = Animator.StringToHash("AttackIndex");

    public void Attack()
    {
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
    }
}
