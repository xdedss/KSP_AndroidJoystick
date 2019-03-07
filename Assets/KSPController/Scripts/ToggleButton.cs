using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour {

    public static Dictionary<int, ToggleButton> buttons = new Dictionary<int, ToggleButton>();

    //public bool on
    //{
    //    get
    //    {
    //        return on_;
    //    }
    //    set
    //    {
    //        SetOn(value);
    //    }
    //}
    //bool on_ = false;
    public int index;
    public Color colorOn = new Color(0.5f, 1, 1);
    public Color colorOff = new Color(0.8f, 0.8f, 0.8f);
    Button button;

	void Start () {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        buttons.Add(index, this);
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

    void OnClick()
    {
        Toggle();
    }

    public void Toggle()
    {
        ConnectionInitializer.instance.toggles[index] ^= true;
        var colors = button.colors;
        colors.normalColor = ConnectionInitializer.instance.toggles[index] ? colorOn : colorOff;
        button.colors = colors;
    }
}
