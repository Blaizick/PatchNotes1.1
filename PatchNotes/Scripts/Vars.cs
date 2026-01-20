using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Mono.Cecil.Cil;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class Vars : MonoBehaviour
{
    public static Vars Instance {get;set;}

    public MoneySystem moneySystem;
    public IncomeSystem income;
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
    public InfluenceSystem influence;
    public SuppliersSystem suppliers;
    public Tooltip tooltip;
    public TaxesSystem taxes;
    public ReportsSystem reports;
    public TimeSpanUpdateSystem timeSpanUpdate;
    public ModifiersSystem modifiers;
    public DetailQualitySystem detailQualitySystem;
    public MaterialPriceSystem materialPriceSystem;

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
        Suppliers.Init();
        
        productionLineColorSystem = new();
        productionLineColorSystem.Init();

        time = new();
        time.Init();

        timeSpanUpdate = new();
        timeSpanUpdate.Init();

        input.Init();

        state = new();
        state.Init();

        moneySystem = new();
        moneySystem.Init();

        income = new();
        income.Init();

        modifiers = new();
        modifiers.Init();

        detailQualitySystem = new();
        materialPriceSystem = new();

        influence = new();
        influence.Init();

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

        suppliers = new();
        suppliers.Init();

        taxes = new();
        taxes.Init();

        reports = new();
        reports.Init();

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
        tooltip.Init();
    }

    public void Restart()
    {
        time.Restart();
        timeSpanUpdate.Restart();
        unlockedDetails.Restart();
        state.Restart();
        moneySystem.Restart();
        income.Restart();
        influence.Restart();
        detailsSystem.Restart();
        orders.Restart();
        researches.Restart();
        buildSpotPriceSystem.Restart();
        buildSystem.Restart();
        speedSystem.Restart();
        productionLineColorSystem.Restart();
        chefs.Restart();
        buffs.Restart();
        managers.Restart();
        suppliers.Restart();
        taxes.Restart();
        reports.Restart();
        modifiers.Restart();
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
        buffs.Update();
        managers.Update();
        influence.Update();
        suppliers.Update();
        taxes.Update();
        income.Update();
        reports.Update();
        materialPriceSystem.Update();
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

    public List<OrderRequirement> requirements;
    public List<OrderPunishment> punishments;
    public List<OrderReward> rewards;

    public Order AsOrder()
    {
        return new Order{type = this};
    }
}

public class OrderRequirement
{
    public virtual bool CanComplete() => false;
}

public class MoneyOrderRequirement : OrderRequirement, IFormattable
{
    public float money;

    public override bool CanComplete() => Vars.Instance.moneySystem.HasEnought(money);

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"Money: {(int)money}";
    }
}

public class DetailOrderRequirement : OrderRequirement, IFormattable
{
    public DetailStack detailStack;

    public override bool CanComplete() => Vars.Instance.detailsSystem.HasEnought(detailStack);

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"{detailStack.detail.name}: {(int)detailStack.count}";
    }
}

public class OrderPunishment
{
    public virtual void Execute() {}
}

public class MoneyOrderPunishment : OrderPunishment, IFormattable
{
    public float money;

    public override void Execute()
    {
        Vars.Instance.moneySystem.Take(money);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"Money: {(int)money}";
    }
}

public class OrderDetailPunishment : OrderPunishment, IFormattable
{
    public DetailStack detailStack;

    public override void Execute()
    {
        Vars.Instance.detailsSystem.Take(detailStack);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"{detailStack.detail.name}: {(int)detailStack.count}";
    }
}

public class OrderReward
{
    public virtual void Execute() {}
}

public class OrderIncomeReward : OrderReward, IFormattable
{
    public float income;

    public override void Execute()
    {
        Vars.Instance.income.Add(income);
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
            if (order.timeProgress > 1.0f)
            {
                if (order.CanComplete())
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
        details.Shuffle();

        for (int i = 0; i < 3 && details.Count > 0; i++)
        {
            var count = (int)UnityEngine.Random.Range(5f, 100f);
            var detail = details.Last();
            details.RemoveAt(details.Count - 1);

            awailableOrders.Add(new OrderType()
            {
                requirements = new()
                {
                    new DetailOrderRequirement()
                    {
                        detailStack = new(detail, count)
                    },
                },
                punishments = new()
                {
                    new OrderDetailPunishment()
                    {
                        detailStack = new(detail, count)
                    }
                },
                rewards = new()
                {
                    new OrderIncomeReward()
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
    public float buildTime;
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
    public float researchSpeedBonus;

    public override void Research()
    {
        Vars.Instance.buffs.maxEffeciencyBonus += maxEffeciencyBonus;
        Vars.Instance.buffs.maxEffeciencyMultiplier += maxEffeciencyMultiplier;
        Vars.Instance.buffs.effeciencyGrowBonus += effeciencyGrowBonus;
        Vars.Instance.buffs.effeciencyGrowMultiplier += effeciencyGrowMultiplier;
        Vars.Instance.buffs.researchSpeedBonus += researchSpeedBonus;
    }
}

public class BuffsSystem
{
    public float maxEffeciencyBonus;
    public float maxEffeciencyMultiplier;
    public float effeciencyGrowBonus;
    public float effeciencyGrowMultiplier;

    public float researchSpeedBonus;

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

        researchSpeedBonus = 0.0f;
    }

    public void Update()
    {
        
    }
}

public class ResearchSystem
{
    public ResearchTech research;
    public float researchProgress;
    public float savedResearchTime;
    public const float MaxSavedResearchTime = 30.0f;

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
            Researches.supply,
            Researches.smelting
        };    
        UpdateAwailableResearches();
        savedResearchTime = 0.0f;
    }

    public void Update()
    {
        if (research == null)
        {
            savedResearchTime += Vars.Instance.time.deltaDay;
            savedResearchTime = Mathf.Clamp(savedResearchTime, 0, MaxSavedResearchTime);
        }
        else
        {
            researchProgress += TimeAsProgress(Vars.Instance.time.deltaDay);
            if (researchProgress > 1.0f)
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
        research = tech;
        researchProgress = TimeAsProgress(savedResearchTime);
        savedResearchTime = 0.0f;
    }

    public bool CanStartResearch(ResearchTech tech)
    {
        return awailableTechs.Contains(tech);
    }

    public float TimeAsProgress(float t)
    {
        return t / research.researchTime * (1 + Vars.Instance.buffs.researchSpeedBonus);
    }

    public float DaysLeft => (1.0f - researchProgress) * research.researchTime / (1 + Vars.Instance.buffs.researchSpeedBonus);
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

public class InfluenceSystem
{
    public float influence;
    public const float MaxInfluence = 2000.0f;
    public const float InfluenceGrow = 2.0f; 

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
    public float IncomeTax => BaseIncomeTax;
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
        Vars.Instance.moneySystem.Add(GetIncomeWithIncomeTax());
        ResetIncomes();
    }

    public void AddDetailIncome(DetailType detail, float income)
    {
        Vars.Instance.reports.cur.AddDetailIncome(detail, income);
        this.income += income;
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

    public float GetIncomeWithIncomeTax()
    {
        if (income <= 0)
        {
            return income;
        }
        return income * Vars.Instance.taxes.IncomeTaxInfluence;
    }
    public float GetIncomeTaxExpense()
    {
        if (income <= 0)
        {
            return 0;
        }
        return income - GetIncomeWithIncomeTax();
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

public class Modifier
{
    public virtual void Apply() {}     
    public virtual void Cancel() {}     

    public bool IsInflucing() => true;
}

public interface IBonus
{
    public float Bonus {get;set;}        
}
public interface IMultiplier
{
    public float Multiplier {get;set;}        
}

public class DetailQualityModifier : Modifier, IBonus, IFormattable
{
    public float Bonus { get; set; }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"Detail quality: {(Bonus < 0 ? "-" : "+")}{(int)(Mathf.Abs(Bonus) * 100)}%";
    }
}
public class MaterialPriceModifier : Modifier, IBonus, IMultiplier, IFormattable
{
    public float Bonus { get; set; }
    public float Multiplier { get; set; }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        string str = string.Empty;
        if (Bonus != 0)
        {
            str += "Material Price: ";
            str += Bonus > 0 ? "+" : "-";
            str += Mathf.Abs(Bonus);
            if (Multiplier != 0)
            {
                str += "\n";
            }
        }
        if (Multiplier != 0)
        {
            str += "Material Price: ";
            str += Multiplier > 0 ? "+" : "-";
            str += Mathf.Abs(Multiplier);
            str += "%";
        }
        return str;
    }
}
public class EffeciencyGrowModifier : Modifier, IBonus, IFormattable
{
    public float Bonus { get; set; }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        string str = string.Empty;
        if (Bonus != 0)
        {
            
        }
        return str;
    }
}
public class MaxEffeciencyModifier : Modifier, IBonus
{
    public float Bonus { get; set; }
}

public class ModifiersSystem
{
    public Dictionary<Type, List<Modifier>> modifiers = new();

    public void Init()
    {
        Restart();
    }

    public void Restart()
    {
        modifiers.Clear();
    }

    public List<Modifier> GetModifiers(Type modifierType)
    {
        return modifiers.TryGetValue(modifierType, out var l) ? l : new();
    }
    public List<Modifier> GetModifiers<T>() => GetModifiers(typeof(T));

    public void AddModifier(Modifier modifier)
    {
        List<Modifier> l;
        if (!modifiers.TryGetValue(modifier.GetType(), out l))
        {
            l = new();
            modifiers[modifier.GetType()] = l;
        }
        l.Add(modifier);        
    }
    public void RemoveModifier(Modifier m)
    {
        if (modifiers.TryGetValue(m.GetType(), out var l))
        {
            l.Remove(m);
        }
    }

    public float GetBonus<T>()
    {
        float total = 0;
        foreach (var m in GetModifiers<T>())
        {
            if (m is IBonus bm)
            {
                total += bm.Bonus; 
            }
        }
        return total;
    }
    public float GetMultiplier<T>()
    {
        float total = 0;
        foreach (var m in GetModifiers<T>())
        {
            if (m is IMultiplier bm)
            {
                total += bm.Multiplier; 
            }
        }
        return total;
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