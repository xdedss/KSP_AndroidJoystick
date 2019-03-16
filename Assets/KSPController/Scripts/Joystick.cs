using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IChangeColor {

    public float radius = 120;
    public RectTransform stick;
    public bool isDragging = false;
    RectTransform rectTransform;

    public bool gyroOverride = false;
    public Slider gyroSensitivity;

    Vector3 targetPosition = Vector2.zero;

    Image imageBack;
    Image imageHandle;
    Text infoText;
    RawImage[] centerlines;
    Transform markBL;
    Transform markUR;

    public Vector2 Value {
        get
        {
            return pos;
        }
        private set
        {
            pos = value;
        }
    }
    private Vector2 pos;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //radius = Mathf.Min(Screen.height / 4, Screen.width / 8);
        //rectTransform.right = radius / 2;
        //rectTransform.sizeDelta= new Vector2(radius, radius) * 2;
        //rectTransform.anchoredPosition = rectTransform.anchoredPosition.SetX(Mathf.Sign(rectTransform.anchoredPosition.x) * (160 + radius));

        ColorManager.instance.coloredElements.Add(this);
        imageBack = GetComponent<Image>();
        imageHandle = transform.Find("Joystick").GetComponent<Image>();
        infoText = transform.Find("InfoText").GetComponent<Text>();
        var centerlineParent = transform.Find("Centerline");
        centerlines = new RawImage[] { centerlineParent.GetChild(0).GetComponent<RawImage>(), centerlineParent.GetChild(1).GetComponent<RawImage>() };
        markBL = transform.Find("MarkBL");
        markUR = transform.Find("MarkUR");
        radius = (markUR.position.x - markBL.position.x) / 2;
    }

    private void Update()
    {
        if(gyroOverride && !isDragging)
        {
            ChangePosition(radius * Gyro.Value * gyroSensitivity.value);
        }

        var target = transform.position + targetPosition;
        stick.position = Vector3.Lerp(stick.position, target, Mathf.Clamp01(Time.deltaTime * 20));
    }

    public void TouchDown(BaseEventData dataraw)
    {
        var data = dataraw as PointerEventData;
        ChangePosition(data.position.V3(0) - transform.position);
        isDragging = true;
    }

    public void TouchUp(BaseEventData dataraw)
    {
        var data = dataraw as PointerEventData;
        ChangePosition(Vector3.zero);
        isDragging = false;
    }

    public void Drag(BaseEventData dataraw)
    {
        var data = dataraw as PointerEventData;
        ChangePosition(data.position.V3(0) - transform.position);
        isDragging = true;
    }

    void ChangePosition(Vector3 localPos)
    {
        var raw = localPos.V2();
        var rawinput = raw / radius;
        var curveInput = new Vector2(SettingsPanel.instance.ConvertInput(rawinput.x), SettingsPanel.instance.ConvertInput(rawinput.y));
        var curvePos = curveInput * radius;
        var clamped = Clamp(curvePos);
        targetPosition = clamped.V3(0);
        Value = clamped / radius;
    }

    public void UpdateColor()
    {
        imageBack.color = ColorManager.instance.joystickBg;
        imageHandle.color = ColorManager.instance.joystickHandle;
        infoText.color = ColorManager.instance.joystickInfo;
        centerlines[0].color = ColorManager.instance.joystickCenterline;
        centerlines[1].color = ColorManager.instance.joystickCenterline;
    }

    Vector2 Clamp(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, -radius, radius);
        pos.y = Mathf.Clamp(pos.y, -radius, radius);
        return pos;
    }

}
