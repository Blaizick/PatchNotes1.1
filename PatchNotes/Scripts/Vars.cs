using System.Collections.Generic;
using UnityEngine;

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

        orders = new();
        orders.Init();

        researches = new();
        researches.Init();

        foreach (var i in Resources.FindObjectsOfTypeAll<Complex>())
        {
            i.Init();
        }
        foreach (var i in Resources.FindObjectsOfTypeAll<BuildSpot>())
        {
            i.Init();
        }

        ui.Init();
    }

    public void Restart()
    {
        state.Restart();
        moneySystem.Restart();
        detailsSystem.Restart();
        orders.Restart();
        time.Restart();
        researches.Restart();
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

    public void TakeMoney(float count)
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

    public static List<OrderType> orderedOrdersList;
    
    public static void Init()
    {
        order0 = new()
        {
            money = 200,
            time = 20
        };
        order1 = new()
        {
            money = 105,
            time = 10
        };
        order2 = new()
        {
            money = 105,
            time = 10
        };
        order3 = new()
        {
            money = 105,
            time = 10
        };

        orderedOrdersList = new()
        {
            order0, order1, order2, order3
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
        curOrderId = 0;
        orderStartTime = Time.time;
        curOrder = Orders.orderedOrdersList[curOrderId];
    }

    public void Restart()
    {
        curOrderId = 0;
        orderStartTime = Time.time;
        curOrder = Orders.orderedOrdersList[curOrderId];
    }

    public void Update()
    {
        if (Time.time - orderStartTime > curOrder.time)
        {
            if (Vars.Instance.moneySystem.HaveEnoughtMoney(curOrder.money))
            {
                if (curOrderId + 1 < Orders.orderedOrdersList.Count)
                {
                    Vars.Instance.moneySystem.TakeMoney(curOrder.money);
                    orderStartTime = Time.time;
                    curOrder = Orders.orderedOrdersList[++curOrderId];
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
        sec += Time.deltaTime;
        day = sec / SecsPerDay; 
    }
}