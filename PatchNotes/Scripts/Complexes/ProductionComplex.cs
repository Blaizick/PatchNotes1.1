using UnityEngine;

public class ProductionComplex : Complex
{
    public ProducingComplexType ProducingComplexType => (ProducingComplexType)type;

    public override void Update()
    {
        var m = Vars.Instance.materialPrices.MaterialPrice;
        foreach (var stack in ProducingComplexType.outputStacks)
        {
            float c = stack.count * effeciencySystem.effeciency * Vars.Instance.time.deltaDay;
            float e = 0;
            foreach (var nc in nextComplexes)
            {
                float lc = nc.GetReceiveCount(new DetailStack(stack.detail, c));
                nc.Receive(new(stack.detail, lc));
                c -= lc;
                e += lc * m;
            }
            Vars.Instance.income.ExpenseByMaterial(e);    
        }
        
        base.Update();
    }
}