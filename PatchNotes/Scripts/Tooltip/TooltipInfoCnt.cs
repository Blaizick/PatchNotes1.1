using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipInfoCnt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string title;
    public string desc;

    [NonSerialized] public bool pointerStay;

    public void Update()
    {
        if (pointerStay)
        {
            Vars.Instance.tooltip.Set(title, desc);
        }
    }

    public void OnDestroy()
    {
        if (pointerStay)
        {
            Vars.Instance.tooltip.Hide();
        }        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(desc))
        {
            pointerStay = true;
            Vars.Instance.tooltip.Show(title, desc);    
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        pointerStay = false;
        Vars.Instance.tooltip.Hide();
    }
}