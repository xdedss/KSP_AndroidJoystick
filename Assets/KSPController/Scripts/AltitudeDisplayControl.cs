using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltitudeDisplayControl : MonoBehaviour {

    Text text;

    public void Set(float alt)
    {
        text.text = SettingsPanel.instance.ConvertAlt(alt);
    }
    
	void Start () {
        text = GetComponent<Text>();
	}
	
	void Update () {
		
	}
}
