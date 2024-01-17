using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    [Header("Turret parts")]
    [SerializeField] private Transform _verticalRotator;
    [SerializeField] private Transform _horizontalRotator;
    [SerializeField] private Transform _rot;

    [Header("Params")]
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _minXAngle;
    [SerializeField] private float _maxXAngle;
    [SerializeField] private Vector3 _offset;

    private Transform _player;

    private float _xAngle;
    private float _yAngle;

    private const float Y_ROTATION_OFFSET = 90f;

    private void Start()
    {
		_player = Player.Instance.transform;
	}

    void Update()
    {
        Quaternion targetRot = Quaternion.LookRotation(_player.position - _rot.position + _offset);
        _rot.rotation = Quaternion.Slerp(_rot.rotation, targetRot, _rotationSpeed * Time.deltaTime);

        float x = _rot.eulerAngles.x;
        float y = _rot.eulerAngles.y + Y_ROTATION_OFFSET;

        _xAngle = Mathf.LerpAngle(_xAngle, x, _rotationSpeed * Time.deltaTime);
        _yAngle = Mathf.LerpAngle(_yAngle, y, _rotationSpeed * Time.deltaTime);

        _xAngle = Mathf.Clamp(_xAngle, _minXAngle, _maxXAngle);

        _horizontalRotator.transform.eulerAngles = new (0, _yAngle);
		_verticalRotator.transform.localEulerAngles = new (0, 0, _xAngle);
	}
}
