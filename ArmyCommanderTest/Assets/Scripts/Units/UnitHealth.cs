using TriInspector;
using UnityEngine;

public class UnitHealth : MonoBehaviour, IDamageable
{
    [HideInPlayMode]
    [Header("Player Health Settings")]
    [SerializeField]
    private UnitData _unitData;
    private float _maxHealth => _unitData.MaxHealth;
    private float _currentHealth = 0;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (_currentHealth <= 0) return;

        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} destroyed!");

        Destroy(gameObject);
    }

}
