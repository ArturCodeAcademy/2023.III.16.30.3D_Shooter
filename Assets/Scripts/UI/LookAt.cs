using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform _target;

	private void LateUpdate()
	{
		transform.forward = transform.position - _target.position;
	}
}
