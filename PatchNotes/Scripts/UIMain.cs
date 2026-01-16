using System.Text;
using TMPro;
using UnityEngine;

public class UIMAin : MonoBehaviour
{
    public TMP_Text moneyText;

    public TMP_Text orderMoneyText;
    public TMP_Text orderTimeText;

    public TMP_Text detailsText;

    public RectTransform detailsRootTransform;
    public DetailUIContainerPrefab detailUIContainerPrefab;

    public void Init()
    {
        foreach (var d in Details.all)
        {
            var script = Instantiate(detailUIContainerPrefab, detailsRootTransform);
            script.nameText.text = d.name;
            script.sellAllBtn.onClick.AddListener(() =>
            {
                Vars.Instance.detailsSystem.SellAll(d);
            });
        }
    }

    public void Update()
    {
        moneyText.text = ((int)Vars.Instance.moneySystem.money).ToString();
    
        orderMoneyText.text = $"{Mathf.Clamp(Vars.Instance.moneySystem.money, 0, Vars.Instance.orderSystem.curOrder.money)}/{Vars.Instance.orderSystem.curOrder.money}";
        orderTimeText.text = $"{(int)(Vars.Instance.orderSystem.TimeLeft)} sec";
    
        StringBuilder sb = new();
        foreach (var (k, v) in Vars.Instance.detailsSystem.details)
        {
            sb.Append($"{k.name}: {(int)v}\n");
        }
        detailsText.text = sb.ToString();
    }
}