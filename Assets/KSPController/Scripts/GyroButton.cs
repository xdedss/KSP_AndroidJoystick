using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroButton : MonoBehaviour, IChangeColor {

    public Button button;
    public Text buttonText;
    public Joystick target;
    public Color normalColor;
    public Color enabledColor;
    
	void Start ()
    {
        ColorManager.instance.coloredElements.Add(this);
        button = GetComponent<Button>();
        buttonText = transform.GetChild(0).GetComponent<Text>();
	}
	
	void Update () {
		
	}

    public void OnClick()
    {
        target.gyroOverride ^= true;
        //var colors = button.colors;
        //colors.normalColor = target.gyroOverride ? enabledColor : normalColor;
        //button.colors = colors;
        UpdateColor();
        Gyro.Calibrate();
    }

    public void UpdateColor()
    {
        var colors = button.colors;
        colors.normalColor = target.gyroOverride ? ColorManager.instance.buttonOn : ColorManager.instance.buttonOff;
        colors.pressedColor = ColorManager.instance.buttonTouched;
        colors.highlightedColor = ColorManager.instance.buttonTouched;
        button.colors = colors;
        buttonText.color = ColorManager.instance.buttonText;
    }
}
