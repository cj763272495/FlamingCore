using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public Vector3 UpDirection;
    public float UpH;
    public float UpV;
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        UpDirection = Vector3.forward * Vertical + Vector3.right * Horizontal;
        UpH = Horizontal;
        UpV = Vertical;
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}