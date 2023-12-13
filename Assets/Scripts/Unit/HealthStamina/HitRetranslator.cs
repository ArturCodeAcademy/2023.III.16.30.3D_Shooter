using UnityEngine;

public class HitRetranslator : MonoBehaviour, IHittable
{
    [SerializeField] private Health _health;
	[SerializeField] private float _damageMultiplier = 1;

	public float Hit(float damage)
	{
		return _health.Hit(damage * _damageMultiplier);
	}

#if UNITY_EDITOR

	private void Reset()
	{
		_health = GetComponentInParent<Health>();
	}

#endif

}
