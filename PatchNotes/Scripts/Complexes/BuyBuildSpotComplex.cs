using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyBuildSpotComplex : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text priceText;
    public float price;

    public void Init()
    {
        
    }

    public void Update()
    {
        priceText.text = ((int)price).ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        Vars.Instance.ui.ShowConfirmDialog($"Are you sure you want to buy new build spot for {(int)price}?", () =>
        {
            if (Vars.Instance.moneySystem.HasEnought(price))
            {
                Vars.Instance.moneySystem.Take(price);
                Vars.Instance.buildSystem.BuySpot(gameObject);
            }
        }, null);
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