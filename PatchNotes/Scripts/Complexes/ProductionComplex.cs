using UnityEngine;

public class ProductionComplex : Complex
{
    public ProducingComplexType ProducingComplexType => (ProducingComplexType)type;

    public override void Update()
    {
        var m = Vars.Instance.materialPrices.MaterialPrice;
        if (nextComplex)
        {
            foreach (var stack in ProducingComplexType.outputStacks)
            {
                float c = stack.count * effeciencySystem.effeciency;
                c = nextComplex.GetReceiveCount(stack);
                nextComplex.Receive(new(stack.detail, c));
                float e = c * m;
                Vars.Instance.income.ExpenseByMaterial(e);
            }
        }
        
        base.Update();
    }
}