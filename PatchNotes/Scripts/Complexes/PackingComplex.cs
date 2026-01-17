public class PackingComplex : Complex
{
    public override void Receive(DetailStack stack)
    {
        Vars.Instance.detailsSystem.Add(stack.detail, stack.count);
    }
}