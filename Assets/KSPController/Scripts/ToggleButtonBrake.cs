using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonBrake : MonoBehaviour, IChangeColor  {

    public static Dictionary<int, ToggleButtonBrake> buttons = new Dictionary<int, ToggleButtonBrake>();

    public int index;
    public bool useHighlight;
    [SerializeField]
    Button button;
    [SerializeField]
    Text buttonText;
    [SerializeField]
    Button lockButton;
    [SerializeField]
    RawImage lockIcon;

    bool locked;
    bool pressed;

    void Start ()
    {
        ColorManager.instance.coloredElements.Add(this);
        buttons.Add(index, this);
    }
	
	void Update () {

    }

    public void SetLocked(bool isOn)
    {
        if (locked ^ isOn)
        {
            ToggleLocked();
        }
    }

    public void ToggleLocked()
    {
        locked ^= true;
        UpdateOn();
        UpdateColor();
    }

    public void OnTouchDown()
    {
        pressed = true;
        UpdateOn();
        UpdateColor();
    }

    public void OnTouchUp()
    {
        pressed = false;
        UpdateOn();
        UpdateColor();
    }

    public void OnLockTouched()
    {
        ToggleLocked();
    }

    public void UpdateOn()
    {
        ConnectionInitializer.instance.toggles[index] = locked | pressed;
    }

    public void UpdateColor()
    {
        var colors = button.colors;
        colors.normalColor = ConnectionInitializer.instance.toggles[index] && useHighlight ? ColorManager.instance.buttonOn : ColorManager.instance.buttonOff;
        colors.pressedColor = colors.normalColor;
        colors.highlightedColor = colors.normalColor;
        button.colors = colors;
        buttonText.color = ColorManager.instance.buttonText;

        lockIcon.texture = locked ? ColorManager.instance.lockTex : ColorManager.instance.unlockTex;
        lockIcon.color = ColorManager.instance.buttonText;
        colors = lockButton.colors;
        colors.normalColor = locked && useHighlight ? ColorManager.instance.buttonOn : ColorManager.instance.buttonOff;
        colors.pressedColor = ColorManager.instance.buttonTouched;
        colors.highlightedColor = ColorManager.instance.buttonTouched;
        lockButton.colors = colors;
    }
}
