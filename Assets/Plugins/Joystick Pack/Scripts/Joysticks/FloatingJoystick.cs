using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public Vector3 UpDirection;
    public float UpH;
    public float UpV;
    private bool isShow = true;
    public Action OnPointerDownAction;
    public Action OnPointerUpAction;

    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public void SetIsShow(bool show) {
        isShow = show;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        if (isShow) {
            background.gameObject.SetActive(true); 
        }
        OnPointerDownAction?.Invoke();
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        UpDirection = Vector3.forward * Vertical + Vector3.right * Horizontal;
        UpH = Horizontal;
        UpV = Vertical;
        if (isShow) {
            background.gameObject.SetActive(false);
        }
        OnPointerUpAction?.Invoke();
        base.OnPointerUp(eventData);
    }
}