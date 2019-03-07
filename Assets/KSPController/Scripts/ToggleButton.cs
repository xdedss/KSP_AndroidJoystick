using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour {

    public bool on
    {
        get
        {
            return on_;
        }
        set
        {
            SetOn(value);
        }
    }
    bool on_ = false;
    public int index;
    public Color colorOn = new Color(0.5f, 1, 1);
    public Color colorOff = new Color(0.8f, 0.8f, 0.8f);
    Button button;

	void Start () {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
	}
	
	void Update () {
		
	}

    void SetOn(bool isOn)
    {

    }

    void OnClick()
    {
        ConnectionInitializer.instance.toggles[index] ^= true;
        //Debug.Log(gameObject.name);
    }
}
