using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VelocityDisplayControl : MonoBehaviour {

    Text text;

    public void Set(float vel)
    {
        text.text = SettingsPanel.instance.ConvertVel(vel);
    }
    
	void Start () {
        text = GetComponent<Text>();
	}
	
	void Update () {
		
	}
}
