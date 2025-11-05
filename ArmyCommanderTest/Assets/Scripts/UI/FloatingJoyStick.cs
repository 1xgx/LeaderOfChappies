using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FloatingJoyStick : MonoBehaviour
{
    [HideInInspector]
    public RectTransform RectTransform;
    public RectTransform Knob;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }
}
