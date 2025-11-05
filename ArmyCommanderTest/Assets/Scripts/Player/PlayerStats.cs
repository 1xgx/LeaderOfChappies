using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private UnitData _unitData;

    public UnitData Data => _unitData;
}
