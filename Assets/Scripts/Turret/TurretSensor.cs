using UnityEngine;

[RequireComponent(typeof(Laser), typeof(RotateToPlayer), typeof(TurretShooting))]
public class TurretSensor : MonoBehaviour
{
	[SerializeField, Min(0)] private float _detectionRadius;

	private Laser _laser;
	private RotateToPlayer _rotateToPlayer;
	private TurretShooting _turretShooting;
	private LineRenderer _lineRenderer;

	private float _sqrDetectionRadius;

	private void Awake()
	{
		_laser = GetComponent<Laser>();
		_rotateToPlayer = GetComponent<RotateToPlayer>();
		_turretShooting = GetComponent<TurretShooting>();
		_lineRenderer = GetComponentInChildren<LineRenderer>();

		_sqrDetectionRadius = _detectionRadius * _detectionRadius;
	}

	private void FixedUpdate()
	{
		Vector3 distanceVector = Player.Instance.transform.position - transform.position;
		if (distanceVector.sqrMagnitude <= _sqrDetectionRadius)
			SetEnabled(true);
		else
			SetEnabled(false);
	}

	private void SetEnabled(bool enabled)
	{
		_rotateToPlayer.enabled = enabled;
		_turretShooting.enabled = enabled;
		_laser.enabled = enabled;
		_lineRenderer.enabled = enabled;
	}

#if UNITY_EDITOR

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _detectionRadius);
	}

#endif
}
