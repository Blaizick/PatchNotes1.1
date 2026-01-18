using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


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

    public virtual bool IsChefAllowed => true;

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

    public void OnPointerDown()
    {
        pointerStay = true;
        pointerDownPos = Vars.Instance.input.mouseWorldPos;
    }
    public void OnPointerUp(Complex c)
    {
        pointerStay = false;
        nextComplex = null;
        if (c != this)
        {
            nextComplex = c;
        }
    }

    public virtual void Receive(DetailStack stack){}

    public void OnPointerClick()
    {
        Vars.Instance.ui.ShowConfirmDialog($"Destroy {type.name}?", () =>
        {
            Vars.Instance.buildSystem.DestroyBuild(this);
        }, null);
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
    public float MaxEffeciency => (maxEffeciency + maxEffeciencyBonus + Vars.Instance.buffs.maxEffeciencyBonus) * 
        (1.0f + maxEffeciencyMultiplier + Vars.Instance.buffs.maxEffeciencyMultiplier);

    public void Init()
    {
        effeciency = 0.5f;
        maxEffeciency = 1.25f;
        effeciencyGrow = 0.01f;
    }

    public void Update()
    {
        effeciency += (effeciencyGrow + effeciencyGrowBonus + Vars.Instance.buffs.effeciencyGrowBonus) * Vars.Instance.time.deltaDay * 
            (1.0f + effeciencyGrowMultiplier + Vars.Instance.buffs.effeciencyGrowMultiplier);
        effeciency = Mathf.Clamp(effeciency, 0, MaxEffeciency);
    
        maxEffeciencyMultiplier = 0.0f;
        effeciencyGrowMultiplier = 0.0f;

        maxEffeciencyBonus = 0.0f;
        effeciencyGrowBonus = 0.0f;
    }
}