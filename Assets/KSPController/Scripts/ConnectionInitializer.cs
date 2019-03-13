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
            var parsedData = new SocketDataParser.ServerSideInitialData(data);
            sliderThrottle.value = parsedData.throttle;
            ToggleButton.buttons[10].SetOn(parsedData.SAS);
            ToggleButton.buttons[11].SetOn(parsedData.RCS);
            ToggleButton.buttons[12].SetOn(parsedData.brake);
            ToggleButton.buttons[13].SetOn(parsedData.light);
            ToggleButton.buttons[14].SetOn(parsedData.gear);
            ToggleButton.buttons[15].SetOn(parsedData.stage);
        }
    }

    void HandleInfoData(byte[] data)
    {
        var serverData = new SocketDataParser.ServerSideSocketData(data);

        Vector3 srfVel = serverData.srfVel;
        Quaternion rotation = serverData.rotation;
        double longitude = serverData.longitude;
        double latitude = serverData.latitude;
        float altitudeSL = serverData.altitudeSealevel;//TODO: display altitude
        float altitudeR = serverData.altitudeRadar;

        var forward = rotation * new Vector3(0, 0, 1);
        var horLength = forward.SetY(0).magnitude;
        var pitch = Mathf.Atan2(forward.y, horLength) * Mathf.Rad2Deg;
        var yaw = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        var roll = rotation.eulerAngles.z;

        gimbal.eulerAngles = new Vector3(0, yaw, 0);
        gimbal.Rotate(new Vector3(-1, 0, 0), pitch, Space.World);
        gimbal.Rotate(new Vector3(0, 0, -1), roll, Space.World);
        speedIndicator.text = srfVel.magnitude.ToString(".00") + " m/s";
        headingIndicator.text = Mathf.RoundToInt(yaw) + "°";
        compass.eulerAngles = new Vector3(0, 0, yaw);
        Debug.Log(string.Format("vel:{0},rot:{1},lon{2}/lat{3}/alt{4}", srfVel, rotation, longitude, latitude, altitudeR));

    }

    byte[] Bundle()
    {
        var clientData = new SocketDataParser.ClientSideSocketData();
        clientData.joystickL = joystickL.Value;
        clientData.joystickR = joystickR.Value;
        clientData.throttle = sliderThrottle.value;
        clientData.steering = joystickRudder.Value;
        for(int i = 0; i < 10; i++)
        {
            clientData.actions[i] = toggles[i];
        }
        clientData.SAS = toggles[10];
        clientData.RCS = toggles[11];
        clientData.brake = toggles[12];
        clientData.light = toggles[13];
        clientData.gear = toggles[14];
        clientData.stage = toggles[15];
        return clientData;
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
            var bundle = Bundle();

            //string b = "send bundle";
            //foreach (byte by in bundle)
            //{
            //    b += '|' + by;
            //}
            //Debug.Log(b);
            //try
            //{
                socketClient.SendBytes(bundle);
            //}
            //catch(Exception e)
            //{
            //    Debug.LogError(e.Message + '\n' + e.StackTrace);
            //    break;
            //}
        }
    }
}
