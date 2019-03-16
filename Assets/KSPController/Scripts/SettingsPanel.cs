using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour {

    public static float inputSmooth;
    public static float inputCurve;
    public static int units;
    public static SettingsPanel instance;

    public float height;
    public float width;

    float targetT;
    float currentT;
    public GameObject panel;
    public RectTransform rectTransform;
    public SettingsSlider inputSmoothSlider;
    public SettingsSlider inputCurveSlider;
    public SettingsMultipleToggle unitToggle;

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        rectTransform = GetComponent<RectTransform>();
        width = Screen.width;
        height = Screen.height;
        float rate = width / 1920;
        if(rate < 1)
        {
            width /= rate;
            height /= rate;
        }
        rectTransform.sizeDelta = new Vector2(width, height);
        //showing = panel.activeInHierarchy;
	}
	
	void Update () {
        if (currentT != targetT)
        {
            float maxDelta = Mathf.Abs(targetT - currentT) * 10 * Time.deltaTime;
            float minDelta = 0.1f * Time.deltaTime;
            currentT = Mathf.MoveTowards(currentT, targetT, Mathf.Max(maxDelta, minDelta));
            rectTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(0, -height, currentT));
        }
        inputSmooth = inputSmoothSlider.value;
        inputCurve = inputCurveSlider.value;
        units = unitToggle.value;
    }

    public float ConvertInput(float input)
    {
        return input / (Mathf.Abs(input) * inputCurve + 1);
    }

    public string ConvertVel(float vel)
    {
        switch (units)
        {
            default:
            case 0:
                return vel.ToString("0.0") + "m/s";
            case 1:
                return (vel * 3.6).ToString("0.0") + "km/h";
            case 2:
                return (vel * 2.236936f).ToString("0.0" + "mph");
        }
    }

    public string ConvertAlt(float alt)
    {
        switch (units)
        {
            default:
            case 0:
            case 1:
                return alt.ToString("0.") + "m";
            case 2:
                return (alt * 3.28f).ToString("0.");
        }
    }

    public void ShowPanel()
    {
        targetT = 1;
        //panel.SetActive(true);
    }

    public void HidePanel()
    {
        targetT = 0;
        //panel.SetActive(false);
    }

    public void RestoreDefault()
    {
        inputSmoothSlider.RestoreDefault();
        inputCurveSlider.RestoreDefault();
        unitToggle.RestoreDefault();
    }
}
