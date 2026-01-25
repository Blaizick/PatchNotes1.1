using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CooperationUi : MonoBehaviour
{
    public GameObject root;
    public RectTransform awailableOrdersContentRootTransform;
    public RectTransform takenOrdersContentRootTransform;
    public Button ordersBtn;
    public OrderUICntPfb orderUICntPfb;
    [NonSerialized] public List<OrderUICntPfb> instances = new();

    public SupplierUiCntPfb supplierUiCntPfb;
    public SupplierUiState curSupplier;
    public TooltipInfoCnt supplierTooltipInfoCnt;
    [NonSerialized] public List<SupplierUiCntPfb> awailableSupplierInstances = new();
    public Dictionary<SupplierType, SupplierUiCntPfb> awailableSuppliersDic = new();

    public GameObject awailableSuppliersRoot;
    public Button closeAwailableSuppliersBtn;
    public RectTransform awailableSuppliersContentRootTranform;

    public Dictionary<Order, OrderUICntPfb> takenOrdersDic = new();
    public Dictionary<OrderType, OrderUICntPfb> awailableOrdersDic = new();

    public Button closeBtn;

    public void Init()
    {
        root.SetActive(false);

        Vars.Instance.orders.onChange.AddListener(() => RebuildOrderMenu());
        RebuildOrderMenu();
        
        closeBtn.onClick.AddListener(() => 
        {
            Vars.Instance.audioManager.Play(Sounds.uiClick);
            root.SetActive(false);
        });
    
        curSupplier.btn.onClick.AddListener(() =>
        {
            awailableSuppliersRoot.SetActive(!awailableSuppliersRoot.activeInHierarchy);
            Vars.Instance.audioManager.Play(Sounds.uiClick);
        });

        closeAwailableSuppliersBtn.onClick.AddListener(() => 
        {
            Vars.Instance.audioManager.Play(Sounds.uiClick);
            awailableSuppliersRoot.SetActive(false);
        });
        Vars.Instance.suppliers.onChange.AddListener(() => RebuildSuppliersMenu());
        awailableSuppliersRoot.SetActive(false);

        RebuildSuppliersMenu();

        Vars.Instance.orders.onOptionalOrderComplete.AddListener(o =>
        {
            Vars.Instance.ui.popups.ShowPopup("Order Comleted", $"{o.type.name}", null, new()
            {
                new()
                {
                    name = "OK",
                },
            });
        });
    }

    public void Update()
    {
        foreach (var (k, v) in takenOrdersDic)
        {
            v.timeLeftFiller.fillAmount = 1.0f - k.timeProgress;
            v.timeLeftText.text = $"{(int)k.DaysLeft} days";

            v.tooltipInfoCnt.title = k.type.name;
            v.tooltipInfoCnt.desc = $"Time: {k.type.time} days\n\n";

            if (k.type.requirements != null && k.type.requirements.Count > 0)
            {
                v.tooltipInfoCnt.desc += $"Requirements:\n";
                foreach (var r in k.type.requirements)
                {
                    v.tooltipInfoCnt.desc += $"{r}\n";
                }    
            }
            if (k.type.punishments != null && k.type.punishments.Count > 0)
            {
                v.tooltipInfoCnt.desc += $"Punishments:\n";
                foreach (var r in k.type.punishments)
                {
                    v.tooltipInfoCnt.desc += $"{r}\n";
                }    
            }
            if (k.type.rewards != null && k.type.rewards.Count > 0)
            {
                v.tooltipInfoCnt.desc += $"Rewards:\n";
                foreach (var r in k.type.rewards)
                {
                    v.tooltipInfoCnt.desc += $"{r}\n";
                }    
            }
        }
        foreach (var (k, v) in awailableOrdersDic)
        {
            v.tooltipInfoCnt.title = k.name;

            v.tooltipInfoCnt.desc = $"Time: {k.time} days\n\n";
            if (k.requirements != null && k.requirements.Count > 0)
            {
                v.tooltipInfoCnt.desc += $"Requirements:\n";
                foreach (var r in k.requirements)
                {
                    v.tooltipInfoCnt.desc += $"{r}\n";
                }    
            }
            if (k.punishments != null && k.punishments.Count > 0)
            {
                v.tooltipInfoCnt.desc += $"Punishments:\n";
                foreach (var r in k.punishments)
                {
                    v.tooltipInfoCnt.desc += $"{r}\n";
                }    
            }
            if (k.rewards != null && k.rewards.Count > 0)
            {
                v.tooltipInfoCnt.desc += $"Rewards:\n";
                foreach (var r in k.rewards)
                {
                    v.tooltipInfoCnt.desc += $"{r}\n";
                }    
            }
        }

        foreach (var (k, v) in awailableSuppliersDic)
        {
            SetSupplierTooltip(v.tooltipInfoCnt, k);

            foreach (var state in v.AllStates)
            {
                state.image.sprite = k.sprite;
                state.influencePriceText.text = ((int)k.influencePrice).ToString();
            }
            bool taken = Vars.Instance.suppliers.supplier.type == k;
            bool awailable = Vars.Instance.suppliers.IsAwailable(k);
            v.takenState.root.SetActive(taken);
            v.awailableState.root.SetActive(!taken && awailable);
            v.unawailableStates.root.SetActive(!taken && !awailable);
        }

        curSupplier.image.sprite = Vars.Instance.suppliers.supplier.type.sprite;
        SetSupplierTooltip(supplierTooltipInfoCnt, Vars.Instance.suppliers.supplier.type);
    }

    private void SetSupplierTooltip(TooltipInfoCnt tooltipInfoCnt, SupplierType supplier)
    {
        tooltipInfoCnt.title = supplier.name;
        tooltipInfoCnt.desc = string.Empty;
        foreach (var m in supplier.modifiers)
        {
            if (m.IsInflucing())
            {
                tooltipInfoCnt.desc += $"{m}";
            }
        }
        tooltipInfoCnt.desc += $"Price: {(int)Mathf.Clamp(Vars.Instance.influence.influence, 0, supplier.influencePrice)}/{supplier.influencePrice} influence\n";
    }

    public void RebuildOrderMenu()
    {
        takenOrdersDic.Clear();
        awailableOrdersDic.Clear();
        instances.ForEach(i => Destroy(i.gameObject));
        instances.Clear();

        if (Vars.Instance.orders.curOrder != null)
        {
            SpawnTakenOrder(Vars.Instance.orders.curOrder);
        }
        foreach (var i in Vars.Instance.orders.optionalOrders)
        {
            SpawnTakenOrder(i);
        }
        foreach (var i in Vars.Instance.orders.awailableOrders)
        {
            var script = Instantiate(orderUICntPfb, awailableOrdersContentRootTransform);
            script.nameText.text = i.name;
            script.awailableStateRoot.SetActive(true);
            script.takenStateRoot.SetActive(false);
            script.btn.onClick.AddListener(() =>
            {
                Vars.Instance.audioManager.Play(Sounds.uiClick);
                Vars.Instance.orders.TakeOptionalOrder(i);
            });
            instances.Add(script);
            awailableOrdersDic[i] = script;
        }
    }

    public void SpawnTakenOrder(Order order)
    {
        var script = Instantiate(orderUICntPfb, takenOrdersContentRootTransform);
        script.nameText.text = order.type.name;
        script.awailableStateRoot.SetActive(false);
        script.takenStateRoot.SetActive(true);
        takenOrdersDic[order] = script;
        instances.Add(script);
    }


    public void RebuildSuppliersMenu()
    {
        awailableSupplierInstances.ForEach(i => Destroy(i.gameObject));
        awailableSupplierInstances.Clear();
        awailableSuppliersDic.Clear();

        foreach (var supplier in Vars.Instance.suppliers.displayableSuppliers)
        {
            var script = Instantiate(supplierUiCntPfb, awailableSuppliersContentRootTranform);
            foreach (var state in script.AllStates)
            {
                state.btn.onClick.AddListener(() =>
                {
                    Vars.Instance.audioManager.Play(Sounds.uiClick);
                    if (Vars.Instance.influence.HasEnought(supplier.influencePrice))
                    {
                        Vars.Instance.suppliers.Take(supplier);
                        awailableSuppliersRoot.SetActive(false);
                    }
                });
            }
            awailableSupplierInstances.Add(script);
            awailableSuppliersDic[supplier] = script;
        }
    }
}

public class SupplierType
{
    public List<Modifier> modifiers;
    public float influencePrice;
    public string name;
    public Sprite sprite;

    public Supplier AsSupplier()
    {
        return new Supplier()
        {
            type = this
        };
    }
}

public class Supplier
{
    public SupplierType type;

    public void OnTake()
    {
        foreach (var i in type.modifiers)
        {
            Vars.Instance.modifiers.AddModifier(i);
        }
    }
    public void OnLeave()
    {
        foreach (var i in type.modifiers)
        {
            Vars.Instance.modifiers.RemoveModifier(i);
        }
    }
}

public class SuppliersSystem
{
    public Supplier supplier;
    public List<SupplierType> displayableSuppliers;

    public UnityEvent onChange = new();

    public void Init()
    {
        Restart();
    }
    public void Restart()
    {
        displayableSuppliers = Suppliers.all;
        supplier = Suppliers.baseSupplier.AsSupplier();
        supplier.OnTake();
        onChange?.Invoke();
    }

    public void Update()
    {

    }

    public void Take(SupplierType type)
    {
        supplier.OnLeave();
        Vars.Instance.influence.Take(type.influencePrice);
        supplier = type.AsSupplier();
        supplier.OnTake();
    }

    public bool IsAwailable(SupplierType type)
    {
        return type != supplier.type && Vars.Instance.influence.HasEnought(type.influencePrice);
    }
}