using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttitudeIndicatorSwitch : MonoBehaviour {

    public GameObject navball;
    public GameObject navscreen;
    
	void Start () {
		
	}
	
	void Update () {
		if(navball.activeInHierarchy ^ SettingsPanel.useNavball)
        {
            navball.SetActive(SettingsPanel.useNavball);
            navscreen.SetActive(!SettingsPanel.useNavball);
        }
	}
}
