using System;
using UnityEngine;
using UnityEngine.UI;

public class ILSControl : MonoBehaviour
{

    public MeshRenderer lineHor;
    public MeshRenderer lineVer;
    public Text siteNameText;

    void Start()
    {
        RunwayManager.Init();
    }

    void Update()
    {

    }

    public void UpdatePosition(double lat, double lon, float altSL)
    {
        if (SettingsPanel.ILSMode == 1)
        {
            lineHor.enabled = false;
            lineVer.enabled = false;
            siteNameText.enabled = false;
            return;
        }
        float horTan;
        float verTan;
        var runway = RunwayManager.FindNearestRunway(lat, lon, altSL, "Kerbin", out horTan, out verTan);
        //Debug.Log(runway);
        if (runway == null)
        {
            lineHor.enabled = false;
            lineVer.enabled = false;
            siteNameText.enabled = false;
        }
        else
        {
            lineHor.enabled = true;
            lineVer.enabled = true;
            siteNameText.enabled = true;
            siteNameText.text = runway.GetIdStr();
            //runway.AssessBias(lat, lon, altSL, out horTan, out verTan);
            //Debug.Log("bias: h-v  " + horTan + "  ,  " + verTan);
            lineHor.transform.localPosition = lineHor.transform.localPosition.SetX(Mathf.Clamp(-horTan / 0.12f, -0.4f, 0.4f));
            lineVer.transform.localPosition = lineVer.transform.localPosition.SetY(Mathf.Clamp(-verTan / 0.2f, -0.4f, 0.4f));
        }
    }
}