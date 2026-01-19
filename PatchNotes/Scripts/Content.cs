using System.Collections.Generic;
using UnityEngine;

public static class Details
{
    public static DetailType ironOre;
    public static DetailType ironIngot;
    public static DetailType ironPlate;
    public static DetailType rod;
    public static DetailType screws;

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
            price = 20.0f
        };
        ironPlate = new()
        {
            name = "Iron Plate",
            price = 50.0f,
        };
        rod = new()
        {
            name = "Rod",
            price = 40.0f,
        };
        screws = new()
        {
            name = "Screws",
            price = 20.0f,
        };

        all = new()
        {
            ironOre,
            ironIngot,
            ironPlate,
            rod,
            screws
        };
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

public static class Recipes
{
    public static CraftRecipe smeltRecipe;
    public static CraftRecipe pressRecipe;
    public static CraftRecipe bendRodRecipe;
    public static CraftRecipe cutScrewsRecipe;

    public static List<CraftRecipe> all;

    public static void Init()
    {
        smeltRecipe = new()
        {
            craftTime = 0.5f,
            inputStacks = new()
            {
                new(Details.ironOre, 1.0f)
            },
            outputStacks = new()
            {
                new(Details.ironIngot, 1.0f)
            },
        };
        pressRecipe = new()
        {
            craftTime = 0.5f,
            inputStacks = new()
            {
                new(Details.ironIngot, 3.0f)
            },
            outputStacks = new()
            {
                new(Details.ironPlate, 2.0f)
            }
        };
        bendRodRecipe = new()
        {
            craftTime = 0.5f,
            inputStacks = new()
            {
                new(Details.ironPlate, 2.0f)
            },
            outputStacks = new()
            {
                new(Details.rod, 3.0f)
            }
        };
        cutScrewsRecipe = new()
        {
            craftTime = 0.5f,
            inputStacks = new()
            {
                new(Details.rod, 1.0f)
            },
            outputStacks = new()
            {
                new(Details.screws, 2.0f)
            }
        };
    
        all = new()
        {
            smeltRecipe,
            pressRecipe
        };
    }
}

public static class Complexes
{
    public static ComplexType supplier;
    public static ComplexType smelter;
    public static ComplexType press;
    public static ComplexType bendingComplex;
    public static ComplexType cuttingComplex;

    public static List<ComplexType> all;

    public static void Init()
    {
        supplier = new()
        {
            name = "Supplier",
            prefab = Resources.Load<Complex>("Prefabs/Supplier")  
        };
        smelter = new CraftingComplexType()
        {
            name = "Smelter",
            prefab = Resources.Load<Complex>("Prefabs/Smelter"),
            recipe = Recipes.smeltRecipe
        };
        press = new CraftingComplexType()
        {
            name = "Press",
            prefab = Resources.Load<Complex>("Prefabs/Press"),
            recipe = Recipes.pressRecipe,
        };
        bendingComplex = new CraftingComplexType()
        {
            name = "Bending Complex",
            prefab = Resources.Load<Complex>("Prefabs/BendingComplex"),
            recipe = Recipes.bendRodRecipe
        };
        cuttingComplex = new CraftingComplexType()
        {
            name = "Cutting Complex",
            prefab = Resources.Load<Complex>("Prefabs/CuttingComplex"),
            recipe = Recipes.cutScrewsRecipe
        };

        all = new()
        {
            supplier, smelter, press, bendingComplex, cuttingComplex
        };
    } 

    public static void PostInit()
    {
        supplier.research = Researches.supplier;
        smelter.research = Researches.smelter;
        press.research = Researches.press;
        bendingComplex.research = Researches.bending;
        cuttingComplex.research = Researches.cutting;
    }
}

public static class BuildSpots
{
    public static BuildSpotType spot0;

    public static void Init()
    {
        spot0 = new()
        {
        };
    }
}
public static class Researches
{
    public static ComplexResearchTech supplier;
    public static ComplexResearchTech smelter;
    public static ComplexResearchTech press;
    public static ComplexResearchTech bending;
    public static ComplexResearchTech cutting;

    public static BuffsResearchTech production;
    public static BuffsResearchTech standardizedComplexes0;
    public static BuffsResearchTech standardizedComplexes1;
    public static BuffsResearchTech standardizedComplexes2;
    public static BuffsResearchTech flexibleComplexes0;
    public static BuffsResearchTech flexibleComplexes1;
    public static BuffsResearchTech flexibleComplexes2;

    public static List<ComplexResearchTech> complexTechs;
    public static List<ResearchTech> all;

    public static void Init()
    {
        supplier = new()
        {
            researchTime = 0.0f,
            name = "Supplier",
        };
        smelter = new()
        {
            researchTime = 20.0f,
            name = "Smelter",
            requiredTechs = new()
            {
                supplier
            }
        };
        press = new()
        {
            researchTime = 20.0f,
            name = "Press",
            requiredTechs = new()
            {
                smelter
            }
        };
        bending = new()
        {
            researchTime = 20.0f,
            name = "Bending",
            requiredTechs = new()
            {
                press
            }
        };
        cutting = new()
        {
            researchTime = 20.0f,
            name = "Cutting",
            requiredTechs = new()
            {
                bending
            }
        };

        production = new()
        {
            researchTime = 20.0f,
            name = "Production",
        };
        
        standardizedComplexes0 = new()
        {
            researchTime = 20.0f,
            name = "Standardized Complexes 1",
            requiredTechs = new()
            {
                production
            }
        };
        standardizedComplexes1 = new()
        {
            researchTime = 20.0f,
            name = "Standardized Complexes 2",
            requiredTechs = new()
            {
                standardizedComplexes0
            }
        };
        standardizedComplexes2 = new()
        {
            researchTime = 20.0f,
            name = "Standardized Complexes 3",
            requiredTechs = new()
            {
                standardizedComplexes1
            }
        };

        flexibleComplexes0 = new()
        {
            researchTime = 20.0f,
            name = "Flexible Complexes 1",
            requiredTechs = new()
            {
                production
            }
        };
        flexibleComplexes1 = new()
        {
            researchTime = 20.0f,
            name = "Flexible Complexes 2",
            requiredTechs = new()
            {
                flexibleComplexes0
            }
        };
        flexibleComplexes2 = new()
        {
            researchTime = 20.0f,
            name = "Flexible Complexes 3",
            requiredTechs = new()
            {
                flexibleComplexes1
            }
        };

        standardizedComplexes0.exclusive = new()
        {
            flexibleComplexes0
        };
        flexibleComplexes0.exclusive = new()
        {
            standardizedComplexes0
        };


        complexTechs = new()
        {
            supplier, smelter, press, bending, cutting
        };
        all = new()
        {
            supplier, smelter, press, bending, cutting,
            
            production, 
            standardizedComplexes0, standardizedComplexes1, standardizedComplexes2, 
            flexibleComplexes0, flexibleComplexes1, flexibleComplexes2
        };
    }    
}

public static class Suppliers
{
    public static SupplierType baseSupplier;
    public static SupplierType supplier0;

    public static List<SupplierType> all;

    public static void Init()
    {
        baseSupplier = new()
        {
            name = "Base Supplier",
            influencePrice = 100.0f,
            materialPrice = 12.0f,
        };
        supplier0 = new()
        {
            name = "Placeholder Name 0",
            influencePrice = 100.0f,
            materialPrice = 10.0f,
        };

        all = new()
        {
            baseSupplier, supplier0
        };
    }
}