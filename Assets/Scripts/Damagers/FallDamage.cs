using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Health))]
public class FallDamage : MonoBehaviour
{
	[SerializeField, Min(0)] private float _damagePerVelocity;
	[SerializeField, Min(0)] private float _maxVelocityWithoutDamage;

	private PlayerMovement _playerMovement;
	private Health _health;

	private void Awake()
	{
		_playerMovement = GetComponent<PlayerMovement>();
		_health = GetComponent<Health>();
	}

	private void OnEnable()
	{
		_playerMovement.OnGetGrounded += OnGrounded;
	}

	private void OnDisable()
	{
		_playerMovement.OnGetGrounded -= OnGrounded;
	}

	private void OnGrounded(float velocity)
	{
		velocity = -velocity;
		if (velocity < _maxVelocityWithoutDamage)
			return;

		float damage = (velocity - _maxVelocityWithoutDamage) * _damagePerVelocity;
		_health.Hit(damage);
	}
}