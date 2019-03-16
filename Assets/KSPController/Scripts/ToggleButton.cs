using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour, IChangeColor {

    public static Dictionary<int, ToggleButton> buttons = new Dictionary<int, ToggleButton>();
    
    public int index;
    public bool useHighlight;
    Button button;
    Text buttonText;
    public RawImage iconImage;

	void Start () {
        ColorManager.instance.coloredElements.Add(this);
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        buttons.Add(index, this);
        buttonText = transform.Find("Text").GetComponent<Text>();
	}

    void Update () {
		
	}

    public void SetOn(bool isOn)
    {
        if(ConnectionInitializer.instance.toggles[index] ^ isOn)
        {
            Toggle();
        }
    }

    public void OnClick()
    {
        Toggle();
    }

    public void Toggle()
    {
        ConnectionInitializer.instance.toggles[index] ^= true;
        UpdateColor();
    }

    public void UpdateColor()
    {
        var colors = button.colors;
        colors.normalColor = ConnectionInitializer.instance.toggles[index] && useHighlight ? ColorManager.instance.buttonOn : ColorManager.instance.buttonOff;
        colors.pressedColor = ColorManager.instance.buttonTouched;
        colors.highlightedColor = ColorManager.instance.buttonTouched;
        button.colors = colors;
        //Debug.Log(gameObject.name);
        buttonText.color = ColorManager.instance.buttonText;
        if (iconImage)
        {
            iconImage.color = ColorManager.instance.buttonText;
        }
    }
}
