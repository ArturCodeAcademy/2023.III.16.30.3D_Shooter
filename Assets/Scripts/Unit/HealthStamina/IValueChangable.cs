using System;

public interface IValueChangable
{
	float CurrentValue { get; }
	float MaxValue { get; }
	float MinValue { get; }

	event Action<float, float> OnValueChanged;
	event Action OnValueMin;
	event Action OnValueMax;
}
