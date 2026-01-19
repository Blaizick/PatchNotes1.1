using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipInfoCnt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string title;
    public string desc;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vars.Instance.tooltip.Show(title, desc);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Vars.Instance.tooltip.Hide();
    }
}