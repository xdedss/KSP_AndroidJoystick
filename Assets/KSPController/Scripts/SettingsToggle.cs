using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToggle : MonoBehaviour {


    public string storageKey;
    public bool value;
    public bool defaultValue;
    public Text valueText;
    public Button button;
    public string nameTrue;
    public string nameFalse;

	void Start () {
        value = PlayerPrefs.GetInt("settings-" + storageKey, value ? 1 : 0) == 1;
        RefreshText();
        button.onClick.AddListener(Clicked);
	}
	
	void Update () {
		
	}

    public void SetValue(bool value)
    {
        this.value = value;
        PlayerPrefs.SetInt("settings-" + storageKey, value ? 1 : 0);
        PlayerPrefs.Save();
        RefreshText();
    }

    public void RestoreDefault()
    {
        SetValue(defaultValue);
    }

    void Clicked()
    {
        value = !value;
        PlayerPrefs.SetInt("settings-" + storageKey, value ? 1 : 0);
        PlayerPrefs.Save();
        RefreshText();
    }

    void RefreshText()
    {
        valueText.text = value ? nameTrue : nameFalse;
    }
}
