public class PackingComplex : Complex
{
    public override bool IsChefAllowed => false;
    public override bool CanBreak => false;

    public override void Receive(DetailStack stack)
    {
        Vars.Instance.detailsSystem.Add(stack.detail, stack.count);
    }
}