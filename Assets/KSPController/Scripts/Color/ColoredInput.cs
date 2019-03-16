using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredInput : MonoBehaviour, IChangeColor {

    InputField field;
    Text text;
    Text hint;
    
	void Start () {
        ColorManager.instance.coloredElements.Add(this);
        field = GetComponent<InputField>();
        hint = transform.Find("Placeholder").GetComponent<Text>();
        text = transform.Find("Text").GetComponent<Text>();
	}
	
	void Update () {
		
	}

    public void UpdateColor()
    {
        var colors = field.colors;
        colors.normalColor = ColorManager.instance.buttonOff;
        colors.highlightedColor = ColorManager.instance.buttonTouched;
        colors.pressedColor = ColorManager.instance.buttonTouched;
        field.colors = colors;
        text.color = ColorManager.instance.buttonText;
        var hintColor = ColorManager.instance.buttonText;
        hintColor.a *= 0.4f;
        hint.color = hintColor;
    }
}
