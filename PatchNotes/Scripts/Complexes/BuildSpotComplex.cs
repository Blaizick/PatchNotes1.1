using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class BuildSpotComplex : Complex, IPointerClickHandler
{
    public UnityEvent<Complex> onConvert = new();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        List<ComplexType> complexes = new();
        foreach (var c in Complexes.all)
        {
            if (Vars.Instance.researches.IsResearched(c.research))
            {
                complexes.Add(c);
            }
        }
        Vars.Instance.ui.RebuildBuildComplexDialog(Camera.main.WorldToScreenPoint(transform.position), complexes, c =>
        {
            Vars.Instance.buildSystem.StartBuilding(this, c);
            Vars.Instance.ui.buildComplexDialogRoot.SetActive(false);
        });
    }

    public override string GetDesc()
    {
        return type.GetDesc();
    }
}