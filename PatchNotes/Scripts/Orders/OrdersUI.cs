using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrdersUI : MonoBehaviour
{
    public GameObject ordersMenuRoot;
    public RectTransform takenOrdersContentRootTransform;
    public RectTransform awailableOrdersContentRootTransform;
    public Button ordersBtn;
    public OrderUICntPfb orderUICntPfb;
    [NonSerialized] public List<OrderUICntPfb> takenOrderInstances = new();
    [NonSerialized] public List<OrderUICntPfb> awailableOrderInstances = new();

    public GameObject takeOrderDialogRoot;
    public RequirementUICntPfb requirementUICntPfb;
    public RectTransform requirementsRootTransform;
    public RectTransform rewardsRootTransform;
    [NonSerialized] public List<RequirementUICntPfb> requirementInstances = new();
    [NonSerialized] public List<RequirementUICntPfb> rewardInstances = new();
    public Button takeOrderAcceptButton;
    public Button takeOrderBackButton;
    public TMP_Text takeOrderNameText;

    public void Init()
    {
        ordersMenuRoot.SetActive(false);
        takeOrderDialogRoot.SetActive(false);

        Vars.Instance.orders.onChange.AddListener(() => RebuildOrderMenu());
        RebuildOrderMenu();
        
        takeOrderBackButton.onClick.AddListener(() => takeOrderDialogRoot.SetActive(false));
    }

    public void Update()
    {
        
    }

    public void RebuildOrderMenu()
    {
        takenOrderInstances.ForEach(i => Destroy(i.gameObject));
        takenOrderInstances.Clear();
        awailableOrderInstances.ForEach(i => Destroy(i.gameObject));
        awailableOrderInstances.Clear();

        takeOrderDialogRoot.SetActive(false);

        foreach (var i in Vars.Instance.orders.optionalOrders)
        {
            takenOrderInstances.Add(InstantiateOrder(i.type, takenOrdersContentRootTransform, true));
        }
        foreach (var i in Vars.Instance.orders.awailableOrders)
        {
            awailableOrderInstances.Add(InstantiateOrder(i, awailableOrdersContentRootTransform, false));
        }

        OrderUICntPfb InstantiateOrder(OrderType order, RectTransform root, bool info)
        {
            var script = Instantiate(orderUICntPfb, root);
            script.btn.onClick.AddListener(() =>
            {
                requirementInstances.ForEach(r => Destroy(r.gameObject));
                requirementInstances.Clear();
                rewardInstances.ForEach(r => Destroy(r.gameObject));
                rewardInstances.Clear();

                var detailOrder = (DetailsOrderType)order;

                foreach (var stack in detailOrder.requiredDetails)
                {
                    var requirementScr = Instantiate(requirementUICntPfb, requirementsRootTransform);
                    requirementScr.countText.text = stack.count.ToString();

                    requirementInstances.Add(requirementScr);
                }

                var rewardScr = Instantiate(requirementUICntPfb, rewardsRootTransform);
                rewardScr.countText.text = detailOrder.moneyReward.ToString();
                
                rewardInstances.Add(rewardScr);

                takeOrderAcceptButton.gameObject.SetActive(!info);
                takeOrderAcceptButton.onClick.RemoveAllListeners();
                takeOrderAcceptButton.onClick.AddListener(() =>
                {
                    takeOrderDialogRoot.SetActive(false);
                    Vars.Instance.orders.TakeOptionalOrder(order);
                });

                takeOrderNameText.text = order.name;

                takeOrderDialogRoot.SetActive(true);
            });
            script.nameText.text = order.name;
            return script;
        }
    }
}