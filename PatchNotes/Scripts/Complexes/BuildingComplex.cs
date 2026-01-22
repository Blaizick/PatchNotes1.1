using System;
using UnityEngine;

public class BuildingComplex : Complex
{
    public ComplexType buildingComplex;
    [NonSerialized] public float progress;

    public override void Update()
    {
        var modifier = 1.0f + Vars.Instance.modifiers.GetBonus<BuildSpeedModifier>();
        progress += Vars.Instance.time.deltaDay / buildingComplex.buildTime * modifier;
        effeciencyFiller.fillAmount = progress;
        if (progress > 1)
        {
            Vars.Instance.buildSystem.FinishBuilding(this);
        }
        SetTooltip();
    }

    public override void SetTooltip()
    {
        var modifier = 1.0f + Vars.Instance.modifiers.GetBonus<BuildSpeedModifier>();
        tooltipInfoCnt.title = type.name;
        tooltipInfoCnt.desc = $"Building: {buildingComplex.name}\n" +
                              $"Time Left: {(int)((1.0f - progress) * buildingComplex.buildTime / modifier)} days\n";
    }
}