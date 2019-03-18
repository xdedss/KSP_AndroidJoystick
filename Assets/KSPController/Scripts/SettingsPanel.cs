using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour {

    public static float inputSmooth;
    public static float inputCurve;
    public static int units;
    public static bool useNavball;
    public static float roll2yaw;
    public static float roll2steer;
    public static float yaw2steer;
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
    public SettingsToggle navballToggle;
    public SettingsSlider Roll2YawSlider;
    public SettingsSlider Roll2SteerSlider;
    public SettingsSlider Yaw2SteerSlider;

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
        useNavball = navballToggle.value;
        roll2yaw = Roll2YawSlider.value;
        roll2steer = Roll2SteerSlider.value;
        yaw2steer = Yaw2SteerSlider.value;
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
        navballToggle.RestoreDefault();
        Roll2YawSlider.RestoreDefault();
        Roll2SteerSlider.RestoreDefault();
        Yaw2SteerSlider.RestoreDefault();
    }
}
