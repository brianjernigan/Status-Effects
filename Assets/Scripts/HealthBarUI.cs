using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private CharacterStats _target;
    [SerializeField] private Image _fillImage;
    [SerializeField] private TextMeshProUGUI _healthText;

    private void OnEnable()
    {
        _target.OnHealthChanged += UpdateHealth;
    }

    private void OnDisable()
    {
        _target.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(float current, float max)
    {
        _fillImage.fillAmount = current / max;
        _healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }
}
