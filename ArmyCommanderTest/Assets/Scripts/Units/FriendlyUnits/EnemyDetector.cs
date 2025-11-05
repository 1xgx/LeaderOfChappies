using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField]
    private float alertRadius = 8f;
    [SerializeField]
    private float alertCooldown = 1f;
    [SerializeField]
    private CounterEnemy _counterEnemy;
    [SerializeField]
    private float _detectionRadius = 8.0f;

    [SerializeField]
    private Enemy _target;
    private float _lastAlertTime = 0.0f;

    private void Update()
    {
        CheckForEnemies();
    }

    private void CheckForEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius);
        if (hits.Length == 0) return;
        Debug.Log("Try Detect");
        Enemy nearest = null;
        float best = float.MaxValue;

        foreach (Collider other in hits)
        {
            float distance = Vector3.SqrMagnitude(other.transform.position - transform.position);
            if (distance < best)
            {
                best = distance;
                nearest = other.GetComponent<Enemy>();
                SetTarget(nearest);
            }
        }

        if (_target != null)
        {
            if (Time.time >= _lastAlertTime + alertCooldown)
            {
                _lastAlertTime = Time.time;
                AlertNearbyAllies(_target);
            }
        }
    }

    private void AlertNearbyAllies(Enemy enemy)
    {
        Collider[] allies = Physics.OverlapSphere(transform.position, alertRadius);
        foreach (var ally in allies)
        {
            if (ally.transform == transform) continue;

            if (ally.TryGetComponent<EnemyDetector>(out var allyAI))
            {
                allyAI.OnAllySpottedEnemy(enemy);
            }
        }
    }

    public void OnAllySpottedEnemy(Enemy enemy)
    {
        SetTarget(enemy);
    }

    private void SetTarget(Enemy enemy)
    {
        if(enemy != null)
        {
            _target = enemy;
            _counterEnemy.LookAtUnit(enemy.transform);
            Debug.Log($"Enemy: {enemy}");
        }
        
    }
}
