using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitsDate", menuName = "Scriptable Objects/UnitsDate")]
public class UnitsData : ScriptableObject
{
    public List<UnitData> allUnits;

    public UnitData GetUnitByType(TypeOfUnit type)
    {
        return allUnits.Find(u => u.Characteristic.Type == type);
    }
}
