using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class Vars : MonoBehaviour
{
    public static Vars Instance {get;set;}

    public MoneySystem money;
    public IncomeSystem income;
    public OrderSystem orders;
    public UIMAin ui;
    public DetailsSystem details;
    public DesktopInput input;
    public GameStateSystem state;
    public TimeSystem time;
    public ResearchSystem researches;
    public BuildingsSystem buildSystem;
    public SpeedSystem speedSystem;
    public UnlockedDetailsSystem unlockedDetails;
    public BuildSpotPriceSystem buildSpotPrice;
    public ProductionLineColorSystem productionLineColor;
    public ChefsSystem chefs;
    public ManagersSystem managers;
    public LayerMasksSystem layerMasks;
    public InfluenceSystem influence;
    public SuppliersSystem suppliers;
    public Tooltip tooltip;
    public TaxesSystem taxes;
    public ReportsSystem reports;
    public TimeSpanUpdateSystem timeSpanUpdate;
    public ModifiersSystem modifiers;
    public DetailQualitySystem detailQualitySystem;
    public MaterialPriceSystem materialPrices;
    public CinemachineCamera cam;
    public RebindSystem rebinds;
    public EventsSystem events;

    private void Start()
    {
        float time0 = Time.realtimeSinceStartup;

        Instance = this;

        Portraits.Init();
        BuildingIcons.Init();
        Details.Init();
        Orders.Init();
        Recipes.Init();
        Complexes.Init();
        Researches.Init();
        Complexes.PostInit();
        ManagerCategory.GInit();
        ManagerType.GInit();
        Suppliers.Init();
        Chefs.Init();
        Events.Init();
        
        productionLineColor = new();
        productionLineColor.Init();

        time = new();
        time.Init();

        timeSpanUpdate = new();
        timeSpanUpdate.Init();

        input.Init();

        state = new();
        state.Init();

        money = new();
        money.Init();

        income = new();
        income.Init();

        modifiers = new();
        modifiers.Init();

        detailQualitySystem = new();
        materialPrices = new();

        influence = new();
        influence.Init();

        chefs = new();
        chefs.Init();

        details = new();
        details.Init();

        researches = new();
        researches.Init();

        speedSystem = new();
        speedSystem.Init();

        unlockedDetails = new();
        unlockedDetails.Init();

        orders = new();
        orders.Init();

        buildSpotPrice = new();
        buildSpotPrice.Init();

        managers = new();
        managers.Init();

        suppliers = new();
        suppliers.Init();

        taxes = new();
        taxes.Init();

        reports = new();
        reports.Init();

        rebinds = new();
        rebinds.Init();

        foreach (var i in FindObjectsByType<Complex>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            i.Init();
        }
        foreach (var i in FindObjectsByType<BuildSpotComplex>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            i.Init();
        }

        buildSystem.Init();
        ui.Init();
        tooltip.Init();

        events = new();
        events.Init();

        Debug.Log($"Startup Time: {Time.realtimeSinceStartup - time0}");
    }

    public void Restart()
    {
        time.Restart();
        timeSpanUpdate.Restart();
        unlockedDetails.Restart();
        state.Restart();
        money.Restart();
        income.Restart();
        influence.Restart();
        details.Restart();
        orders.Restart();
        researches.Restart();
        buildSpotPrice.Restart();
        buildSystem.Restart();
        speedSystem.Restart();
        productionLineColor.Restart();
        chefs.Restart();
        managers.Restart();
        suppliers.Restart();
        taxes.Restart();
        reports.Restart();
        modifiers.Restart();
        ui.Restart();
        events.Restart();
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
        timeSpanUpdate.Update();
        orders.Update();
        researches.Update();
        speedSystem.Update();
        unlockedDetails.Update();
        chefs.Update();
        managers.Update();
        influence.Update();
        suppliers.Update();
        taxes.Update();
        income.Update();
        reports.Update();
        materialPrices.Update();
        details.Update();
        events.Update();
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

    public bool HasEnought(float count)
    {
        return count <= money;
    }
}

public class OrderType
{
    public float time;
    public string name;

    public List<Requirement> requirements;
    public List<Punishment> punishments;
    public List<Reward> rewards;

    public Order AsOrder()
    {
        return new Order{type = this};
    }
}

public class Requirement
{
    public virtual bool CanComplete() => false;
}

public class MoneyRequirement : Requirement, IFormattable
{
    public float money;

    public override bool CanComplete() => Vars.Instance.money.HasEnought(money);

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"Money: {(int)money}";
    }
}

public class DetailRequirement : Requirement, IFormattable
{
    public DetailStack detailStack;

    public override bool CanComplete() => Vars.Instance.details.HasEnought(detailStack);

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"{detailStack.detail.name}: {(int)detailStack.count}";
    }
}

public class DayRequirement : Requirement
{
    public float day;

    public override bool CanComplete()
    {
        return Vars.Instance.time.day >= day;
    }
}

public class Punishment
{
    public virtual void Execute() {}
}

public class MoneyPunishment : Punishment, IFormattable
{
    public float money;

    public override void Execute()
    {
        Vars.Instance.money.Take(money);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"Money: {(int)money}";
    }
}

public class DetailPunishment : Punishment, IFormattable
{
    public DetailStack detailStack;

    public override void Execute()
    {
        Vars.Instance.details.Take(detailStack);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"{detailStack.detail.name}: {(int)detailStack.count}";
    }
}

public class Reward
{
    public virtual void Execute() {}
}

public class IncomeReward : Reward, IFormattable
{
    public float income;

    public override void Execute()
    {
        Vars.Instance.income.AddOrderIncome(income);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"Income: {income}";
    }
}

public class Order
{
    public float timeProgress = 0.0f;

    public OrderType type;

    public virtual bool CanComplete()
    {
        if (type.requirements == null)
        {
            return false;            
        }
        foreach (var i in type.requirements)
        {
            if (!i.CanComplete())
            {
                return false;
            }
        }
        return true;
    }
    public virtual void Complete()
    {
        if (type.punishments != null)
        {
            foreach (var i in type.punishments)
            {
                i.Execute();
            }    
        }
        if (type.rewards != null)
        {
            foreach (var i in type.rewards)
            {
                i.Execute();
            }    
        }
    }
    public virtual float DaysLeft => (1.0f - timeProgress) * type.time; 
}

public class OrderSystem
{
    public List<OrderType> awailableOrders;
    public List<Order> optionalOrders;
    
    public HashSet<DetailType> lastOrderDetails = new();
    
    public Order curOrder;
    public int curOrderId;

    public UnityEvent onChange = new();
    public UnityEvent<Order> onOptionalOrderComplete = new();

    public float lastUpdateTime;

    public void Init()
    {
        Restart();
    }
    public void Restart()
    {
        lastOrderDetails.Clear();
        awailableOrders = new();
        UpdateAwailableOrders();
        SetRequiredOrder(0);
        lastUpdateTime = 0.0f;
        optionalOrders = new();
        onChange?.Invoke();
    }

    public void Update()
    {
        curOrder.timeProgress += Vars.Instance.time.deltaDay / curOrder.type.time;
        if (curOrder.timeProgress > 1.0f)
        {
            if (curOrder.CanComplete())
            {
                if (curOrderId + 1 < Orders.requiredOrders.Count)
                {
                    curOrder.Complete();
                    SetRequiredOrder(curOrderId + 1);
                    onChange?.Invoke();
                }
                else
                {
                    if (!CheatsSystem.godMode)
                    {
                        Vars.Instance.Win();
                    }
                }
            }
            else
            {
                if (!CheatsSystem.godMode)
                {
                    Vars.Instance.Lose();
                }
            }    
        }
        
        HashSet<Order> remove = new(); 
        for (int i = 0; i < optionalOrders.Count; i++)
        {
            var order = optionalOrders[i];
            order.timeProgress += Vars.Instance.time.deltaDay /  order.type.time;
            if (order.CanComplete())
            {
                order.Complete();
                onOptionalOrderComplete?.Invoke(order);
                remove.Add(order);
            }
            if (order.timeProgress > 1)
            {
                remove.Add(order);
            }
        }
        if (remove.Count > 0)
        {
            optionalOrders.RemoveAll(i => remove.Contains(i));
            onChange?.Invoke();
        }
    }

    public void MonthUpdate()
    {
        UpdateAwailableOrders();
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
        details.RemoveAll(i => lastOrderDetails.Contains(i));
        details.Shuffle();

        lastOrderDetails.Clear();

        for (int i = 0; i < 3 && details.Count > 0; i++)
        {
            var count = (int)UnityEngine.Random.Range(5f, 100f);
            var detail = details.Last();
            lastOrderDetails.Add(detail);
            details.RemoveAt(details.Count - 1);

            awailableOrders.Add(new OrderType()
            {
                requirements = new()
                {
                    new DetailRequirement()
                    {
                        detailStack = new(detail, count)
                    },
                },
                punishments = new()
                {
                    new DetailPunishment()
                    {
                        detailStack = new(detail, count)
                    }
                },
                rewards = new()
                {
                    new IncomeReward()
                    {
                        income = detail.price * count * 1.5f,
                    }
                },
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
}

public class DetailType
{
    public string name;
    public float price;
}

public class DetailsSystem
{
    public Dictionary<DetailType, float> details;
    public Dictionary<DetailType, bool> autoSellDetails;

    public void Init()
    {   
        Restart();
    }
    public void Restart()
    {
        autoSellDetails = new();
        details = new();
    }

    public void Update()
    {
        foreach (var (k, v) in autoSellDetails)
        {
            if (v)
            {
                SellAll(k);
            }
        }
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
        if (detail == null || !details.ContainsKey(detail))
            return;
        Sell(new(detail, details[detail]));
    }

    public void Sell(DetailStack detailStack)
    {
        if (detailStack.detail == null || !details.ContainsKey(detailStack.detail))
            return;
        var d = Mathf.Min(detailStack.count, details[detailStack.detail]);
        Vars.Instance.income.AddDetailIncome(detailStack.detail, d * detailStack.detail.price * Vars.Instance.detailQualitySystem.Quality);
        details[detailStack.detail] -= d;
    }

    public void Take(DetailStack stack)
    {
        if (!details.ContainsKey(stack.detail))
            return;

        details[stack.detail] = Mathf.Clamp(details[stack.detail] - stack.count, 0, float.MaxValue);
    }

    public bool HasEnought(DetailStack stack)
    {
        return GetCount(stack.detail) >= stack.count;
    }

    public void SwithAutoSellState(DetailType detail)
    {
        if (!autoSellDetails.ContainsKey(detail))
        {
            autoSellDetails[detail] = true;
        }
        else
        {
            autoSellDetails[detail] = !autoSellDetails[detail];
        }
    }
    public void SetAutoSell(DetailType detail, bool v)
    {
        autoSellDetails[detail] = v;
    }
    public bool IsAutoSelling(DetailType detail)
    {
        return autoSellDetails.TryGetValue(detail, out var b) && b;
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
            0.5f,
            0.75f,
            1.0f,
            2.0f,
            5.0f            
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
        foreach (var (k, v) in Vars.Instance.details.details)
        {
            if (!unlockedSet.Contains(k))
            {
                unlockedSet.Add(k);
                unlocked.Add(k);
            }
        }
    }
}

public class InfluenceSystem
{
    public float influence;
    public const float MaxInfluence = 300.0f;
    public const float InfluenceGrow = 1.2f; 

    public void Init()
    {
        Restart();
    }
    public void Restart()
    {
        influence = 0.0f;
    }

    public void Update()
    {
        Add(InfluenceGrow * Vars.Instance.time.deltaDay);
    }

    public void Take(float count)
    {
        influence -= count;
    }
    public void Add(float count)
    {
        influence = Mathf.Clamp(influence + count, 0, MaxInfluence);
    }

    public bool HasEnought(float count)
    {
        return influence >= count;
    }
}

public class TaxesSystem
{
    public const float BaseIncomeTax = 0.15f;
    public float IncomeTax => BaseIncomeTax + Vars.Instance.modifiers.GetBonus<IncomeTaxModifier>();
    public float IncomeTaxInfluence => 1.0f - IncomeTax;

    public void Init()
    {
        Restart();
    }

    public void Restart()
    {
        
    }

    public void Update()
    {
        
    }
}

public class IncomeSystem
{
    public float income;

    public void Init()
    {
        Restart();
    } 
    public void Restart()
    {
        ResetIncomes();
    }

    public void Update()
    {

    }

    public void MonthUpdate()
    {
        Vars.Instance.money.Add(IncomeWithIncomeTax);
        ResetIncomes();
    }

    public void AddDetailIncome(DetailType detail, float income)
    {
        Vars.Instance.reports.cur.AddDetailIncome(detail, income);
        this.income += income;
    }
    public void AddOrderIncome(float income)
    {
        this.income += income;
        Vars.Instance.reports.cur.orders += income;
    }


    public void ResetIncomes()
    {
        income = 0.0f;
    }
    public void ExpenseByMaterial(float expense)
    {
        this.income -= expense;
        Vars.Instance.reports.cur.totalMaterialsExpense += expense;
    }

    public float IncomeWithIncomeTax
    {
        get
        {
            if (income <= 0)
            {
                return income;
            }
            return income * Vars.Instance.taxes.IncomeTaxInfluence;    
        }
    }
    public float GetIncomeTaxExpense()
    {
        if (income <= 0)
        {
            return 0;
        }
        return income - IncomeWithIncomeTax;
    }

    public void Add(float income)
    {
        this.income += income;        
    }
    public void Remove(float expense)
    {
        income -= expense;
    }
}

public class TimeSpanUpdateSystem
{
    public float lastDayUpdateTime;
    public float lastMonthUpdateTime;

    public void Init()
    {
        Restart();
    }
    public void Restart()
    {
        lastDayUpdateTime = Vars.Instance.time.day;
        lastMonthUpdateTime = Vars.Instance.time.month;
    }

    public void Update()
    {
        if (Vars.Instance.time.day - lastDayUpdateTime > 1.0f)
        {
            lastDayUpdateTime = Vars.Instance.time.day;
        }
        if (Vars.Instance.time.month - lastMonthUpdateTime > 1.0f)
        {
            Vars.Instance.reports.MonthUpdate();
            Vars.Instance.income.MonthUpdate();
            Vars.Instance.orders.MonthUpdate();

            lastMonthUpdateTime = Vars.Instance.time.month;
        }
    }
}

public class DetailQualitySystem
{
    public const float BaseValue = 1.0f;
    public float Quality => BaseValue + Vars.Instance.modifiers.GetBonus<DetailQualityModifier>();
}
public class MaterialPriceSystem
{
    public const float BaseValue = 10.0f;
    public float MaterialPrice => (BaseValue + Vars.Instance.modifiers.GetBonus<MaterialPriceModifier>()) * 
        (1 + Vars.Instance.modifiers.GetMultiplier<MaterialPriceModifier>());

    public void Update()
    {
        Vars.Instance.reports.cur.materialPrice = MaterialPrice;
    }
}

public static class CheatsSystem
{
    public static bool godMode;
}

public class Event
{
    public string name;
    public string desc;

    public List<Requirement> requirements;
    public UnityAction action;
}

public class EventsSystem
{
    public List<Event> events;

    public HashSet<Event> invokedEvents = new();

    public void Init()
    {
        events = new(Events.all);
        
        Restart();
    }

    public void Restart()
    {
        invokedEvents.Clear();
    }

    public void Update()
    {
        foreach (var e in events)
        {
            if (invokedEvents.Contains(e))
            {
                continue;
            }
            bool canInvoke = true;
            if (e.requirements != null)
            {
                foreach (var r in e.requirements)
                {
                    if (!r.CanComplete())
                    {
                        canInvoke = false;
                        break;
                    }
                }    
            }
            if (canInvoke)
            {
                invokedEvents.Add(e);
                e.action?.Invoke();
            }
        }
    }
}