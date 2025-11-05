using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using TriInspector;

public class PositionSpawner : MonoBehaviour
{
    private FormationBase _formation;
    public FormationBase Formation
    {
        get
        {
            if (_formation == null) _formation = GetComponent<FormationBase>();
            return _formation;
        }
        set => _formation = value;
    }

    [Header ("👨‍✈️")]
    [SerializeField] 
    private Point _postionPrefab;
    [HideInPlayMode]
    [Tooltip ("Without this place, you can't send your army to fight")]
    private readonly List<Point> _spawnedPoints = new List<Point>();
    [ShowInPlayMode]
    [SerializeField]
    private List<Point> _activePoints = new List<Point>();
    [SerializeField]
    private Charge charge;
    private ArmyGroupedUp _army;
    private List<Vector3> _points = new List<Vector3>();
    private Transform _parent;

    private void Start()
    {
        _parent = new GameObject("Unit Parent").transform;
    }

    private void Update()
    {
        SetFormation();
    }

    private void SetFormation()
    {
        _points = Formation.EvaluatePoints().ToList();

        if (_points.Count > _spawnedPoints.Count)
        {
            IEnumerable<Vector3> remainingPoints = _points.Skip(_spawnedPoints.Count);
            Spawn(remainingPoints);
        }
        else if (_points.Count < _spawnedPoints.Count)
        {
            Kill(_spawnedPoints.Count - _points.Count);
        }

        for (int i = 0; i < _spawnedPoints.Count; i++)
        {
            Vector3 position = new Vector3(transform.position.x + _points[i].x,
                transform.position.y + _points[i].y,
                transform.position.z + _points[i].z);
            _spawnedPoints[i].transform.position = position;
        }
    }

    private void Spawn(IEnumerable<Vector3> points)
    {
        _army = new GameObject("ArmyGroupedUp").AddComponent<ArmyGroupedUp>();
        foreach (Vector3 pos in points)
        {
            Point unit = Instantiate(_postionPrefab, transform.position + pos, Quaternion.identity, _parent);
            _spawnedPoints.Add(unit);
        }
    }

    public List<Transform> TryGetActivePoints(int value)
    {
        int ActivePointsCount = _activePoints.Count + value;
        List<Transform> FreePoints = new List<Transform>();

        if (value > _spawnedPoints.Count)
        {
            Debug.LogError($"_value:{value} more than _spawnedPoinst{_spawnedPoints}.");
            return null;
        }

        for (int i = 0; i < ActivePointsCount; i++)
        {

            if (_spawnedPoints[i].Active.activeInHierarchy == false)
            {
                _activePoints.Add(_spawnedPoints[i]);
                _activePoints[i].Active.SetActive(true);
                _activePoints[i].UnActive.SetActive(false);
                FreePoints.Add(_activePoints[i].transform);
            }
        }

        return FreePoints;
    }

    public void SetArmyToCharge(CounterEnemy unit, int MaxUnits, int Units)
    {

        if (MaxUnits == Units + 1)
        {
            charge.SetArmy(unit, true);
        }
        else
        {
            Debug.Log($"army {unit}");
            charge.SetArmy(unit, false);
        }
    }

    private void Kill(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var unit = _spawnedPoints.Last();
            _spawnedPoints.Remove(unit);
            Destroy(unit.gameObject);
        }
    }
}
