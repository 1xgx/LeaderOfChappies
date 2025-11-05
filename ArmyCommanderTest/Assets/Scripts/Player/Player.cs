using TriInspector;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [HideInPlayMode]
    [Header("Player Health Settings")]
    [SerializeField]
    private UnitData _unitData;
    private float _maxHealth => _unitData.MaxHealth;
    private float _currentHealth = 0;
    private float _respawnDelay => _unitData.RespawnDelay;

    public UnitData Data => _unitData;
    public TypeOfUnit Type => _unitData.Characteristic.Type;

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (_currentHealth <= 0) return;

        _currentHealth -= amount;
        _currentHealth = Mathf.Max(_currentHealth, 0);

        if (_currentHealth == 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Game Over");
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    }
}
