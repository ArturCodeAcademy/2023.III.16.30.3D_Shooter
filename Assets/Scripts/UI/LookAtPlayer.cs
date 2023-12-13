using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
	private Transform _playerCamera;

	private void Start()
	{
		_playerCamera = Camera.main.transform;
	}

	private void LateUpdate()
	{
		transform.forward = transform.position - _playerCamera.position;
	}
}
