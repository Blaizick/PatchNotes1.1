using System;
using UnityEngine;

public class BuildingComplex : Complex
{
    public ComplexType buildingComplex;
    [NonSerialized] public float progress;

    public override bool IsChefAllowed => false;

    public override void Init()
    {
        
    }

    public override void Update()
    {
        progress += Vars.Instance.time.deltaDay / buildingComplex.buildTime;
        effeciencyFiller.fillAmount = progress;
        if (progress > 1)
        {
            Vars.Instance.buildSystem.FinishBuilding(this);
        }
    }
}