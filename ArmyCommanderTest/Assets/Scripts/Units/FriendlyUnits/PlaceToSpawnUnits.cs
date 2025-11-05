using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

public class PlaceToSpawnUnits : MonoBehaviour
{
    [SerializeField] private PositionSpawner _pointsSpawner;
    [SerializeField] private CounterEnemy _unit;
    [SerializeField] private int _maxUnits;
    [SerializeField] private float _spawnDelay = 1.0f;

    private int _startingDelay = 500;
    private List<Transform> _freePoints;
    private List<CounterEnemy> _counterEnemies = new List<CounterEnemy>();

    private void OnEnable()
    {
        DelayBeforeSpawn().Forget();
    }

    private async UniTask DelayBeforeSpawn()
    {
        await UniTask.Delay(_startingDelay);
        _freePoints = _pointsSpawner.TryGetActivePoints(_maxUnits);
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        int units = _maxUnits;

        for (int i = 0; i < units; i++)
        {
            yield return new WaitForSeconds(_spawnDelay);
            CounterEnemy newUnit = Instantiate(_unit, transform.position, Quaternion.identity);
            newUnit.SetTransform(_freePoints[i]);
            _pointsSpawner.SetArmyToCharge(newUnit, _maxUnits, i);

        }

        StopCoroutine(SpawnCoroutine());
        
    }
    public void ResetArmy()
    {
        DelayBeforeSpawn().Forget();
    }
}
