using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftRecipe
{
    public float craftTime;

    public List<DetailStack> inputStacks;
    public List<DetailStack> outputStacks;
}

public class CraftingComplex : Complex
{
    public CraftingComplexType CraftingComplexType => (CraftingComplexType)type;

    [NonSerialized] public bool crafting;
    [NonSerialized] public float craftProgress;
    [NonSerialized] public Dictionary<DetailType, float> detailsDic = new();

    public override void Init()
    {
        foreach (var i in CraftingComplexType.recipe.inputStacks)
        {
            detailsDic[i.detail] = 0;
        }

        base.Init();
    }

    public override void Update()
    {
        if (crafting)
        {
            craftProgress += Vars.Instance.time.deltaDay * effeciencySystem.effeciency;
            if (craftProgress > 1)
            {
                if (nextComplex)
                {
                    foreach (var i in CraftingComplexType.recipe.outputStacks)
                    {
                        nextComplex.Receive(i);
                    }
                }
                crafting = false;
            }
        }
        else
        {
            bool canStart = true;
            foreach (var i in CraftingComplexType.recipe.inputStacks)
            {
                if (detailsDic[i.detail] < i.count)
                {
                    canStart = false;
                    break;
                }
            }
            if (canStart)
            {
                foreach (var i in CraftingComplexType.recipe.inputStacks)
                {
                    detailsDic[i.detail] -= i.count;
                    detailsDic[i.detail] = Mathf.Clamp(detailsDic[i.detail], 0, float.MaxValue);
                }
                crafting = true;
                craftProgress = 0.0f;
            }
        }

        base.Update();
    }

    public override void Receive(DetailStack stack)
    {
        if (detailsDic.ContainsKey(stack.detail))
        {
            detailsDic[stack.detail] += stack.count;
        }
    }
}