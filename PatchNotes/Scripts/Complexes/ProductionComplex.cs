using UnityEngine;

public class ProductionComplex : Complex
{
    public override void Update()
    {
        nextComplex?.Receive(new DetailStack(Details.ironOre, 0.5f * Time.deltaTime));
    
        base.Update();
    }
}