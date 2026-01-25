using System;
using System.Collections.Generic;
using UnityEngine;

public class DetailStack : IFormattable
{
    public DetailType detail;
    public float count;

    public DetailStack(DetailType detail, float count)
    {
        this.detail = detail;
        this.count = count;
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"{detail.name}: {count}";
    }
}

public class Complex : MonoBehaviour
{
    public ComplexType type;   
    public LineRenderer outputLineRenderer;

    // public Complex nextComplex;

    [NonSerialized] public bool pointerStay;
    [NonSerialized] public Vector2 pointerDownPos;
    
    public EffeciencySystem effeciencySystem;
    public UnityEngine.UI.Image effeciencyFiller;
    
    public GameObject selectionFrameRoot;
    public GameObject circleSelectionRoot;
    public TooltipInfoCnt tooltipInfoCnt;

    public AudioSource audioSource;

    [NonSerialized] public List<Complex> nextComplexes = new();

    [NonSerialized] public bool affectedByChef = false;

    public virtual void Init()
    {
        if (selectionFrameRoot)
        {
            selectionFrameRoot.SetActive(false);
        }
        if (circleSelectionRoot)
        {
            circleSelectionRoot.SetActive(false);
        }
        
        effeciencySystem = new();
        effeciencySystem.Init();

        if (outputLineRenderer)
        {
            var col = Vars.Instance.productionLineColor.GetNextColor();
            outputLineRenderer.startColor = col;
            outputLineRenderer.endColor = col;
        }

        affectedByChef = false;

        StartPlayingSound();
    }

    public virtual void StartPlayingSound()
    {
        // if (audioSource && type.idleClip)
        // {
        //     audioSource.minDistance = 1.0f;
        //     audioSource.minDistance = 500.0f;
        //     audioSource.clip = type.idleClip;
        //     audioSource.Play();
        // }
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

        if (type.canHaveNextComplex && outputLineRenderer != null)
        {
            nextComplexes.RemoveAll(c => c == null); 
            List<Vector3> positions = new();
            foreach (var c in nextComplexes)
            {   
                positions.Add(transform.position);
                positions.Add(c.transform.position);
            }
            outputLineRenderer.positionCount = positions.Count;
            outputLineRenderer.SetPositions(positions.ToArray());
        }
        
        // if (outputLineRenderer)
        // {
        //     if (pointerStay)
        //     {
        //         outputLineRenderer.positionCount = 2;
        //         outputLineRenderer.SetPosition(0, pointerDownPos);
        //         outputLineRenderer.SetPosition(1, Vars.Instance.input.mouseWorldPos);
        //     }
        //     else
        //     {
        //         if (nextComplex)
        //         {
        //             outputLineRenderer.positionCount = 2;
        //             outputLineRenderer.SetPosition(0, transform.position);
        //             outputLineRenderer.SetPosition(1, nextComplex.transform.position);
        //         }
        //         else
        //         {
        //             outputLineRenderer.positionCount = 0;
        //         }
        //     }
        // }
    }

    public virtual void SetTooltip()
    {
        tooltipInfoCnt.title = GetName();
        tooltipInfoCnt.desc = GetDesc();
    }

    public virtual string GetName()
    {
        return type.name;
    }
    public virtual string GetDesc()
    {
        string str = $"{type.GetDesc()}";
        str += $"Effeciency: {(int)(effeciencySystem.Effeciency * 100)}/{(int)(effeciencySystem.MaxEffeciency * 100)}\n";
        if (nextComplexes != null && nextComplexes.Count > 0)
        {
            str += "Connected With: ";
            foreach (var c in nextComplexes)
            {
                if (c != null)
                {
                    str += $"{c.type.name},";
                }
            }    
            str += "\n";
        }
        return str;
    }

    // public void OnPointerDown()
    // {
    //     if (type.canHaveNextComplex)
    //     {
    //         pointerStay = true;
    //         pointerDownPos = Vars.Instance.input.mouseWorldPos;    
    //     }
    // }
    // public void OnPointerUp(Complex c)
    // {
    //     if (type.canHaveNextComplex)
    //     {
    //         pointerStay = false;
    //         nextComplex = null;
    //         if (c != null && c != this && c.type != null && c.type.canBeNextComplex)
    //         {
    //             nextComplex = c;
    //         }
    //     }
    // }

    public virtual void OnPointerClick()
    {
    }

    public virtual float GetReceiveCount(DetailStack stack)
    {
        return 0;
    }

    public virtual void Receive(DetailStack stack) {}

    public virtual void SwitchConnecting(Complex complex)
    {
        if (complex == null || complex == this)
        {
            return;
        }
        if (nextComplexes.Contains(complex))
        {
            nextComplexes.Remove(complex);
        }
        else
        {
            if (nextComplexes.Count + 1 <= type.maxNextConnections)
            {
                nextComplexes.Add(complex);
            }
        }
    }
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
    public float MaxEffeciency => (maxEffeciency + maxEffeciencyBonus + Vars.Instance.modifiers.GetBonus<MaxEffeciencyModifier>()) * 
        (1.0f + maxEffeciencyMultiplier + Vars.Instance.modifiers.GetMultiplier<MaxEffeciencyModifier>());

    public void Init()
    {
        effeciency = 0.5f;
        maxEffeciency = 1.25f;
        effeciencyGrow = 0.01f;
    }

    public void Update()
    {
        var growBonus = Vars.Instance.modifiers.GetBonus<EffeciencyGrowModifier>();
        var growMultiplier = Vars.Instance.modifiers.GetMultiplier<EffeciencyGrowModifier>();

        effeciency += (effeciencyGrow + effeciencyGrowBonus + growBonus) * Vars.Instance.time.deltaDay * 
            (1.0f + effeciencyGrowMultiplier + growMultiplier);
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

    public Sprite sprite;

    public int maxNextConnections;

    public AudioClip idleClip;

    public Complex AsComplex()
    {
        var scr = GameObject.Instantiate(prefab);
        scr.type = this;
        return scr;
    }

    public virtual string GetName()
    {
        return name;
    }
    public virtual string GetDesc()
    {
        string str = desc;
        if (buildable)
        {
            str += $"\nBuild Time: {buildTime} days\n";
        }
        return str;
    }
}

public class ProducingComplexType : ComplexType
{
    public List<DetailStack> outputStacks;

    public override string GetDesc()
    {
        string str = $"{base.GetDesc()}";
        str += $"Output:\n";
        foreach (var stack in outputStacks)
        {
            str += $"{stack}\n";
        }
        return str;
    }
}