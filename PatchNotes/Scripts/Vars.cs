using System;
using System.Collections.Generic;
using System.Linq;
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
    public BuildingsSystem buildSystem;
    public SpeedSystem speedSystem;
    public UnlockedDetailsSystem unlockedDetails;
    public BuildSpotPriceSystem buildSpotPriceSystem;
    public ProductionLineColorSystem productionLineColorSystem;
    public ChefsSystem chefs;
    public BuffsSystem buffs;
    public ManagersSystem managers;
    public LayerMasksSystem layerMasks;

    private void Start()
    {
        Instance = this;

        Details.Init();
        Orders.Init();
        Recipes.Init();
        Complexes.Init();
        BuildSpots.Init();
        Researches.Init();
        Complexes.PostInit();
        ManagerCategory.GInit();
        ManagerType.GInit();
        
        productionLineColorSystem = new();
        productionLineColorSystem.Init();

        time = new();
        time.Init();

        input.Init();

        state = new();
        state.Init();

        moneySystem = new();
        moneySystem.Init();

        buffs = new();
        buffs.Init();

        chefs = new();
        chefs.Init();

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

        buildSpotPriceSystem = new();
        buildSpotPriceSystem.Init();

        managers = new();
        managers.Init();

        foreach (var i in FindObjectsByType<Complex>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            i.Init();
        }
        foreach (var i in FindObjectsByType<BuildSpot>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            i.Init();
        }

        buildSystem.Init();
        ui.Init();
    }

    public void Restart()
    {
        time.Restart();
        unlockedDetails.Restart();
        state.Restart();
        moneySystem.Restart();
        detailsSystem.Restart();
        orders.Restart();
        researches.Restart();
        buildSystem.Restart();
        speedSystem.Restart();
        buildSpotPriceSystem.Restart();
        productionLineColorSystem.Restart();
        chefs.Restart();
        buffs.Restart();
        managers.Restart();
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
        chefs.Update();
        buffs.Update();
        managers.Update();
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

    public bool HasEnoughtMoney(float count)
    {
        return count <= money;
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

public class CraftingComplexType : ComplexType
{
    public CraftRecipe recipe;
}

public class BuildSpotType
{
}


public class ResearchTech
{
    /// <summary>
    /// Days to research tech
    /// </summary>
    public float researchTime;
    public string name;
    public List<ResearchTech> requiredTechs;
    public List<ResearchTech> exclusive;

    public virtual void Research() {}
}

public class ComplexResearchTech : ResearchTech
{
    public ComplexType type;
}

public class BuffsResearchTech : ResearchTech
{
    public float maxEffeciencyBonus;
    public float effeciencyGrowBonus;
    public float maxEffeciencyMultiplier;
    public float effeciencyGrowMultiplier;

    public override void Research()
    {
        Vars.Instance.buffs.maxEffeciencyBonus += maxEffeciencyBonus;
        Vars.Instance.buffs.maxEffeciencyMultiplier += maxEffeciencyMultiplier;
        Vars.Instance.buffs.effeciencyGrowBonus += effeciencyGrowBonus;
        Vars.Instance.buffs.effeciencyGrowMultiplier += effeciencyGrowMultiplier;
    }
}

public class BuffsSystem
{
    public float maxEffeciencyBonus;
    public float maxEffeciencyMultiplier;
    public float effeciencyGrowBonus;
    public float effeciencyGrowMultiplier;

    public void Init()
    {
        Restart();
    }

    public void Restart()
    {
        maxEffeciencyBonus = 0.0f;
        maxEffeciencyMultiplier = 0.0f;
        effeciencyGrowBonus = 0.0f;
        effeciencyGrowMultiplier = 0.0f;
    }

    public void Update()
    {
        
    }
}

public class ResearchSystem
{
    public ResearchTech research;
    public float researchStart;

    public List<ResearchTech> awailableTechs = new();
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
            Researches.supplier
        };    
        UpdateAwailableResearches();
    }

    public void Update()
    {
        if (research != null)
        {
            if (Vars.Instance.time.day - researchStart >= research.researchTime)
            {
                research.Research();
                researched.Add(research);
                research = null;
                UpdateAwailableResearches();
            }
        }
    }

    public void UpdateAwailableResearches()
    {
        awailableTechs.Clear();
        foreach (var tech in Researches.all)
        {
            if (!researched.Contains(tech))
            {
                if (tech.requiredTechs != null)
                {
                    bool can = true;
                    foreach (var req in tech.requiredTechs)
                    {
                        if (!researched.Contains(req))
                        {
                            can = false;
                            break;
                        }
                    }
                    if (!can)
                    {
                        continue;
                    }
                }
                if (tech.exclusive != null)
                {
                    bool can = true;
                    foreach (var exc in tech.exclusive)
                    {
                        if (researched.Contains(exc))
                        {
                            can = false;
                            break;
                        }
                    }
                    if (!can)
                    {
                        continue;
                    }
                }
                awailableTechs.Add(tech);
            }
        }
    }

    public bool IsResearched(ResearchTech tech)
    {
        return researched.Contains(tech);
    }

    public void StartResearch(ResearchTech tech)
    {
        researchStart = Vars.Instance.time.day;
        research = tech;
    }

    public bool CanStartResearch(ResearchTech tech)
    {
        return awailableTechs.Contains(tech);
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
    public float deltaDay;

    public const float DaysPerMonth = 30.0f;
    public float month;

    public void Init()
    {
         
    } 

    public void Restart()
    {        
        sec = 0.0f;
        day = 0;
        month = 0.0f;
    }

    public void Update()
    {
        delta = Time.deltaTime * timeScale;
        deltaDay = delta / SecsPerDay;
        sec += delta;
        day = sec / SecsPerDay;
        month = day / DaysPerMonth;
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