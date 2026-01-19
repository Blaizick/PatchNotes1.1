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
    [NonSerialized] public List<SupplierUiCntPfb> awailableSupplierInstances = new();
    public Dictionary<SupplierType, SupplierUiCntPfb> awailableSupplierInstancesDic = new();

    public GameObject awailableSuppliersRoot;
    public Button closeAwailableSuppliersBtn;
    public RectTransform awailableSuppliersContentRootTranform;

    public Button closeBtn;

    public void Init()
    {
        root.SetActive(false);

        Vars.Instance.orders.onChange.AddListener(() => RebuildOrderMenu());
        RebuildOrderMenu();
        
        closeBtn.onClick.AddListener(() => root.SetActive(false));
    
        curSupplier.btn.onClick.AddListener(() =>
        {
            awailableSuppliersRoot.SetActive(!awailableSuppliersRoot.activeInHierarchy);
        });

        closeAwailableSuppliersBtn.onClick.AddListener(() => awailableSuppliersRoot.SetActive(false));
        Vars.Instance.suppliers.onChange.AddListener(() => RebuildSuppliersMenu());
        awailableSuppliersRoot.SetActive(false);

        RebuildSuppliersMenu();
    }

    public void Update()
    {
        foreach (var (k, v) in awailableSupplierInstancesDic)
        {
            foreach (var state in v.AllStates)
            {
                state.influencePriceText.text = ((int)k.influencePrice).ToString();
            }
            bool taken = Vars.Instance.suppliers.supplier.type == k;
            bool awailable = Vars.Instance.suppliers.IsAwailable(k);
            v.takenState.root.SetActive(taken);
            v.awailableState.root.SetActive(!taken && awailable);
            v.unawailableStates.root.SetActive(!taken && !awailable);
        }
    }

    public void RebuildOrderMenu()
    {
        instances.ForEach(i => Destroy(i.gameObject));
        instances.Clear();

        foreach (var i in Vars.Instance.orders.optionalOrders)
        {
            var script = Instantiate(orderUICntPfb, takenOrdersContentRootTransform);
            script.nameText.text = i.type.name;
            script.awailableStateRoot.SetActive(false);
            script.takenStateRoot.SetActive(true);
            instances.Add(script);
        }
        foreach (var i in Vars.Instance.orders.awailableOrders)
        {
            var script = Instantiate(orderUICntPfb, awailableOrdersContentRootTransform);
            script.nameText.text = i.name;
            script.awailableStateRoot.SetActive(true);
            script.takenStateRoot.SetActive(false);
            script.btn.onClick.AddListener(() =>
            {
                Vars.Instance.orders.TakeOptionalOrder(i);
            });
            instances.Add(script);
        }
    }

    public void RebuildSuppliersMenu()
    {
        awailableSupplierInstances.ForEach(i => Destroy(i.gameObject));
        awailableSupplierInstances.Clear();
        awailableSupplierInstancesDic.Clear();

        foreach (var supplier in Vars.Instance.suppliers.displayableSuppliers)
        {
            var script = Instantiate(supplierUiCntPfb, awailableSuppliersContentRootTranform);
            foreach (var state in script.AllStates)
            {
                state.btn.onClick.AddListener(() =>
                {
                    if (Vars.Instance.influence.HasEnought(supplier.influencePrice))
                    {
                        Vars.Instance.suppliers.Take(supplier);
                        awailableSuppliersRoot.SetActive(false);
                    }
                });
            }
            awailableSupplierInstances.Add(script);
            awailableSupplierInstancesDic[supplier] = script;
        }
    }
}

public class SupplierType
{
    public float influencePrice;
    public float materialPrice;
    public string name;

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
        onChange?.Invoke();
    }

    public void Update()
    {
        Vars.Instance.materialCostSystem.pricePerMaterial = supplier.type.materialPrice;
    }

    public void Take(SupplierType type)
    {
        Vars.Instance.influence.Take(type.influencePrice);
        supplier = type.AsSupplier();
    }

    public bool IsAwailable(SupplierType type)
    {
        return type != supplier.type && Vars.Instance.influence.HasEnought(type.influencePrice);
    }
}