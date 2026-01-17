
using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftRecipe
{
    public float craftTime;

    public List<DetailStack> inputStacks;
    public List<DetailStack> outputStacks;
}

public static class Recipes
{
    public static CraftRecipe smeltRecipe;

    public static List<CraftRecipe> all;

    public static void Init()
    {
        smeltRecipe = new()
        {
            craftTime = 1.0f,
            inputStacks = new()
            {
                new(Details.ironOre, 1.0f)
            },
            outputStacks = new()
            {
                new(Details.ironIngot, 1.0f)
            },
        };
    
        all = new()
        {
            smeltRecipe
        };
    }
}

public class CraftingComplex : Complex
{
    public CraftRecipe recipe;

    [NonSerialized] public bool crafting;
    [NonSerialized] public float craftStartDay;
    [NonSerialized] public Dictionary<DetailType, float> detailsDic = new();

    public override void Init()
    {
        recipe = Recipes.smeltRecipe;

        foreach (var i in recipe.inputStacks)
        {
            detailsDic[i.detail] = 0;
        }

        base.Init();
    }

    public override void Update()
    {
        if (crafting)
        {
            if (Vars.Instance.time.day - craftStartDay >= recipe.craftTime)
            {
                foreach (var i in recipe.inputStacks)
                {
                    detailsDic[i.detail] -= i.count;
                    detailsDic[i.detail] = Mathf.Clamp(detailsDic[i.detail], 0, float.MaxValue);
                }
                if (nextComplex)
                {
                    foreach (var i in recipe.outputStacks)
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
            foreach (var i in recipe.inputStacks)
            {
                if (detailsDic[i.detail] < i.count)
                {
                    canStart = false;
                    break;
                }
            }
            if (canStart)
            {
                crafting = true;
                craftStartDay = Vars.Instance.time.day;
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