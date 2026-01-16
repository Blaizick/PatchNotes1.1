using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Vars : MonoBehaviour
{
    public static Vars Instance {get;set;}

    public MoneySystem moneySystem;
    public OrderSystem orderSystem;
    public UIMAin ui;
    public DetailsSystem detailsSystem;

    private void Start()
    {
        Instance = this;

        Details.Init();
        Orders.Init();

        moneySystem = new();
        moneySystem.Init();

        detailsSystem = new();
        detailsSystem.Init();

        orderSystem = new();
        orderSystem.Init();
        
        foreach (var i in Resources.FindObjectsOfTypeAll<ProductionComplex>())
        {
            i.Init();
        }

        ui.Init();
    }

    private void Update()
    {
        orderSystem.Update();
        
    }
}

public class MoneySystem
{
    public float money;

    public void Init()
    {
        money = 1000;
    }

    public void TakeMoney(float count)
    {
        money -= count;
    }
    public void Add(float count)
    {
        money+= count;
    }
}

public static class Orders
{
    public static OrderType order0;
    public static OrderType order1;

    public static List<OrderType> ordersOrderList;
    
    public static void Init()
    {
        order0 = new()
        {
            money = 500,
            time = 10
        };
        order1 = new()
        {
            money = 500,
            time = 10
        };

        ordersOrderList = new()
        {
            order0, order1
        };
    }
}

public class OrderType
{
    public float money;
    public float time;
}

public class OrderSystem
{
    public OrderType curOrder;
    public int curOrderId;

    public float orderStartTime;
    
    public void Init()
    {
        orderStartTime = Time.time;
        curOrder = Orders.order0;
        curOrderId = 0;
    }

    public void Update()
    {
        if (Time.time - orderStartTime > curOrder.time)
        {
            Vars.Instance.moneySystem.TakeMoney(curOrder.money);
            orderStartTime = Time.time;
            curOrder = Orders.ordersOrderList[++curOrderId];
        }
    }

    public float TimeLeft => curOrder.time - (Time.time - orderStartTime);
}

public class DetailType
{
    public string name;
    public float price;
}

public static class Details
{
    public static DetailType ironPlate;

    public static List<DetailType> all;

    public static void Init()
    {
        ironPlate = new()
        {
            name = "Iron plate",
            price = 10.0f
        };

        all = new()
        {
            ironPlate
        };
    }
}
public class DetailsSystem
{
    public Dictionary<DetailType, float> details;

    public void Init()
    {
        details = new();
    }

    public float GetCount(DetailType detail)
    {
        return details.TryGetValue(detail, out var count) ? count : 0;
    }

    public void Add(DetailType detail, float count)
    {
        if (details.ContainsKey(detail))
            details[detail] += count;
        else 
            details[detail] = count;
    }

    public void SellAll(DetailType detail)
    {
        if (!details.ContainsKey(detail))
            return;

        Vars.Instance.moneySystem.Add(details[detail] * detail.price);
        details[detail] = 0;
    }
}