using System;
using UnityEngine;

public class Health : MonoBehaviour, IValueChangable
{
	[field: SerializeField, Min(1)]
	public float MaxValue { get; private set; }
	public float MinValue => 0;
	public float CurrentValue { get; private set; }

	public event Action<float, float> OnValueChanged;
	public event Action OnValueMin;
	public event Action OnValueMax;

	private void Awake()
	{
		CurrentValue = MaxValue;
	}

	public float Hit(float damage)
	{
		if (damage <= 0)
			return 0;

		float appliedDamage = Mathf.Min(CurrentValue, damage);
		CurrentValue -= appliedDamage;
		OnValueChanged?.Invoke(CurrentValue, MaxValue);

		if (CurrentValue <= 0)
			OnValueMin?.Invoke();

		return appliedDamage;
	}

	public float Heal(float amount)
	{
		if (amount <= 0)
			return 0;

		float appliedHeal = Mathf.Min(amount, MaxValue - CurrentValue);
		CurrentValue += appliedHeal;
		OnValueChanged?.Invoke(CurrentValue, MaxValue);

		if (CurrentValue <= 0)
			OnValueMax?.Invoke();

		return appliedHeal;
	}

	public float Kill()
	{
		float damage = CurrentValue;
		CurrentValue = MinValue;
		OnValueChanged?.Invoke(CurrentValue, MaxValue);
		OnValueMin?.Invoke();
		return damage;
	}
}
