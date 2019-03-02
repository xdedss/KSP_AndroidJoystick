using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using SocketUtil;
using System;

public class ConnectionInitializer : MonoBehaviour {

    public static ConnectionInitializer instance;

    public Joystick joystickL;
    public Joystick joystickR;
    public InputField ipField;
    public Slider throttle;
    public Slider trim;
    public bool[] toggles = new bool[16];

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
        
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Init();
        }
	}

    public void Init()
    {
        socketClient = new SocketClient(ipField.text, 8080);
        socketClient.StartClient();
        if(sendCoroutine != null)
        {
            StopCoroutine(sendCoroutine);
        }
        sendCoroutine = StartCoroutine(SendBundle());
    }

    byte[] Bundle()
    {
        var j1raw = joystickL.Value;
        var j2raw = joystickR.Value;
        j2raw.y = Mathf.Clamp(j2raw.y + trim.value * 0.6f, -1, 1);
        var j1 = (j1raw + Vector2.one) / 2 * 255;
        var j2 = (j2raw + Vector2.one) / 2 * 255;
        var throttleB = throttle.value * 255;
        //return string.Format("{0}|{1}|{2}|{3}|", j1.x.ToString("0.00"), j1.y.ToString("0.00"), j2.x.ToString("0.00"), j2.y.ToString("0.00"));
        var masks = GetMasks();
        var bytes = new byte[]
        {
            (byte)Mathf.RoundToInt(j1.x),
            (byte)Mathf.RoundToInt(j1.y),
            (byte)Mathf.RoundToInt(j2.x),
            (byte)Mathf.RoundToInt(j2.y),
            (byte)Mathf.RoundToInt(throttleB),
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
                toggles[i] = false;
                mask1 |= ByteMask(i);
            }
        }
        for(int i = 8; i < 16; i++)
        {
            if (toggles[i])
            {
                toggles[i] = false;
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
            try
            {
                socketClient.SendBytes(Bundle());
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message + '\n' + e.StackTrace);
                break;
            }
        }
    }
}
