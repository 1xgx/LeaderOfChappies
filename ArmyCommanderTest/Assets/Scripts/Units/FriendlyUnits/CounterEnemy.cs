using TriInspector;
using UnityEngine;
using System.Collections.Generic;

public class CounterEnemy : MonoBehaviour, IAttackable, ILookable, IDamageable
{
    [HideInPlayMode]
    [SerializeField]
    private UnitData _unitData;

    private float _unitSpeed => _unitData.UnitSpeed;
    private Transform _position;
    private float _fireRate => _unitData.FireRate;
    private float _damage => _unitData.Damage;
    private float _range => _unitData.Range;
    private float _lastFire;

    public UnitData Data => _unitData;
    public TypeOfUnit Type => _unitData.Characteristic.Type;
    [SerializeField]
    private Transform _target;

    private List<EnemyGroup> _targetsGroups;
    private float _health => _unitData.MaxHealth;
    [SerializeField]
    private float _currentHealth;
    public Charge Parent;

    private void OnEnable()
    {
        _currentHealth = _health;
    }

    private void Update()
    {
        if(_target == null) Move();
        if (_target != null) Attack(_target);

        if(_targetsGroups != null)
        {
            if (_targetsGroups[0].isClear == true)
            {
                _targetsGroups.RemoveAt(0);
                GoToEnemyGroup();
            }
        }
    }

    private void Move()
    {
        if (_position != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _position.position, _unitSpeed * Time.deltaTime);
        }
    }

    public Transform SetTransform(Transform target)
    {
        return _position = target;
    }

    public void Attack(Transform target)
    {
        LookAtUnit(target);
        if (Time.time >= _lastFire + _fireRate)
        {
            Debug.Log("Attack");
            Ray ray = new Ray(transform.position, transform.forward);
            Debug.DrawRay(transform.position, transform.forward * _range, Color.red);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.TryGetComponent<IDamageable>(out var dmg))
                {
                    dmg.TakeDamage(_damage);
                }

            }
            _lastFire = Time.time;
        }
    }

    public void LookAtUnit(Transform target)
    {
        _target = target;
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir*100);
    }

    public void SetEnemiesGroupe(List<EnemyGroup> enemyGroups)
    {
        _targetsGroups = enemyGroups;
        GoToEnemyGroup();
    }

    private void GoToEnemyGroup()
    {
        _position = _targetsGroups[0].transform;
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        if(_currentHealth <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
