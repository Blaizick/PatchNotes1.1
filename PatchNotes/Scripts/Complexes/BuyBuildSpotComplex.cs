using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyBuildSpotComplex : Complex, IPointerClickHandler
{
    public TMP_Text priceText;
    public float price;

    public override void Update()
    {
        priceText.text = ((int)price).ToString();
        base.Update();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        Vars.Instance.ui.ShowConfirmDialog($"Are you sure you want to buy new build spot for {(int)price}?", () =>
        {
            if (Vars.Instance.money.HasEnought(price))
            {
                Vars.Instance.money.Take(price);
                Vars.Instance.buildSystem.BuySpot(gameObject);
            }
        }, null);
    }

    public override void SetTooltip()
    {
        tooltipInfoCnt.title = type.name;
        tooltipInfoCnt.desc = type.desc;
    }
}

public class BuildSpotPriceSystem
{
    public int boughtBuildSpots;
    public const float StartPrice = 200;
    public const float PriceModifier = 1.2f;

    public void Init()
    {
        Restart();
    }

    public void Restart()
    {
        boughtBuildSpots = 0;
    }

    public void OnBuy()
    {
        boughtBuildSpots++;
    }

    public float GetPrice()
    {
        return StartPrice * Mathf.Pow(PriceModifier, boughtBuildSpots);
    }
}