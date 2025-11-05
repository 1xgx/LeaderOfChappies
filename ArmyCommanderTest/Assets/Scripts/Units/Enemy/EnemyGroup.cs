using System.Collections.Generic;
using UnityEngine;
using TriInspector;
using Unity.VisualScripting;

public class EnemyGroup : MonoBehaviour, ILookable
{
    [SerializeField]
    private List<Enemy> _enemies;

    private Transform _target;
    public bool isClear = false;
    private float _lastFire;
    private float _fireRate;
    private float _damage;
    private float _range;

    private void OnEnable()
    {
        foreach (Enemy enemy in _enemies)
        {
            enemy.Parent = this;
            _fireRate = enemy.Data.FireRate;
            _damage = enemy.Data.Damage;
            _range = enemy.Data.Range;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GetTarget(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_target != null)
        {
            LookAtUnit(_target);
            RangeAttack(_target);
        }
    }

    public void LookAtUnit(Transform target)
    {
        _target = target;
        foreach (Enemy enemy in _enemies)
        {
            Vector3 dir = target.position - enemy.transform.position;
            dir.y = 0;
            enemy.transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    private void GetTarget(Collider other)
    {
        if(_target != null)
        {
            return;
        }

        if (other.TryGetComponent<CounterEnemy>(out var CounterEnemy))
        {
            TypeOfUnit Type = CounterEnemy.Type;
            if (Type == TypeOfUnit.CounterEnemy)
            {
                LookAtUnit(CounterEnemy.transform);
            }
        }
        else if (other.TryGetComponent<Player>(out var Player))
        {
            TypeOfUnit Type = Player.Type;
            if (Type == TypeOfUnit.Player)
            {
                LookAtUnit(Player.transform);
            }
        }
    }

    private void RangeAttack(Transform target)
    {
        foreach(Enemy enemy in _enemies)
        {
            //enemy.Attack(_target);
        }

        if (Time.time >= _lastFire + _fireRate)
        {
            Debug.Log($"Attack {target.name}");
            if (target.TryGetComponent<IDamageable>(out var dmg))
            {
                dmg.TakeDamage(_damage);
            }

            _lastFire = Time.time;
        }
    }

    public void UnitDiedInGroup(Enemy enemy)
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if(_enemies[i] == enemy)
            {
                _enemies.RemoveAt(i);
            }
        }
    }
}
