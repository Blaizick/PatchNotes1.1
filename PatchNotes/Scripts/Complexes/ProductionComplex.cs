using UnityEngine;

public class ProductionComplex : Complex
{
    public override void Update()
    {
        var given = 4 * Vars.Instance.time.deltaDay * effeciencySystem.effeciency;
        nextComplex?.Receive(new DetailStack(Details.ironOre, given));
        Vars.Instance.materialCostSystem.Add(given);

        base.Update();
    }
}