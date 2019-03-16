using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAlign : MonoBehaviour {


    
	void Start () {
        transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        GetComponent<Camera>().orthographicSize = Screen.height / 2;
	}
	
	void Update () {
		
	}
}
