using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField, Min(1f)] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth;
    
    public float CurrentHealth => _currentHealth;
    
    // (current, max)
    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        if (_currentHealth <= 0f)
        {
            _currentHealth = _maxHealth;
        }

        RaiseHealthChanged();
    }

    [ContextMenu("Damage 10")]
    private void DebugDamage10() => ApplyDamage(10f);
    
    [ContextMenu("Heal 10")]
    private void DebugHeal10() => Heal(10f);
    
    public void ApplyDamage(float amount)
    {
        if (amount <= 0f) return;

        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0f, _maxHealth);
        RaiseHealthChanged();
    }
    
    public void Heal(float amount)
    {
        if (amount <= 0f) return;

        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0f, _maxHealth);
        RaiseHealthChanged();
    }
    
    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        RaiseHealthChanged();
    }
    
    private void RaiseHealthChanged()
    {
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
}
