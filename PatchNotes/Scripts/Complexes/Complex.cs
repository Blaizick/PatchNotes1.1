using System;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class DetailStack
{
    public DetailType detail;
    public float count;

    public DetailStack(DetailType detail, float count)
    {
        this.detail = detail;
        this.count = count;
    }
}

public class Complex : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public LineRenderer outputLineRenderer;

    public Complex nextComplex;

    [NonSerialized] public bool pointerStay;
    [NonSerialized] public Vector2 pointerDownPos;

    public virtual void Init()
    {
        
    }

    public virtual void Update()
    {
        if (outputLineRenderer)
        {
            if (pointerStay)
            {
                outputLineRenderer.positionCount = 2;
                outputLineRenderer.SetPosition(0, pointerDownPos);
                outputLineRenderer.SetPosition(1, Vars.Instance.input.mouseWorldPos);
            }
            else
            {
                if (nextComplex)
                {
                    outputLineRenderer.positionCount = 2;
                    outputLineRenderer.SetPosition(0, transform.position);
                    outputLineRenderer.SetPosition(1, nextComplex.transform.position);
                }
                else
                {
                    outputLineRenderer.positionCount = 0;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerStay = true;
        pointerDownPos = Camera.main.ScreenToWorldPoint(eventData.position);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        pointerStay = false;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

        nextComplex = null;
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Complex>(out var c) && c != this)
            {
                nextComplex = c;
                break;
            }
        }
    }

    public virtual void Receive(DetailStack stack){}

}