using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickSingle : MonoBehaviour {

    public float radius = 120;
    public RectTransform stick;
    RectTransform rectTransform;

    Vector3 targetPosition = Vector2.zero;

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
        radius = Screen.height / 4;
        //rectTransform.right = radius / 2;
        rectTransform.sizeDelta = rectTransform.sizeDelta.SetX(radius * 2);
    }
	
	void Update () {
        var target = transform.position + targetPosition;
        stick.position = Vector3.Lerp(stick.position, target, Mathf.Clamp01(Time.deltaTime * 20));
    }

    public void TouchDown(BaseEventData dataraw)
    {
        var data = dataraw as PointerEventData;
        ChangePosition(data.position.V3(0) - transform.position);
    }

    public void TouchUp(BaseEventData dataraw)
    {
        var data = dataraw as PointerEventData;
        ChangePosition(Vector3.zero);
    }

    public void Drag(BaseEventData dataraw)
    {
        var data = dataraw as PointerEventData;
        ChangePosition(data.position.V3(0) - transform.position);
    }

    void ChangePosition(Vector3 localPos)
    {
        var clamped = Clamp(localPos.V2());
        targetPosition = clamped.V3(0);
        Value = clamped.x / radius;
    }

    Vector2 Clamp(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, -radius, radius);
        pos.y = 0;
        return pos;
    }
}
