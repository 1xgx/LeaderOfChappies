using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    [Header ("🎥")]
    [SerializeField]
    private Transform _camera;
    [SerializeField]
    private Vector3 _offset;

    private void Update()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        Vector3 Direction = transform.position;
        _camera.transform.position = new Vector3(Direction.x + _offset.x, Direction.y + _offset.y, Direction.z + _offset.z);
    }
}
