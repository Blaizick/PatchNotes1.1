public class PackingComplex : Complex
{
    public override void Init()
    {
        type = Complexes.packingComplex;
        base.Init();
    }

    public override void Receive(DetailStack stack)
    {
        Vars.Instance.details.Add(stack.detail, stack.count);
    }
}