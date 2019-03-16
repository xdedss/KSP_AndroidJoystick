using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredButton : MonoBehaviour, IChangeColor
{

    Button button;
    Text buttonText;

    void Start()
    {
        ColorManager.instance.coloredElements.Add(this);
        button = GetComponent<Button>();
        buttonText = transform.Find("Text").GetComponent<Text>();
    }

    void Update()
    {

    }

    public void UpdateColor()
    {
        var colors = button.colors;
        colors.normalColor = ColorManager.instance.buttonOff;
        colors.pressedColor = ColorManager.instance.buttonTouched;
        colors.highlightedColor = ColorManager.instance.buttonTouched;
        button.colors = colors;
        buttonText.color = ColorManager.instance.buttonText;
    }
}