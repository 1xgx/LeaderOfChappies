using System.Collections.Generic;
using UnityEngine;

public class ArmyGroupedUp : MonoBehaviour
{
    private List<Transform> _army = new List<Transform>();
    private List<Transform> _enemyTransforms;


    public void addSoldier(CounterEnemy unit)
    {
        _army.Add(unit.transform);
    }

    public void SendArmyTransforms(List<Transform> transforms)
    {
        _enemyTransforms = transforms;
        SetArmyPosition();
    }

    private void SetArmyPosition()
    {
        transform.position = _enemyTransforms[0].position;
    }
}
