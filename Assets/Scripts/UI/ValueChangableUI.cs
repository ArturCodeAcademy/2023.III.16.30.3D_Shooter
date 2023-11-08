using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueChangableUI : MonoBehaviour
{
	[SerializeField] private MonoBehaviour _valueChangableObject;

	[Space(5)]
	[SerializeField] private Image _foreground;
	[SerializeField] private bool _useGradient;
	[SerializeField] private Gradient _gradient;
	[SerializeField] private TMP_Text _text;
	[SerializeField] private string _format = "{0:0} / {1:0}";

	private IValueChangable _valueChangable;

	private void Awake()
	{
		_valueChangable = _valueChangableObject as IValueChangable;
	}

	private void Start()
	{
		UpdateUI(_valueChangable.CurrentValue, _valueChangable.MaxValue);
	}

	private void OnEnable()
	{
		_valueChangable.OnValueChanged += UpdateUI;
	}

	private void OnDisable()
	{
		_valueChangable.OnValueChanged -= UpdateUI;
	}

	private void UpdateUI(float current, float max)
	{
		float fillAmount = current / max;
		_foreground.fillAmount = fillAmount;

		if (_useGradient)
			_foreground.color = _gradient.Evaluate(fillAmount);

		if (_text is not null)
			_text.text = string.Format(_format, current, max, fillAmount * 100);
	}

#if UNITY_EDITOR

	private void Reset()
	{
		_valueChangableObject = GetComponent<IValueChangable>() as MonoBehaviour;
	}

	private void OnValidate()
	{
		if (!_valueChangableObject.TryGetComponent<IValueChangable>(out var _))
			_valueChangableObject = null;
	}

#endif
}