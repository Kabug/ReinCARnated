using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
	public Slider slider;
	public Gradient gradient;
	public Image fill;
	private float _CurrentHealth;
	private float _MaxHealth;


	[SerializeField]
	private TextMeshProUGUI valueText;

	private void Update()
	{
		valueText.text = $"{_CurrentHealth}/{_MaxHealth}";
	}

	public void SetMaxHealth(float health)
	{
		slider.maxValue = health;
		slider.value = health;
		_MaxHealth = health;
		fill.color = gradient.Evaluate(1f);
	}

	public void SetCurrentHealth(float health)
	{
		slider.value = health;
		_CurrentHealth = health;
		fill.color = gradient.Evaluate(slider.normalizedValue);
	}
}
