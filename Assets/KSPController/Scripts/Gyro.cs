using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro : MonoBehaviour {

    public UnityEngine.UI.Text text;

    public static float yCenter = 0;

    public static Quaternion CenterRot;

    public static Vector2 Value
    {
        get
        {
            return pos;
        }
    }
    private static Vector2 pos = Vector2.zero;

	void Start () {
        Input.gyro.enabled = true;
	}
	
	void Update () {
        //euler
        //x roll left
        //y pull up

        var eulers = Input.gyro.attitude.eulerAngles;
        var deulers = (Quaternion.Inverse(CenterRot) * Input.gyro.attitude).eulerAngles;

        pos.x = -NormalizeDegrees(0, deulers.z) / 10;
        pos.y = -NormalizeDegrees(0, deulers.x) / 10;


        //text.text = eulers.ToString("0.0000") + pos.ToString() + "\n" + deulers.ToString();
    }

    public static void Calibrate()
    {
        var eulers = Input.gyro.attitude.eulerAngles;
        yCenter = eulers.y;
        CenterRot = Input.gyro.attitude;
    }

    float NormalizeDegrees(float center, float degree)
    {
        while(degree > center + 180)
        {
            degree -= 360;
        }
        while(degree <= center - 180)
        {
            degree += 360;
        }
        return degree;
    }
}
