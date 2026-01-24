using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftRecipe : IFormattable
{
    public float craftTime;

    public List<DetailStack> inputStacks;
    public List<DetailStack> outputStacks;

    public string ToString(string format, IFormatProvider formatProvider)
    {
        string str = string.Empty;
        str += $"Crafting Time: {craftTime} days\n";
        if (inputStacks != null && inputStacks.Count > 0)
        {
            str += "Input:\n";
            foreach  (var stack in inputStacks)
            {
                str += $"{stack.detail.name}: {stack.count}\n";
            }
        }
        if (outputStacks != null && outputStacks.Count > 0)
        {
            str += "Output:\n";
            foreach  (var stack in outputStacks)
            {
                str += $"{stack.detail.name}: {stack.count}\n";
            }
        }
        return str;
    }
}

public class CraftingComplexType : ComplexType
{
    public CraftRecipe recipe;
    public float capacity;

    public override string GetDesc()
    {
        return $"{base.GetDesc()}\nCapacity: {capacity}\nRecipe:\n{recipe}\n";
    }
}

public class CraftingComplex : Complex
{
    public CraftingComplexType CraftingComplexType => (CraftingComplexType)type;

    [NonSerialized] public bool crafting;
    [NonSerialized] public float craftProgress;
    [NonSerialized] public Dictionary<DetailType, float> inputDic = new();
    [NonSerialized] public Dictionary<DetailType, float> outputDic = new();

    public override void Init()
    {
        foreach (var i in CraftingComplexType.recipe.inputStacks)
        {
            inputDic[i.detail] = 0;
        }
        foreach (var i in CraftingComplexType.recipe.outputStacks)
        {
            outputDic[i.detail] = 0;
        }

        base.Init();
    }

    public override void Update()
    {
        if (nextComplex)
        {
            List<DetailStack> changes = new();
            foreach (var (k, v) in outputDic)
            {
                float c = nextComplex.GetReceiveCount(new(k, v));
                changes.Add(new(k, c));
                nextComplex.Receive(new(k, c));
            }
            foreach (var s in changes)
            {
                outputDic[s.detail] -= s.count;
            } 
        }

        if (crafting)
        {
            bool canStore = true;
            foreach (var (k, v) in outputDic)
            {
                if (v >= CraftingComplexType.capacity)
                {
                    canStore = false;
                    continue;
                }
            }
            if (canStore)
            {
                craftProgress += Vars.Instance.time.deltaDay * effeciencySystem.effeciency;
                if (craftProgress > 1)
                {
                    foreach (var stack in CraftingComplexType.recipe.outputStacks)
                    {
                        if (outputDic[stack.detail] + stack.count >= CraftingComplexType.capacity)
                            outputDic[stack.detail] = CraftingComplexType.capacity;
                        else
                            outputDic[stack.detail] += stack.count;
                    }
                    crafting = false;
                }    
            }
        }
        else
        {
            bool canStart = true;
            foreach (var i in CraftingComplexType.recipe.inputStacks)
            {
                if (inputDic[i.detail] < i.count)
                {
                    canStart = false;
                    break;
                }
            }
            if (canStart)
            {
                foreach (var i in CraftingComplexType.recipe.inputStacks)
                {
                    inputDic[i.detail] -= i.count;
                    inputDic[i.detail] = Mathf.Clamp(inputDic[i.detail], 0, float.MaxValue);
                }
                crafting = true;
                craftProgress = 0.0f;
            }
        }

        base.Update();
    }

    public override void Receive(DetailStack stack)
    {
        if (inputDic.ContainsKey(stack.detail))
        {
            inputDic[stack.detail] += stack.count;
        }
    }
    public override float GetReceiveCount(DetailStack stack)
    {
        if (!inputDic.ContainsKey(stack.detail))
        {
            return 0;
        }
        return Mathf.Clamp(stack.count, 0, CraftingComplexType.capacity - inputDic[stack.detail]);
    }
}