using System;
using System.Collections.Generic;
using UnityEngine;

public class DetailStack
{
    public DetailType detail;
    public float count;

    public DetailStack(DetailType detail, float count)
    {
        this.detail = detail;
        this.count = count;
    }
}

public class Complex : MonoBehaviour
{
    public ComplexType type;   
    public LineRenderer outputLineRenderer;

    public Complex nextComplex;

    [NonSerialized] public bool pointerStay;
    [NonSerialized] public Vector2 pointerDownPos;
    
    public EffeciencySystem effeciencySystem;
    public UnityEngine.UI.Image effeciencyFiller;
    
    public GameObject selectionFrameRoot;
    public TooltipInfoCnt tooltipInfoCnt;

    public virtual void Init()
    {
        if (selectionFrameRoot)
        {
            selectionFrameRoot.SetActive(false);
        }
        
        effeciencySystem = new();
        effeciencySystem.Init();

        if (outputLineRenderer)
        {
            var col = Vars.Instance.productionLineColorSystem.GetNextColor();
            outputLineRenderer.startColor = col;
            outputLineRenderer.endColor = col;
        }
    }

    public virtual void Update()
    {
        if (tooltipInfoCnt)
        {
            SetTooltip();
        }

        effeciencySystem.Update();
        if (effeciencyFiller)
        {
            effeciencyFiller.fillAmount = effeciencySystem.RelativeEffeciency;   
        }

        if (outputLineRenderer)
        {
            if (pointerStay)
            {
                outputLineRenderer.positionCount = 2;
                outputLineRenderer.SetPosition(0, pointerDownPos);
                outputLineRenderer.SetPosition(1, Vars.Instance.input.mouseWorldPos);
            }
            else
            {
                if (nextComplex)
                {
                    outputLineRenderer.positionCount = 2;
                    outputLineRenderer.SetPosition(0, transform.position);
                    outputLineRenderer.SetPosition(1, nextComplex.transform.position);
                }
                else
                {
                    outputLineRenderer.positionCount = 0;
                }
            }
        }
    }

    public virtual void SetTooltip()
    {
        tooltipInfoCnt.title = type.name;
        tooltipInfoCnt.desc = $"Effeciency: {(int)(effeciencySystem.Effeciency * 100)}/{(int)(effeciencySystem.MaxEffeciency * 100)}\n";
        if (nextComplex != null)
        {
            tooltipInfoCnt.desc += $"Connected With: {nextComplex.type.name}\n";
        }
    }

    public void OnPointerDown()
    {
        if (type.canHaveNextComplex)
        {
            pointerStay = true;
            pointerDownPos = Vars.Instance.input.mouseWorldPos;    
        }
    }
    public void OnPointerUp(Complex c)
    {
        if (type.canHaveNextComplex)
        {
            pointerStay = false;
            nextComplex = null;
            if (c != null && c != this && c.type != null && c.type.canBeNextComplex)
            {
                nextComplex = c;
            }
        }
    }

    public virtual bool CanReceive(DetailStack stack)
    {
        return true;
    }

    public virtual void Receive(DetailStack stack) {}

    // public void OnPointerClick()
    // {
    //     if (type != null)
    //     {
    //         Vars.Instance.ui.ShowConfirmDialog($"Destroy {type.name}?", () =>
    //         {
    //             Vars.Instance.buildSystem.DestroyBuild(this);
    //         }, null);    
    //     }
    // }
}

public class ProductionLineColorSystem
{
    public List<Color> awailableColors;
    
    public int id;
    public List<Color> dynamicColorList;

    public void Init()
    {
        awailableColors = new()
        {
            Utils.FromHexadecimal("#9e271e"),
            Utils.FromHexadecimal("#9e5a1e"),
            Utils.FromHexadecimal("#bfae0f"),
            Utils.FromHexadecimal("#49b00e"),
            Utils.FromHexadecimal("#0eb07f"),
            Utils.FromHexadecimal("#0e75b0"),
            Utils.FromHexadecimal("#240eb0"),
            Utils.FromHexadecimal("#5c0eb0"),
            Utils.FromHexadecimal("#b00e5c"),
            Utils.FromHexadecimal("#FFFFFF"),
        };
        Restart();
    }

    public void Restart()
    {
        dynamicColorList = new(awailableColors);
        dynamicColorList.Shuffle();
        id = 0;
    }

    public Color GetNextColor()
    {
        if (id >= dynamicColorList.Count)
        {
            dynamicColorList.Shuffle();
            id = 0;
        }    
        return dynamicColorList[id++];
    }
}

public class EffeciencySystem
{
    public float maxEffeciencyMultiplier;
    public float effeciencyGrowMultiplier;

    public float effeciency;
    public float maxEffeciency;
    public float effeciencyGrow;

    public float effeciencyGrowBonus;
    public float maxEffeciencyBonus;

    public float RelativeEffeciency => effeciency / MaxEffeciency;
    public float Effeciency => effeciency;
    public float MaxEffeciency => (maxEffeciency + maxEffeciencyBonus + Vars.Instance.modifiers.GetBonus<EffeciencyGrowModifier>()) * 
        (1.0f + maxEffeciencyMultiplier + Vars.Instance.modifiers.GetMultiplier<EffeciencyGrowModifier>());

    public void Init()
    {
        effeciency = 0.5f;
        maxEffeciency = 1.25f;
        effeciencyGrow = 0.01f;
    }

    public void Update()
    {
        var bonus = Vars.Instance.modifiers.GetBonus<EffeciencyGrowModifier>();
        var multiplier = Vars.Instance.modifiers.GetMultiplier<EffeciencyGrowModifier>();

        effeciency += (effeciencyGrow + effeciencyGrowBonus + bonus) * Vars.Instance.time.deltaDay * 
            (1.0f + effeciencyGrowMultiplier + multiplier);
        effeciency = Mathf.Clamp(effeciency, 0, MaxEffeciency);
    
        maxEffeciencyMultiplier = 0.0f;
        effeciencyGrowMultiplier = 0.0f;

        maxEffeciencyBonus = 0.0f;
        effeciencyGrowBonus = 0.0f;
    }
}

public class ComplexType
{
    public Complex prefab;
    public string name;
    public ComplexResearchTech research;
    public float buildTime;

    public bool canBeNextComplex = true;
    public bool canHaveNextComplex = true;

    public bool breakable = true;
    public bool chefAllowed = true;

    public bool buildable = true;

    public string desc;

    public Complex AsComplex()
    {
        var scr = GameObject.Instantiate(prefab);
        scr.type = this;
        return scr;
    }
}

public class CraftingComplexType : ComplexType
{
    public CraftRecipe recipe;
}