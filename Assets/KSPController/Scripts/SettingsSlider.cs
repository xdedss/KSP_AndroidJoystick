using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsSlider : MonoBehaviour {

    public string storageKey;
    public Text valueText;
    public Slider slider;
    public float value;
    public float defaultValue;
    
	void Start () {
        value = PlayerPrefs.GetFloat("settings-" + storageKey, defaultValue);
        slider.value = value;
        slider.onValueChanged.AddListener(ValueChanged);
        RefreshText();
    }
	
	void Update () {

	}

    public void SetValue(float value)
    {
        slider.value = value;
        ValueChanged(value);
        DragEnd();
    }

    public void RestoreDefault()
    {
        SetValue(defaultValue);
    }

    void ValueChanged(float value)
    {
        this.value = value;
        RefreshText();
    }

    void RefreshText()
    {
        valueText.text = value.ToString("0.00");
    }

    public void DragEnd()
    {
        PlayerPrefs.SetFloat("settings-" + storageKey, slider.value);
        PlayerPrefs.Save();
    }
}
