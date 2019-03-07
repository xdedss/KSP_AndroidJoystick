﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour {

    public float radius = 120;
    public RectTransform stick;
    public bool isDragging = false;
    RectTransform rectTransform;

    public bool gyroOverride = false;
    public Slider gyroSensitivity;

    Vector3 targetPosition = Vector2.zero;

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
        radius = Screen.height / 4;
        //rectTransform.right = radius / 2;
        rectTransform.sizeDelta= new Vector2(radius, radius) * 2;
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
        var clamped = Clamp(localPos.V2());
        targetPosition = clamped.V3(0);
        Value = clamped / radius;
    }

    Vector2 Clamp(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, -radius, radius);
        pos.y = Mathf.Clamp(pos.y, -radius, radius);
        return pos;
    }

}
