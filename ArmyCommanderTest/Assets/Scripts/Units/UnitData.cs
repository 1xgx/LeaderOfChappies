using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "Scriptable Objects/UnitStats")]
public class UnitData : ScriptableObject
{
    public UnitCharacteristic Characteristic;
    public GameObject Bullet;
    public int MaxHealth = 100;
    public float RespawnDelay = 2f;
    public float UnitSpeed = 2;
    public float FireRate = 1.0f;
    public float Damage = 10;
    public float Range = 10;
}
