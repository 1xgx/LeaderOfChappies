using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
public class PlayerTouchControl : MonoBehaviour
{
    [SerializeField]
    private Vector2 _joystickSize = new Vector2(300, 300);
    [SerializeField]
    private FloatingJoyStick _joyStick;
    [SerializeField] private Player _playerStats;
    private float _speed => _playerStats.Data.UnitSpeed;
    [SerializeField]
    private NavMeshAgent _player;
 
    private Finger _movementFinger;
    private Vector2 _movementAmount;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        _joyStick.gameObject.SetActive(true);
        _joyStick.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
    }

    private void HandleFingerMove(Finger MovedFinger)
    {
        if (MovedFinger == _movementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = _joystickSize.x / 2f;
            ETouch.Touch currentTouch = MovedFinger.currentTouch;

            if (Vector2.Distance(
                    currentTouch.screenPosition,
                    _joyStick.RectTransform.anchoredPosition
                ) > maxMovement)
            {
                knobPosition = (
                    currentTouch.screenPosition - _joyStick.RectTransform.anchoredPosition
                    ).normalized
                    * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - _joyStick.RectTransform.anchoredPosition;
            }

            _joyStick.Knob.anchoredPosition = knobPosition;
            _movementAmount = knobPosition / maxMovement;
        }
    }
    private void HandleLoseFinger(Finger LostFinger)
    {
        if (LostFinger == _movementFinger)
        {
            _movementFinger = null;
            _joyStick.Knob.anchoredPosition = Vector2.zero;
            _joyStick.gameObject.SetActive(false);
            _movementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        if (_movementFinger == null && TouchedFinger.screenPosition.x <= Screen.width)
        {
            _movementFinger = TouchedFinger;
            _movementAmount = Vector2.zero;
            _joyStick.gameObject.SetActive(true);
            _joyStick.RectTransform.sizeDelta = _joystickSize;
            _joyStick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 StartPosition)
    {
        if (StartPosition.x < _joystickSize.x / 2)
        {
            StartPosition.x = _joystickSize.x / 2;
        }

        if (StartPosition.y < _joystickSize.y / 2)
        {
            StartPosition.y = _joystickSize.y / 2;
        }
        else if (StartPosition.y > Screen.height - _joystickSize.y / 2)
        {
            StartPosition.y = Screen.height - _joystickSize.y / 2;
        }

        return StartPosition;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 scaledMovement = _speed * Time.deltaTime * new Vector3(
            _movementAmount.x,
            0,
            _movementAmount.y
        );
        if (_player != null && _player.isActiveAndEnabled && _player.isOnNavMesh)
        {
            transform.LookAt(_player.transform.position + scaledMovement, Vector3.up);
            _player.Move(scaledMovement);
        }
    }

    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle()
        {
            fontSize = 24,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };
        if (_movementFinger != null)
        {
            GUI.Label(new Rect(10, 35, 500, 20), $"Finger Start Position: {_movementFinger.currentTouch.startScreenPosition}", labelStyle);
            GUI.Label(new Rect(10, 65, 500, 20), $"Finger Current Position: {_movementFinger.currentTouch.screenPosition}", labelStyle);
        }
        else
        {
            GUI.Label(new Rect(10, 35, 500, 20), "No Current Movement Touch", labelStyle);
        }

        GUI.Label(new Rect(10, 10, 500, 20), $"Screen Size ({Screen.width}, {Screen.height})", labelStyle);
    }

}
