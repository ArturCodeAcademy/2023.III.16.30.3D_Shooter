using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class OnTurretEndHealth : MonoBehaviour
{
	[SerializeField] private List<GameObject> _detach = new();
	[SerializeField] private List<GameObject> _destroy = new();
	[SerializeField, Min(2)] private float _detachLifeTime = 15;
	[SerializeField, Min(0)] private float _explosionForce = 30;
	[SerializeField, Min(0)] private float _explosionRadius = 5;
	[SerializeField] private ParticleSystem _explosionPrefab;

	private Health _health;

	private void Awake()
	{
		_health = GetComponent<Health>();
	}

	private void OnEnable()
	{
		_health.OnValueMin += OnEndHealth;
	}

	private void OnDisable()
	{
		_health.OnValueMin -= OnEndHealth;
	}

	private void OnEndHealth()
	{
        foreach (var item in _detach)
        {
			if (item.TryGetComponent(out MeshCollider collider))
				collider.convex = true;

            if (!item.TryGetComponent(out Rigidbody rigidbody))
				rigidbody = item.AddComponent<Rigidbody>();

			rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
			item.transform.parent = null;
			Destroy(item, _detachLifeTime);
        }

		foreach (var item in _destroy)
			Destroy(item);

        Destroy(gameObject);
		Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
	}

#if UNITY_EDITOR

	private void Reset()
	{
		CheckChilds(transform);
	}

	private void CheckChilds(Transform parent)
	{
		foreach (Transform child in parent)
		{
			if (child.TryGetComponent<Collider>(out var _))
				_detach.Add(child.gameObject);
			else
				_destroy.Add(child.gameObject);
			
			CheckChilds(child);
		}
	}

#endif
}
