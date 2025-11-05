using UnityEngine;
using TriInspector;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    [SerializeField]
    private List<PlaceToSpawnUnits> _placeToSpawnUnits;

    private void Start()
    {
        instance = this;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ResetArmy()
    {
        foreach(var _army in _placeToSpawnUnits)
        {
            _army.ResetArmy();
        }
    }
}
