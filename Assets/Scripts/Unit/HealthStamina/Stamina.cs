using System;
using UnityEngine;

public class Stamina : MonoBehaviour, IValueChangable
{
	[field: SerializeField, Min(1)]
	public float MaxValue { get; private set; }
	public float MinValue => 0;
	public float CurrentValue { get; private set; }

	[SerializeField, Min(0)] private float _staminaRegenPerSecond = 1f;

	public event Action<float, float> OnValueChanged;
	public event Action OnValueMin;
	public event Action OnValueMax;

	private bool _isRegenerating = true;

	private void Awake()
	{
		CurrentValue = MaxValue;
	}

	public float UseStamina(float stamina)
	{
		if (stamina <= 0)
			return 0;

		_isRegenerating = false;

		float appliedStamina = Mathf.Min(CurrentValue, stamina);
		CurrentValue -= appliedStamina;
		OnValueChanged?.Invoke(CurrentValue, MaxValue);

		if (CurrentValue <= 0)
			OnValueMin?.Invoke();

		return appliedStamina;
	}

	public float RegenStamina(float amount)
	{
		if (amount <= 0 || CurrentValue >= MaxValue)
			return 0;

		float appliedStamina = Mathf.Min(amount, MaxValue - CurrentValue);
		CurrentValue += appliedStamina;
		OnValueChanged?.Invoke(CurrentValue, MaxValue);

		if (CurrentValue >= MaxValue)
			OnValueMax?.Invoke();

		return appliedStamina;
	}

	private void LateUpdate()
	{
		if (_isRegenerating)
			RegenStamina(_staminaRegenPerSecond * Time.deltaTime);
		_isRegenerating = CurrentValue < MaxValue;
	}
}