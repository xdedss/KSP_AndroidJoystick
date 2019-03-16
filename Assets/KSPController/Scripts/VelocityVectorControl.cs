using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VelocityVectorControl : MonoBehaviour {

    float scale = 9.6f;

    RectTransform rectTransform;
    RawImage image;

    public void Set(Vector3 vel, Quaternion rotation)
    {
        var localVel = Quaternion.Inverse(rotation) * vel;
        var pitch = Mathf.Atan(localVel.y / localVel.z) * Mathf.Rad2Deg;
        var yaw = Mathf.Atan(localVel.x / localVel.z) * Mathf.Rad2Deg;
        //Debug.Log(localVel);
        image.enabled = (localVel.z > 0.1f) && (Mathf.Abs(pitch) < 30) && (Mathf.Abs(yaw) < 30);
        rectTransform.anchoredPosition = new Vector2(yaw, pitch) * scale;
    }

	void Start () {
        image = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();
	}
	
	void Update () {
		
	}
}
