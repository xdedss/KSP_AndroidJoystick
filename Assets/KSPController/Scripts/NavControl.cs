using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavControl : MonoBehaviour {

    public Material mat;

    public void Set(float pitch, float roll)
    {
        mat.SetTextureOffset("_MainTex", new Vector2(0.5f, pitch / 180 + 0.5f));
        mat.SetFloat("_RotateAngle", roll * Mathf.Deg2Rad);
    }
    
	void Start () {
        Set(0, 0);
	}
	
	void Update () {
		
	}
}
