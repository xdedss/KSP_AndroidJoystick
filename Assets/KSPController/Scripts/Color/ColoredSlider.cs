using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredSlider : MonoBehaviour, IChangeColor {

    Image bg;
    Slider slider;

    public void UpdateColor()
    {
        var cm = ColorManager.instance;
        bg.color = cm.sliderBack;
        var colors = slider.colors;
        colors.normalColor = cm.sliderHandle;
        colors.highlightedColor = cm.sliderHandleTouched;
        colors.pressedColor = cm.sliderHandleTouched;
        slider.colors = colors;
    }
    
    void Start () {
        ColorManager.instance.coloredElements.Add(this);
        bg = transform.Find("Background").GetComponent<Image>();
        slider = GetComponent<Slider>();
	}
	
	void Update () {
		
	}
}
