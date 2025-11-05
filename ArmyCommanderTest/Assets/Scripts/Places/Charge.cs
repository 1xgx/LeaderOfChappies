using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charge : MonoBehaviour
{
    [SerializeField]
    private List<EnemyGroup> _enemyGroup;
    [SerializeField]
    private Image Button;
    private bool ArmyIsReady = false;
    private List<CounterEnemy> _counterEnemies = new List<CounterEnemy>();


    public void SendArmy()
    {
        if (ArmyIsReady)
        {
            Debug.Log("Army send");
            foreach (var counterEnemy in _counterEnemies)
            {
                counterEnemy.SetEnemiesGroupe(_enemyGroup);
                counterEnemy.Parent = this;
            }
        }
    }

    public void SetArmy(CounterEnemy army, bool isReady)
    {
        _counterEnemies.Add(army);
        ArmyIsReady = isReady;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out var Player))
        {
            Button.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out var Player))
        {
            Button.enabled = false;

        }
    }

    public void UnitDiedInGroup(CounterEnemy counterEnemy)
    {
        for (int i = 0; i < _counterEnemies.Count; i++)
        {
            if (_counterEnemies[i] == counterEnemy)
            {
                _counterEnemies.RemoveAt(i);
            }
        }

        if(_counterEnemies.Count <= 0)
        {
            LevelManager.instance.ResetArmy();
        }
    }
}
