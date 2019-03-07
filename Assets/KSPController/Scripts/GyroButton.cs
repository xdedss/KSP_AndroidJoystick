using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroButton : MonoBehaviour {

    public Button button;
    public Joystick target;
    public Color normalColor;
    public Color enabledColor;
    
	void Start () {
        button = GetComponent<Button>();
	}
	
	void Update () {
		
	}

    public void OnClick()
    {
        target.gyroOverride ^= true;
        var colors = button.colors;
        colors.normalColor = target.gyroOverride ? enabledColor : normalColor;
        button.colors = colors;
        Gyro.Calibrate();
    }
}
