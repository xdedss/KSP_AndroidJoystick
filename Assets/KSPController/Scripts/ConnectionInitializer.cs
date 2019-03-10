using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using SocketUtil;
using System;

public class ConnectionInitializer : MonoBehaviour {

    public static ConnectionInitializer instance;
    public Text debugText;

    [Space]
    public Gyro gyro;
    public Joystick joystickL;
    public Joystick joystickR;
    public InputField fieldIP;
    public Slider sliderThrottle;
    public Slider sliderTrim;
    public JoystickSingle joystickRudder;
    public bool[] toggles = new bool[16];

    public Transform gimbal;
    public TextMesh speedIndicator;
    public TextMesh headingIndicator;
    public Transform compass;

    SocketClient socketClient;
    Coroutine sendCoroutine;

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i] = false;
        }
    }

    void Start () {
        fieldIP.text = PlayerPrefs.GetString("server", "");
    }
	
	void Update () {
        if (socketClient != null)
        {
            Receive();
        }
	}

    public void Init()
    {
        var split = fieldIP.text.Split(':');
        if (split.Length <= 0) return;
        PlayerPrefs.SetString("server", fieldIP.text);
        PlayerPrefs.Save();

        var port = 23333;
        var ip = split[0];
        if(split.Length > 1)
        {
            if(!int.TryParse(split[1], out port))
            {
                port = 23333;
            }
        }
        socketClient = new SocketClient(ip, port);
        socketClient.StartClient();
        HandleInitialData(socketClient.ReceiveBlocked());
        if(sendCoroutine != null)
        {
            StopCoroutine(sendCoroutine);
        }
        sendCoroutine = StartCoroutine(SendBundle());
    }

    void Receive()
    {
        var data = socketClient.Receive();
        if(data != null)
        {
            HandleInfoData(data);
        }
    }

    void HandleInitialData(byte[] data)
    {
        if (data.Length >= 2)
        {
            var throttleValue = (float)data[0] / 255;
            sliderThrottle.value = throttleValue;
            ToggleButton.buttons[10].SetOn((data[1] & ByteMask(0)) != 0);
            ToggleButton.buttons[11].SetOn((data[1] & ByteMask(1)) != 0);
            ToggleButton.buttons[12].SetOn((data[1] & ByteMask(2)) != 0);
            ToggleButton.buttons[13].SetOn((data[1] & ByteMask(3)) != 0);
            ToggleButton.buttons[14].SetOn((data[1] & ByteMask(4)) != 0);
            ToggleButton.buttons[15].SetOn((data[1] & ByteMask(5)) != 0);
        }
    }

    void HandleInfoData(byte[] data)
    {
        Vector3 srfVel = new Vector3(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
        var rotX = (float)BitConverter.ToUInt16(data, 12) / 65535 * 2 - 1;
        var rotY = (float)BitConverter.ToUInt16(data, 14) / 65535 * 2 - 1;
        var rotZ = (float)BitConverter.ToUInt16(data, 16) / 65535 * 2 - 1;
        var rotW = (float)BitConverter.ToUInt16(data, 18) / 65535 * 2 - 1;
        Quaternion rotation = new Quaternion(rotX, rotY, rotZ, rotW);
        double longitude = (double)BitConverter.ToUInt32(data, 20) / uint.MaxValue * 360 - 180;
        double latitude = (double)BitConverter.ToUInt32(data, 24) / uint.MaxValue * 180 - 90;

        var forward = rotation * new Vector3(0, 0, 1);
        var horLength = forward.SetY(0).magnitude;
        var pitch = Mathf.Atan2(forward.y, horLength) * Mathf.Rad2Deg;
        var yaw = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        var roll = rotation.eulerAngles.z;

        gimbal.eulerAngles = new Vector3(0, yaw, 0);
        gimbal.Rotate(new Vector3(-1, 0, 0), pitch, Space.World);
        gimbal.Rotate(new Vector3(0, 0, -1), roll, Space.World);
        speedIndicator.text = srfVel.magnitude.ToString(".00") + " m/s";
        //Debug.Log((rotation * new Vector3(0, 0, 1)).ToString());
        //Debug.Log((rotation * new Vector3(0, 1, 0)).ToString());
        headingIndicator.text = Mathf.RoundToInt(yaw) + "°";
        compass.eulerAngles = new Vector3(0, 0, yaw);
        Debug.Log(string.Format("vel:{0},rot:{1},lon{2}/lat{3}", srfVel, rotation, longitude, latitude));

    }

    byte[] Bundle()
    {
        var j1raw = joystickL.Value;
        var j2raw = joystickR.Value;
        j2raw.y = Mathf.Clamp(j2raw.y + sliderTrim.value * 0.6f, -1, 1);
        var j1 = (j1raw + Vector2.one) / 2 * 255;
        var j2 = (j2raw + Vector2.one) / 2 * 255;
        var throttleB = sliderThrottle.value * 255;
        var rud = (joystickRudder.Value + 1) / 2 * 255;
        //return string.Format("{0}|{1}|{2}|{3}|", j1.x.ToString("0.00"), j1.y.ToString("0.00"), j2.x.ToString("0.00"), j2.y.ToString("0.00"));
        var masks = GetMasks();
        var bytes = new byte[]
        {
            (byte)Mathf.RoundToInt(j1.x),
            (byte)Mathf.RoundToInt(j1.y),
            (byte)Mathf.RoundToInt(j2.x),
            (byte)Mathf.RoundToInt(j2.y),
            (byte)Mathf.RoundToInt(throttleB),
            (byte)Mathf.RoundToInt(rud),
            masks[0],
            masks[1],
        };
        return bytes;
    }

    byte[] GetMasks()
    {
        byte mask1 = 0;
        byte mask2 = 0;
        for(int i = 0; i < 8; i++)
        {
            if (toggles[i])
            {
                //toggles[i] = false;
                mask1 |= ByteMask(i);
            }
        }
        for(int i = 8; i < 16; i++)
        {
            if (toggles[i])
            {
                //toggles[i] = false;
                mask2 |= ByteMask(i - 8);
            }
        }
        return new byte[]
        {
            mask1,
            mask2,
        };
    }

    byte ByteMask(int pos)
    {
        return (byte)(1 << pos);
    }

    IEnumerator SendBundle()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            //try
            //{
                socketClient.SendBytes(Bundle());
            //}
            //catch(Exception e)
            //{
            //    Debug.LogError(e.Message + '\n' + e.StackTrace);
            //    break;
            //}
        }
    }
}
