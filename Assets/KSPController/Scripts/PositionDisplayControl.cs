using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionDisplayControl : MonoBehaviour {

    Text text;

    public void Set(float lon, float lat)
    {
        string lonstr = Mathf.Abs(lon).ToString("0.0°") + (lon > 0 ? "E" : "W");
        string latstr = Mathf.Abs(lat).ToString("0.0°") + (lat > 0 ? "N" : "S");
        text.text = lonstr + ',' + latstr;
    }

	void Start () {
        text = GetComponent<Text>();
	}
	
	void Update () {
		
	}
}
