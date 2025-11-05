using System;
using TriInspector;
using UnityEngine;

[Serializable]
public class UnitCharacteristic
{
    [SerializeField] private TypeOfUnit _type;
    [SerializeField] private GameObject _drop;
    [SerializeField] private AudioClip _clip;
    public TypeOfUnit Type => _type;
    public GameObject Drop => _drop;
}
