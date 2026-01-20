using System.Collections.Generic;
using UnityEngine;

public static class Details
{
    public static DetailType ore;
    public static DetailType ingot;
    public static DetailType plate;
    public static DetailType rod;
    public static DetailType screws;
    public static DetailType reinforcedPlate;
    public static DetailType gear;
    public static DetailType tube;
    public static DetailType engine;

    public static List<DetailType> all;

    public static void Init()
    {
        ore = new()
        {
            name = "Ore",
            price = 10.0f
        };
        ingot = new()
        {
            name = "Ingot",
            price = 20.0f
        };
        plate = new()
        {
            name = "Plate",
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
        reinforcedPlate = new()
        {
            name = "Reinforced Plate",
            price = 20.0f
        };
        gear = new()
        {
            name = "Gear",
            price = 20.0f
        };
        tube = new()
        {
            name = "Tube",
            price = 20.0f
        };
        engine = new()
        {
            name = "Engine",
            price = 20.0f,
        };
        

        all = new()
        {
            ore, ingot, plate, rod, screws, reinforcedPlate, gear, tube, engine,
        };
    }
}

public static class Orders
{
    public static OrderType order0;
    public static OrderType order1;
    public static OrderType order2;
    public static OrderType order3;
    public static OrderType order4;
    public static OrderType order5;
    public static OrderType order6;
    public static OrderType order7;
    public static OrderType order8;
    public static OrderType order9;

    public static List<OrderType> requiredOrders;
    
    public static void Init()
    {
        order0 = new()
        {
            requirements = new()
            {
                new MoneyOrderRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyOrderPunishment()
                {
                    money = 200
                },
            },
            time = 35,
            name = "Required Order 1",
        };
        order1 = new()
        {
            requirements = new()
            {
                new MoneyOrderRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyOrderPunishment()
                {
                    money = 200
                },
            },

            time = 35,
            name = "Required Order 2",
        };
        order2 = new()
        {
            requirements = new()
            {
                new MoneyOrderRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyOrderPunishment()
                {
                    money = 200
                },
            },

            time = 35,
            name = "Required Order 3",
        };
        order3 = new()
        {
            requirements = new()
            {
                new MoneyOrderRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyOrderPunishment()
                {
                    money = 200
                },
            },

            time = 35,
            name = "Required Order 4",
        };
        order4 = new()
        {
            requirements = new()
            {
                new MoneyOrderRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyOrderPunishment()
                {
                    money = 200
                },
            },
            time = 35,
            name = "Required Order 5",
        };
        order5 = new()
        {
            requirements = new()
            {
                new MoneyOrderRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyOrderPunishment()
                {
                    money = 200
                },
            },
            time = 35,
            name = "Required Order 6",
        };
        order6 = new()
        {
            requirements = new()
            {
                new MoneyOrderRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyOrderPunishment()
                {
                    money = 200
                },
            },
            time = 35,
            name = "Required Order 7",
        };
        order7 = new()
        {
            requirements = new()
            {
                new MoneyOrderRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyOrderPunishment()
                {
                    money = 200
                },
            },
            time = 35,
            name = "Required Order 8",
        };
        order8 = new()
        {
            requirements = new()
            {
                new MoneyOrderRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyOrderPunishment()
                {
                    money = 200
                },
            },
            time = 40,
            name = "Required Order 9",
        };
        order9 = new()
        {
            requirements = new()
            {
                new MoneyOrderRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyOrderPunishment()
                {
                    money = 200
                },
            },
            time = 45,
            name = "Required Order 10",
        };

        requiredOrders = new()
        {
            order0, order1, order2, order3, order4, order5, order6, order7, order8, order9
        };
    }
}

public static class Recipes
{
    public static CraftRecipe smeltRecipe;
    public static CraftRecipe pressRecipe;
    public static CraftRecipe bendRodRecipe;
    public static CraftRecipe cutScrewsRecipe;
    public static CraftRecipe reinforcePlateRecipe;
    public static CraftRecipe bendTubeRecipe;
    public static CraftRecipe formGearRecipe;
    public static CraftRecipe assemblyEngineRecipe;

    public static List<CraftRecipe> all;

    public static void Init()
    {
        smeltRecipe = new()
        {
            craftTime = 0.5f,
            inputStacks = new()
            {
                new(Details.ore, 1.0f)
            },
            outputStacks = new()
            {
                new(Details.ingot, 1.0f)
            },
        };
        pressRecipe = new()
        {
            craftTime = 0.5f,
            inputStacks = new()
            {
                new(Details.ingot, 3.0f)
            },
            outputStacks = new()
            {
                new(Details.plate, 2.0f)
            }
        };
        bendRodRecipe = new()
        {
            craftTime = 0.5f,
            inputStacks = new()
            {
                new(Details.plate, 2.0f)
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
        reinforcePlateRecipe = new()
        {
            craftTime = 0.5f,
            inputStacks = new()
            {
                new(Details.screws, 1.0f),
                new(Details.plate, 1.0f)
            },
            outputStacks = new()
            {
                new(Details.reinforcedPlate, 2.0f)
            }
        };
        bendTubeRecipe = new()
        {
            craftTime = 0.5f,
            inputStacks = new()
            {
                new(Details.plate, 1.0f),
            },
            outputStacks = new()
            {
                new(Details.tube, 2.0f)
            }
        };
        formGearRecipe = new()
        {
            craftTime = 0.5f,
            inputStacks = new()
            {
                new(Details.ingot, 1.0f),
            },
            outputStacks = new()
            {
                new(Details.gear, 2.0f)
            }
        };
        assemblyEngineRecipe = new()
        {
            craftTime = 3.0f,
            inputStacks = new()
            {
                new(Details.reinforcedPlate, 1.0f),
                new(Details.tube, 1.0f),
                new(Details.gear, 1.0f),
            },
            outputStacks = new()
            {
                new(Details.engine, 1.0f),
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
    public static ComplexType rodBendingComplex;
    public static ComplexType cuttingComplex;
    public static ComplexType reinforcingComplex;
    public static ComplexType formingComplex;
    public static ComplexType tubeBendingComplex;
    public static ComplexType engineAssemblingComplex;


    public static List<ComplexType> all;

    public static void Init()
    {
        supplier = new()
        {
            name = "Supplier",
            prefab = Resources.Load<Complex>("Prefabs/Supplier"),
            buildTime = 10.0f,
        };
        smelter = new CraftingComplexType()
        {
            name = "Smelter",
            prefab = Resources.Load<Complex>("Prefabs/CraftingComplex"),
            recipe = Recipes.smeltRecipe,
            buildTime = 10.0f,
        };
        press = new CraftingComplexType()
        {
            name = "Press",
            prefab = Resources.Load<Complex>("Prefabs/CraftingComplex"),
            recipe = Recipes.pressRecipe,
            buildTime = 10.0f,
        };
        rodBendingComplex = new CraftingComplexType()
        {
            name = "Rod Bender",
            prefab = Resources.Load<Complex>("Prefabs/CraftingComplex"),
            recipe = Recipes.bendRodRecipe,
            buildTime = 10.0f,
        };
        cuttingComplex = new CraftingComplexType()
        {
            name = "Cutter",
            prefab = Resources.Load<Complex>("Prefabs/CraftingComplex"),
            recipe = Recipes.cutScrewsRecipe,
            buildTime = 10.0f,
        };
        reinforcingComplex = new CraftingComplexType()
        {
            name = "Reinforcer",
            prefab = Resources.Load<Complex>("Prefabs/CraftingComplex"),
            recipe = Recipes.reinforcePlateRecipe,
            buildTime = 10.0f,
        };
        tubeBendingComplex = new CraftingComplexType()
        {
            name = "Tube Bender",
            prefab = Resources.Load<Complex>("Prefabs/CraftingComplex"),
            recipe = Recipes.bendTubeRecipe,
            buildTime = 10.0f,
        };
        formingComplex = new CraftingComplexType()
        {
            name = "Former",
            prefab = Resources.Load<Complex>("Prefabs/CraftingComplex"),
            recipe = Recipes.formGearRecipe,
            buildTime = 10.0f,
        };
        engineAssemblingComplex = new CraftingComplexType()
        {
            name = "Engine Assembler",
            prefab = Resources.Load<Complex>("Prefabs/CraftingComplex"),
            recipe = Recipes.assemblyEngineRecipe,
            buildTime = 10.0f,
        };

        all = new()
        {
            supplier, smelter, press, rodBendingComplex, cuttingComplex, reinforcingComplex, tubeBendingComplex, formingComplex, engineAssemblingComplex
        };
    } 

    public static void PostInit()
    {
        supplier.research = Researches.supply;
        smelter.research = Researches.smelting;
        press.research = Researches.pressing;
        rodBendingComplex.research = Researches.bending0;
        cuttingComplex.research = Researches.cutting;
        reinforcingComplex.research = Researches.reinforcing;
        tubeBendingComplex.research = Researches.bending1;
        formingComplex.research = Researches.forming;
        engineAssemblingComplex.research = Researches.assembling;
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
    public static ComplexResearchTech supply;
    public static ComplexResearchTech smelting;
    public static ComplexResearchTech pressing;
    public static ComplexResearchTech bending0;
    public static ComplexResearchTech cutting;
    public static ComplexResearchTech reinforcing;
    public static ComplexResearchTech bending1;
    public static ComplexResearchTech forming;
    public static ComplexResearchTech assembling;

    public static BuffsResearchTech production;
    public static BuffsResearchTech standardizedComplexes0;
    public static BuffsResearchTech standardizedComplexes1;
    public static BuffsResearchTech standardizedComplexes2;
    public static BuffsResearchTech flexibleComplexes0;
    public static BuffsResearchTech flexibleComplexes1;
    public static BuffsResearchTech flexibleComplexes2;

    public static BuffsResearchTech researching0;

    public static List<ComplexResearchTech> complexTechs;
    public static List<ResearchTech> all;

    public static void Init()
    {
        supply = new()
        {
            researchTime = 35.0f,
            name = "Supply",
        };
        smelting = new()
        {
            researchTime = 35.0f,
            name = "Smelting",
            requiredTechs = new()
            {
                supply
            }
        };
        pressing = new()
        {
            researchTime = 35.0f,
            name = "Pressing",
            requiredTechs = new()
            {
                smelting
            }
        };
        bending0 = new()
        {
            researchTime = 35.0f,
            name = "Bending 1",
            requiredTechs = new()
            {
                pressing
            }
        };
        cutting = new()
        {
            researchTime = 35.0f,
            name = "Cutting",
            requiredTechs = new()
            {
                bending0
            }
        };
        reinforcing = new()
        {
            researchTime = 35.0f,
            name = "Reinforcing",
            requiredTechs = new()
            {
                cutting
            }
        };
        bending1 = new()
        {
            researchTime = 35.0f,
            name = "Bending 2",
            requiredTechs = new()
            {
                pressing
            }
        };
        forming = new()
        {
            researchTime = 35.0f,
            name = "Forming",
            requiredTechs = new()
            {
                smelting
            }
        };
        assembling = new()
        {
            researchTime = 40.0f,
            name = "Assembling",
            requiredTechs = new()
            {
                reinforcing,
                bending1,
                forming
            }
        };

        production = new()
        {
            researchTime = 35.0f,
            name = "Production",
        };
        
        standardizedComplexes0 = new()
        {
            researchTime = 35.0f,
            name = "Standardized Complexes 1",
            requiredTechs = new()
            {
                production
            }
        };
        standardizedComplexes1 = new()
        {
            researchTime = 35.0f,
            name = "Standardized Complexes 2",
            requiredTechs = new()
            {
                standardizedComplexes0
            }
        };
        standardizedComplexes2 = new()
        {
            researchTime = 35.0f,
            name = "Standardized Complexes 3",
            requiredTechs = new()
            {
                standardizedComplexes1
            }
        };

        flexibleComplexes0 = new()
        {
            researchTime = 35.0f,
            name = "Flexible Complexes 1",
            requiredTechs = new()
            {
                production
            }
        };
        flexibleComplexes1 = new()
        {
            researchTime = 35.0f,
            name = "Flexible Complexes 2",
            requiredTechs = new()
            {
                flexibleComplexes0
            }
        };
        flexibleComplexes2 = new()
        {
            researchTime = 35.0f,
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

        researching0 = new()
        {
            researchTime = 40.0f,
            name = "Researching",
            researchSpeedBonus = 0.05f
        };

        complexTechs = new()
        {
            supply, smelting, pressing, bending0, cutting, reinforcing, bending1, forming, assembling, 
        };
        all = new()
        {
            supply, smelting, pressing, bending0, cutting, reinforcing, bending1, forming, assembling,
            
            production, 
            standardizedComplexes0, standardizedComplexes1, standardizedComplexes2, 
            flexibleComplexes0, flexibleComplexes1, flexibleComplexes2,
        
            researching0,
        };
    }    
}

public static class Suppliers
{
    public static SupplierType baseSupplier;
    public static SupplierType supplier0;
    public static SupplierType supplier1;
    public static SupplierType supplier2;

    public static List<SupplierType> all;

    public static void Init()
    {
        baseSupplier = new()
        {
            name = "Elinor Christabella",
            influencePrice = 100.0f,
            modifiers = new()
            {
                new MaterialPriceModifier()
                {
                    Bonus = 2.0f,
                }
            },
        };
        supplier0 = new()
        {
            name = "Hildegarda Millicent",
            influencePrice = 100.0f,
            modifiers = new()
            {
                new DetailQualityModifier()
                {
                    Bonus = -0.05f
                },
            },
        };
        supplier1 = new()
        {
            name = "Stacey Ennis",
            influencePrice = 100.0f,
            modifiers = new()
            {
                new DetailQualityModifier()
                {
                    Bonus = 0.05f
                },
                new MaterialPriceModifier()
                {
                    Bonus = 4.0f,
                },
            },
        };
        supplier2 = new()
        {
            name = "Jeffrey Epstein",
            influencePrice = 100.0f,
            modifiers = new()
            {
                new DetailQualityModifier()
                {
                    Bonus = -0.4f
                },
                new MaterialPriceModifier()
                {
                    Bonus = -5.0f,
                },
            },
        };

        all = new()
        {
            baseSupplier, supplier0, supplier1, supplier2, 
        };
    }
}