using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCrouching : MonoBehaviour
{
	public bool IsCrouched { get; private set; }

	[SerializeField, Range(0.1f, 5f)] private float _crouchSpeed;
	[SerializeField, Min(0)] private float _crouchHeight;
	[SerializeField, Min(0)] private float _standHeight;

	[Space(3)]
	[SerializeField] private GameObject _virtualCamera;
	[SerializeField, Min(0)] private float _verticalCameraOffset;

	private CharacterController _characterController;
	private Coroutine _crouchCoroutine;

	private void Awake()
	{
		_characterController = GetComponent<CharacterController>();
	}

	private void Start()
	{
		_characterController.height = _standHeight;
		_virtualCamera.transform.localPosition =
			Vector3.up * (_characterController.height - _verticalCameraOffset);
		IsCrouched = false;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.LeftControl) && _characterController.height != _crouchHeight)
		{
			if (_crouchCoroutine != null)
				StopCoroutine(_crouchCoroutine);
			_crouchCoroutine = StartCoroutine(ChangeHeight(true));
		}

		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			if (_crouchCoroutine != null)
				StopCoroutine(_crouchCoroutine);
			_crouchCoroutine = StartCoroutine(ChangeHeight(false));
		}
	}

	private bool CanMoveUp()
	{
		Vector3 rayStartPos = transform.position + Vector3.up *
			(_characterController.height - _characterController.radius);
		RaycastHit[] hits = Physics.SphereCastAll(rayStartPos, _characterController.radius, Vector3.up);
		return hits.Length == 0 || hits.Length == 1 && hits[0].transform == transform;
	}

	private IEnumerator ChangeHeight(bool moveDown)
	{
		IsCrouched = true;
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		while (moveDown
			? _characterController.height > _crouchHeight
			: _characterController.height < _standHeight)
		{
			if (!moveDown && !CanMoveUp())
			{
				yield return wait;
				continue;
			}

			_characterController.height += (moveDown ? -1 : 1) * _crouchSpeed * Time.deltaTime;
			_characterController.center = Vector3.up * _characterController.height / 2;
			_virtualCamera.transform.localPosition =
				Vector3.up * (_characterController.height - _verticalCameraOffset);
			yield return wait;
		}

		_characterController.height = moveDown ? _crouchHeight : _standHeight;
		_characterController.center = Vector3.up * _characterController.height / 2;
		_virtualCamera.transform.localPosition =
			Vector3.up * (_characterController.height - _verticalCameraOffset);
		IsCrouched = moveDown;
	}

#if UNITY_EDITOR

	private void Reset()
	{
		_characterController = GetComponent<CharacterController>();
		_standHeight = _characterController.height;
		_crouchHeight = _standHeight / 2;
	}

	private void OnValidate()
	{
		_characterController = GetComponent<CharacterController>();
		_characterController.height = _standHeight;
		_characterController.center = Vector3.up * _characterController.height / 2;

		if (_crouchHeight > _standHeight)
			_crouchHeight = _standHeight;

		if (_verticalCameraOffset > _standHeight)
			_verticalCameraOffset = _standHeight;

		if (_virtualCamera is not null)
		{
			_virtualCamera.transform.localPosition =
				Vector3.up * (_characterController.height - _verticalCameraOffset);
		}
	}

#endif
}