using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour {

    public static float inputSmooth;
    public static float inputCurve;
    public static bool useSI;
    public static SettingsPanel instance;

    bool showing;
    public GameObject panel;
    public SettingsSlider inputSmoothSlider;
    public SettingsSlider inputCurveSlider;
    public SettingsToggle unitToggle;

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        showing = panel.activeInHierarchy;
	}
	
	void Update () {
        if (showing)
        {
            inputSmooth = inputSmoothSlider.value;
            inputCurve = inputCurveSlider.value;
            useSI = unitToggle.value;
        }
	}

    public void ShowPanel()
    {
        showing = true;
        panel.SetActive(true);
    }

    public void HidePanel()
    {
        showing = false;
        panel.SetActive(false);
    }

    public void RestoreDefault()
    {
        inputSmoothSlider.RestoreDefault();
        inputCurveSlider.RestoreDefault();
        unitToggle.RestoreDefault();
    }
}
