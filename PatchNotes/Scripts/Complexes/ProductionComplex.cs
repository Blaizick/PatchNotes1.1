using UnityEngine;

public class ProductionComplex : Complex
{
    public override void Update()
    {
        var matPrice = Vars.Instance.materialPriceSystem.MaterialPrice;
        if (nextComplex)
        {
            var count = Vars.Instance.time.deltaDay * effeciencySystem.effeciency;
            var expense = count * matPrice;
            nextComplex.Receive(new DetailStack(Details.ore, count));
            Vars.Instance.income.ExpenseByMaterial(expense);
        }
        
        base.Update();
    }
}