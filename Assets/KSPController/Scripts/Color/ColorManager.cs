using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {

    public static ColorManager instance;

    public Color buttonOn;
    public Color buttonOff;
    public Color buttonTouched;
    public Color buttonText;
    [Space]
    public Color joystickBg;
    public Color joystickHandle;
    public Color joystickInfo;
    public Color joystickCenterline;
    [Space]
    public Color sliderBack;
    public Color sliderHandle;
    public Color sliderHandleTouched;
    [Space]
    public Color panel;
    [Space]
    public Texture lockTex;
    public Texture unlockTex;

    public List<IChangeColor> coloredElements = new List<IChangeColor>();

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        UpdateColor();
	}
	
	void Update () {
		
	}

    public void UpdateColor()
    {
        foreach(var ele in coloredElements)
        {
            ele.UpdateColor();
        }
    }
}
