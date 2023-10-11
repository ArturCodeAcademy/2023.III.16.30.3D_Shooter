using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField, Range(0, 180)] private float _verticalAngle;
    [SerializeField, Min(0.1f)] private float _turnSpeed;
    [SerializeField] private bool _verticalInversion;
    [SerializeField] private GameObject _virtualCamera;

    private float _xAxis;

    private void Update()
    {
		if (Time.timeScale == 0)
			return;

		Vector2 rawLookVector = new Vector2
			(
				Input.GetAxis("Mouse X"),
				Input.GetAxis("Mouse Y")
			);
		Vector2 lookVector = rawLookVector * _turnSpeed;
		_xAxis = Mathf.Clamp(_xAxis + (_verticalInversion ? lookVector.y : -lookVector.y),
			-_verticalAngle / 2, _verticalAngle / 2);
		transform.Rotate(Vector3.up * lookVector.x);

		_virtualCamera.transform.localRotation = Quaternion.Euler(Vector3.right * _xAxis);
	}
}
