using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassLineControl : MonoBehaviour {

    public Material mat;
    public Text text;

    public void Set(float degrees)
    {
        mat.SetTextureOffset("_MainTex", new Vector2(0.375f + degrees / 360, 0));
        if (degrees < 0)
        {
            degrees += 360;
        }
        text.text = degrees.ToString("0.");
    }
    
	void Start () {
        Set(0);
	}
	
	void Update () {
		
	}
}
