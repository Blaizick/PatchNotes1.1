using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Vars : MonoBehaviour
{
    public static Vars Instance {get;set;}

    public MoneySystem moneySystem;
    public OrderSystem orders;
    public UIMAin ui;
    public DetailsSystem detailsSystem;
    public DesktopInput input;
    public GameStateSystem state;
    public TimeSystem time;
    public ResearchSystem researches;
    public BuildingsSystem buildingsSystem;
    public SpeedSystem speedSystem;
    public UnlockedDetailsSystem unlockedDetails;

    private void Start()
    {
        Instance = this;

        Details.Init();
        Orders.Init();
        Complexes.Init();
        BuildSpots.Init();
        Researches.Init();
        Complexes.PostInit();
        Recipes.Init();

        time = new();
        time.Init();

        input.Init();

        state = new();
        state.Init();

        moneySystem = new();
        moneySystem.Init();

        detailsSystem = new();
        detailsSystem.Init();

        researches = new();
        researches.Init();

        speedSystem = new();
        speedSystem.Init();

        unlockedDetails = new();
        unlockedDetails.Init();

        orders = new();
        orders.Init();

        foreach (var i in Resources.FindObjectsOfTypeAll<Complex>())
        {
            i.Init();
        }
        foreach (var i in Resources.FindObjectsOfTypeAll<BuildSpot>())
        {
            i.Init();
        }

        buildingsSystem.Init();
        ui.Init();
    }

    public void Restart()
    {
        unlockedDetails.Restart();
        state.Restart();
        moneySystem.Restart();
        detailsSystem.Restart();
        orders.Restart();
        time.Restart();
        researches.Restart();
        buildingsSystem.Restart();
        speedSystem.Restart();
    }

    public void Win()
    {
        state.state = GameState.Win;        
    }
    public void Lose()
    {
        state.state = GameState.Lose;        
    }

    private void Update()
    {
        time.Update();
        orders.Update();
        researches.Update();
        speedSystem.Update();
        unlockedDetails.Update();
    }
}

public class MoneySystem
{
    public float money;

    public void Init()
    {
        money = 1000;
    }
    public void Restart()
    {
        money = 1000;
    }

    public void Take(float count)
    {
        money -= count;
    }
    public void Add(float count)
    {
        money+= count;
    }

    public bool HaveEnoughtMoney(float count)
    {
        return count <= money;
    }
}

public static class Orders
{
    public static OrderType order0;
    public static OrderType order1;
    public static OrderType order2;
    public static OrderType order3;

    public static List<OrderType> requiredOrders;
    
    public static void Init()
    {
        order0 = new MoneyOrderType()
        {
            requiredMoney = 200,
            time = 20,
            name = "Required Order",
        };
        order1 = new MoneyOrderType()
        {
            requiredMoney = 200,
            time = 20,
            name = "Required Order",
        };
        order2 = new MoneyOrderType()
        {
            requiredMoney = 200,
            time = 20,
            name = "Required Order",
        };
        order3 = new MoneyOrderType()
        {
            requiredMoney = 200,
            time = 20,
            name = "Required Order",
        };

        requiredOrders = new()
        {
            order0, order1, order2, order3
        };
    }
}

public class OrderType
{
    public float time;
    public Type type;
    public string name;

    public Order AsOrder()
    {
        var order = (Order)Activator.CreateInstance(type);
        order.type = this;
        return order;
    }
}

public class MoneyOrderType : OrderType
{
    public float requiredMoney;

    public MoneyOrderType() : base()
    {
        type = typeof(MoneyOrder);
    }
}
public class DetailsOrderType : OrderType
{
    public List<DetailStack> requiredDetails;
    public float moneyReward;

    public DetailsOrderType() : base()
    {
        type = typeof(DetailsOrder);
    }
}

public class Order
{
    public float startTime;

    public Order()
    {
        startTime = Vars.Instance.time.day;
    }

    public OrderType type;

    public virtual bool CanComplete => false;
    public virtual void Complete() {}
}
public class MoneyOrder : Order
{
    public MoneyOrderType MoneyOrderType => (MoneyOrderType)type;

    public override bool CanComplete => Vars.Instance.moneySystem.money >= MoneyOrderType.requiredMoney;
    public override void Complete()
    {
        Vars.Instance.moneySystem.Take(MoneyOrderType.requiredMoney);
    }
}
public class DetailsOrder : Order
{
    public DetailsOrderType DetailsOrderType => (DetailsOrderType)type;

    public override bool CanComplete
    {
        get
        {
            foreach (var i in DetailsOrderType.requiredDetails)
            {
                if (!Vars.Instance.detailsSystem.HasEnought(i))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public override void Complete()
    {
        Vars.Instance.moneySystem.Add(DetailsOrderType.moneyReward);

        foreach (var i in DetailsOrderType.requiredDetails)
        {
            Vars.Instance.detailsSystem.Remove(i);
        }
    }
}

public class OrderSystem
{
    public List<OrderType> awailableOrders;
    public List<Order> optionalOrders;
    
    public Order curOrder;
    public int curOrderId;

    public UnityEvent onChange = new();

    public float lastUpdateTime;

    public void Init()
    {
        Restart();
    }
    public void Restart()
    {
        awailableOrders = new();
        UpdateAwailableOrders();
        SetRequiredOrder(0);
        lastUpdateTime = 0.0f;
        optionalOrders = new();
        onChange?.Invoke();
    }

    public void Update()
    {
        if (Vars.Instance.time.day - curOrder.startTime > curOrder.type.time)
        {
            if (curOrder.CanComplete)
            {
                if (curOrderId + 1 < Orders.requiredOrders.Count)
                {
                    curOrder.Complete();
                    SetRequiredOrder(curOrderId + 1);
                }
                else
                {
                    Vars.Instance.Win();
                }
            }
            else
            {
                Vars.Instance.Lose();
            }
        }
        if (Vars.Instance.time.day - lastUpdateTime > 20.0f)
        {
            UpdateAwailableOrders();
            lastUpdateTime = Vars.Instance.time.day;
        }

        HashSet<Order> remove = new(); 
        for (int i = 0; i < optionalOrders.Count; i++)
        {
            var order = optionalOrders[i];
            if (Vars.Instance.time.day - order.startTime > order.type.time)
            {
                if (order.CanComplete)
                {
                    order.Complete();
                }
                remove.Add(order);
            }
        }
        if (remove.Count > 0)
        {
            optionalOrders.RemoveAll(i => remove.Contains(i));
            onChange?.Invoke();
        }
    }

    public void SetRequiredOrder(int id)
    {
        curOrderId = id;
        curOrder = Orders.requiredOrders[id].AsOrder();
        onChange?.Invoke();
    }

    public void UpdateAwailableOrders()
    {
        awailableOrders.Clear();

        List<DetailType> details = new(Vars.Instance.unlockedDetails.unlocked);
        details.Shuffle();

        for (int i = 0; i < 3 && details.Count > 0; i++)
        {
            var count = (int)UnityEngine.Random.Range(5f, 100f);
            var detail = details.Last();
            details.RemoveAt(details.Count - 1);

            awailableOrders.Add(new DetailsOrderType()
            {
                requiredDetails = new()
                {
                    new(detail, count)
                },
                moneyReward = detail.price * count * 1.5f,
                name = $"Optional Order {i}",
                time = 30.0f,
            });
        }

        onChange?.Invoke();
    }

    public void TakeOptionalOrder(OrderType type)
    {
        awailableOrders.Remove(type);
        var order = type.AsOrder();
        optionalOrders.Add(order);
        onChange?.Invoke();
    }

    public float TimeLeft => curOrder.type.time - (Vars.Instance.time.day - curOrder.startTime);
}

public class DetailType
{
    public string name;
    public float price;
}

public static class Details
{
    public static DetailType ironOre;
    public static DetailType ironIngot;

    public static List<DetailType> all;

    public static void Init()
    {
        ironOre = new()
        {
            name = "Iron Ore",
            price = 10.0f            
        };
        ironIngot = new()
        {
            name = "Iron Ingot",
            price = 25.0f
        };

        all = new()
        {
            ironOre,
            ironIngot,
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
    public void Restart()
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

    public void Remove(DetailStack stack)
    {
        if (!details.ContainsKey(stack.detail))
            return;

        details[stack.detail] = Mathf.Clamp(details[stack.detail] - stack.count, 0, float.MaxValue);
    }

    public bool HasEnought(DetailStack stack)
    {
        return GetCount(stack.detail) >= stack.count;
    }
}

public enum GameState
{
    Running,
    Win,
    Lose,
}

public class GameStateSystem
{
    public GameState state;

    public void Init()
    {
        state = GameState.Running;
    }
    public void Restart()
    {
        state = GameState.Running;
    }

    public bool IsGame => state == GameState.Running;
    public bool IsWin => state == GameState.Win;
    public bool IsLose => state == GameState.Lose;
}

public class ComplexType
{
    public Complex prefab;
    public string name;    
    public ComplexResearchTech research;
}

public class BuildSpotType
{
    public List<ComplexType> buildComplexes;
}

public static class Complexes
{
    public static ComplexType supplier;
    public static ComplexType smelter;

    public static List<ComplexType> all;

    public static void Init()
    {
        supplier = new()
        {
            name = "Supplier",
            prefab = Resources.Load<Complex>("Prefabs/Supplier")  
        };
        smelter = new()
        {
            name = "Smelter",
            prefab = Resources.Load<Complex>("Prefabs/Smelter")
        };

        all = new()
        {
            supplier,
            smelter
        };
    } 

    public static void PostInit()
    {
        supplier.research = Researches.supplierTech;
        smelter.research = Researches.smelterTech;
    }
}

public static class BuildSpots
{
    public static BuildSpotType spot0;

    public static void Init()
    {
        spot0 = new()
        {
            buildComplexes = Complexes.all
        };
    }
}


public class ResearchTech
{
    /// <summary>
    /// Days to research tech
    /// </summary>
    public float researchTime;
    public string name;
}

public class ComplexResearchTech : ResearchTech
{
    public ComplexType type;
}

public static class Researches
{
    public static ComplexResearchTech supplierTech;
    public static ComplexResearchTech smelterTech;

    public static List<ComplexResearchTech> complexTechs;
    public static List<ResearchTech> all;

    public static void Init()
    {
        supplierTech = new()
        {
            researchTime = 10.0f,
            name = "Supplier",
        };
        smelterTech = new()
        {
            researchTime = 20.0f,
            name = "Smelter",
        };

        complexTechs = new()
        {
            supplierTech,
            smelterTech
        };
        all = new()
        {
            supplierTech,
            smelterTech
        };
    }    
}

public class ResearchSystem
{
    public ResearchTech research;
    public float researchStart;

    public List<ResearchTech> researched;

    public void Init()
    {
        Restart();
    }

    public void Restart()
    {
        research = null;
        researched = new()
        {
            Researches.supplierTech
        };    
    }

    public void Update()
    {
        if (research != null)
        {
            if (Vars.Instance.time.day - researchStart >= research.researchTime)
            {
                researched.Add(research);
                research = null;
            }
        }
    }

    public void StartResearch(ResearchTech tech)
    {
        researchStart = Vars.Instance.time.day;
        research = tech;
    }

    public bool IsResearched(ResearchTech tech)
    {
        return researched.Contains(tech);
    }

    public float ResearchProgress => (Vars.Instance.time.day - researchStart) / research.researchTime; 
}

public class TimeSystem
{
    public float day;
    public float sec;
    public const float SecsPerDay = 1.0f;

    public float timeScale;
    public float delta;

    public void Init()
    {
         
    } 

    public void Restart()
    {        
        sec = 0.0f;
        day = 0;
    }

    public void Update()
    {
        delta = Time.deltaTime * timeScale;
        sec += delta;
        day = sec / SecsPerDay;
    }
}


public class SpeedSystem
{
    public List<float> speeds;

    public bool pause;
    public int speedId;

    public bool tick;

    public void Init()
    {
        tick = true;

        speeds = new()
        {
            1.0f,
            2.0f,
            3.0f,
            5.0f,
            7.5f            
        };

        Restart();
    }

    public void Restart()
    {
        pause = true;
        speedId = 0;
    }

    public void Update()
    {
        if (pause)
        {
            Vars.Instance.time.timeScale = 0.0f;
        }
        else
        {
            Vars.Instance.time.timeScale = speeds[speedId];
        }
    }

    public void NextSpeed()
    {
        speedId = Mathf.Clamp(speedId + 1, 0, speeds.Count - 1);
    }
    public void PrevSpeed()
    {
        speedId = Mathf.Clamp(speedId - 1, 0, speeds.Count - 1);
    }
    public void SetSpeed(int spd)
    {
        speedId = Mathf.Clamp(spd, 0, speeds.Count - 1);
    }

    public void Pause()
    {
        pause = true;        
    }
    public void Run()
    {
        pause = false;
    }
    public void ChangePauseState()
    {
        pause = !pause;
    }
}

public class UnlockedDetailsSystem
{
    public List<DetailType> unlocked;
    public HashSet<DetailType> unlockedSet;
    
    public void Init()
    {
        Restart();
    }

    public void Restart()
    {
        unlocked = new();
        unlockedSet = new();
    }

    public void Update()
    {
        foreach (var (k, v) in Vars.Instance.detailsSystem.details)
        {
            if (!unlockedSet.Contains(k))
            {
                unlockedSet.Add(k);
                unlocked.Add(k);
            }
        }
    }
}