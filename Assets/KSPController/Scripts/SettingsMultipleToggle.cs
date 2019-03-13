using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMultipleToggle : MonoBehaviour {

    public string storageKey;
    public int value;
    public int defaultValue;
    public Text valueText;
    public Button button;
    public string[] names;

    void Start()
    {
        value = PlayerPrefs.GetInt("settings-" + storageKey, value);
        RefreshText();
        button.onClick.AddListener(Clicked);
    }

    void Update()
    {

    }

    public void SetValue(int value)
    {
        this.value = value;
        PlayerPrefs.SetInt("settings-" + storageKey, value);
        PlayerPrefs.Save();
        RefreshText();
    }

    public void RestoreDefault()
    {
        SetValue(defaultValue);
    }

    void Clicked()
    {
        value++;
        if (value >= names.Length) value = 0;
        PlayerPrefs.SetInt("settings-" + storageKey, value);
        PlayerPrefs.Save();
        RefreshText();
    }

    void RefreshText()
    {
        valueText.text = names[value];
    }
}
