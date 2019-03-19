using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickSingle : MonoBehaviour, IChangeColor {

    public float radius = 120;
    public RectTransform stick;
    public bool isDragging = false;
    RectTransform rectTransform;

    public float mappings;

    Vector3 targetPosition = Vector2.zero;

    Image imageBack;
    Image imageHandle;
    Text infoText;
    Transform markBL;
    Transform markUR;

    public float Value
    {
        get
        {
            return pos;
        }
        private set
        {
            pos = value;
        }
    }
    private float pos;

    void Start ()
    {
        rectTransform = GetComponent<RectTransform>();
        //radius = rectTransform.sizeDelta.x / 2;
        //radius = Mathf.Min(Screen.height / 4, Screen.width / 8);
        //rectTransform.right = radius / 2;
        //rectTransform.sizeDelta = rectTransform.sizeDelta.SetX(radius * 2);

        ColorManager.instance.coloredElements.Add(this);
        imageBack = GetComponent<Image>();
        imageHandle = transform.Find("Joystick").GetComponent<Image>();
        infoText = transform.Find("InfoText").GetComponent<Text>();

        markBL = transform.Find("MarkBL");
        markUR = transform.Find("MarkUR");
        radius = (markUR.position.x - markBL.position.x) / 2;
    }
	
	void Update () {
        if (!isDragging)
        {
            float additive = 0;
            additive += mappings;
            ChangePosition(new Vector2(radius * additive, 0));
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
        var clamped = Clamp(localPos.V2());
        targetPosition = clamped.V3(0);
        Value = clamped.x / radius;
    }

    public void UpdateColor()
    {
        imageBack.color = ColorManager.instance.joystickBg;
        imageHandle.color = ColorManager.instance.joystickHandle;
        infoText.color = ColorManager.instance.joystickInfo;
    }

    Vector2 Clamp(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, -radius, radius);
        pos.y = 0;
        return pos;
    }
}
