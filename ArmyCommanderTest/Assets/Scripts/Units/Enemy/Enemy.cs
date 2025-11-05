using UnityEngine;
using TriInspector;

public class Enemy : MonoBehaviour, IAttackable, IDamageable
{
    [HideInPlayMode]
    [SerializeField]
    private UnitData _unitData;


    private float _fireRate => _unitData.FireRate;
    private float _damage => _unitData.Damage;
    private float _range => _unitData.Range;
    private float _lastFire;
    private float _health => _unitData.MaxHealth;
    private GameObject _drop => _unitData.Characteristic.Drop;

    private float _currentHealth;
    [HideInPlayMode]
    public EnemyGroup Parent;
    public UnitData Data => _unitData;

    private void OnEnable()
    {
        _currentHealth = _health;
    }

    //public void Attack(Transform target)
    //{
    //    if (Time.time >= _lastFire + _fireRate)
    //    {
    //        Debug.Log($"Attack {target.name}");
    //        Ray ray = new Ray(transform.position, transform.forward);
    //        Debug.DrawRay(transform.position, transform.forward * _range, Color.red);

    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            if (hit.collider.TryGetComponent<IDamageable>(out var dmg))
    //            {
    //                dmg.TakeDamage(_damage);
    //            }

    //        }
    //        _lastFire = Time.time;
    //    }
    //}
    public void Attack(Transform target) { }
    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0) DropGold();
    }

    private void DropGold()
    {
        GameObject newDrop = Instantiate(_drop, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) , Quaternion.identity);
        Parent.UnitDiedInGroup(this);
        Destroy(gameObject);
    }
}
