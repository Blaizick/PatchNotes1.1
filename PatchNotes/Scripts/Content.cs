using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            price = 0.0f
        };
        ingot = new()
        {
            name = "Ingot",
            price = 10.0f
        };
        plate = new()
        {
            name = "Plate",
            price = 22.5f,
        };
        rod = new()
        {
            name = "Rod",
            price = 27.5f,
        };
        screws = new()
        {
            name = "Screws",
            price = 21.66f,
        };
        reinforcedPlate = new()
        {
            name = "Reinforced Plate",
            price = 49.16f
        };
        gear = new()
        {
            name = "Gear",
            price = 15.0f
        };
        tube = new()
        {
            name = "Tube",
            price = 27.5f
        };
        engine = new()
        {
            name = "Engine",
            price = 96.16f,
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
                new MoneyRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyPunishment()
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
                new MoneyRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyPunishment()
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
                new MoneyRequirement()
                {
                    money = 200
                }
            },
            punishments = new()
            {
                new MoneyPunishment()
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
                new MoneyRequirement()
                {
                    money = 300
                }
            },
            punishments = new()
            {
                new MoneyPunishment()
                {
                    money = 300
                },
            },

            time = 35,
            name = "Required Order 4",
        };
        order4 = new()
        {
            requirements = new()
            {
                new MoneyRequirement()
                {
                    money = 500
                }
            },
            punishments = new()
            {
                new MoneyPunishment()
                {
                    money = 500
                },
            },
            time = 35,
            name = "Required Order 5",
        };
        order5 = new()
        {
            requirements = new()
            {
                new MoneyRequirement()
                {
                    money = 500
                }
            },
            punishments = new()
            {
                new MoneyPunishment()
                {
                    money = 500
                },
            },
            time = 35,
            name = "Required Order 6",
        };
        order6 = new()
        {
            requirements = new()
            {
                new MoneyRequirement()
                {
                    money = 500
                }
            },
            punishments = new()
            {
                new MoneyPunishment()
                {
                    money = 500
                },
            },
            time = 35,
            name = "Required Order 7",
        };
        order7 = new()
        {
            requirements = new()
            {
                new MoneyRequirement()
                {
                    money = 600
                }
            },
            punishments = new()
            {
                new MoneyPunishment()
                {
                    money = 600
                },
            },
            time = 35,
            name = "Required Order 8",
        };
        order8 = new()
        {
            requirements = new()
            {
                new MoneyRequirement()
                {
                    money = 600
                }
            },
            punishments = new()
            {
                new MoneyPunishment()
                {
                    money = 600
                },
            },
            time = 40,
            name = "Required Order 9",
        };
        order9 = new()
        {
            requirements = new()
            {
                new MoneyRequirement()
                {
                    money = 800
                }
            },
            punishments = new()
            {
                new MoneyPunishment()
                {
                    money = 800
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
            craftTime = 1.0f,
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
            craftTime = 1f,
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
            craftTime = 1f,
            inputStacks = new()
            {
                new(Details.plate, 1.0f)
            },
            outputStacks = new()
            {
                new(Details.rod, 1.0f)
            }
        };
        cutScrewsRecipe = new()
        {
            craftTime = 1f,
            inputStacks = new()
            {
                new(Details.rod, 2.0f)
            },
            outputStacks = new()
            {
                new(Details.screws, 3.0f)
            }
        };
        reinforcePlateRecipe = new()
        {
            craftTime = 1f,
            inputStacks = new()
            {
                new(Details.screws, 1.0f),
                new(Details.plate, 1.0f)
            },
            outputStacks = new()
            {
                new(Details.reinforcedPlate, 1.0f)
            }
        };
        bendTubeRecipe = new()
        {
            craftTime = 1f,
            inputStacks = new()
            {
                new(Details.plate, 1.0f),
            },
            outputStacks = new()
            {
                new(Details.tube, 1.0f)
            }
        };
        formGearRecipe = new()
        {
            craftTime = 1f,
            inputStacks = new()
            {
                new(Details.ingot, 1.0f),
            },
            outputStacks = new()
            {
                new(Details.gear, 1.0f)
            }
        };
        assemblyEngineRecipe = new()
        {
            craftTime = 2.0f,
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
            smeltRecipe, pressRecipe, bendRodRecipe, cutScrewsRecipe, reinforcePlateRecipe, bendTubeRecipe, formGearRecipe, assemblyEngineRecipe,
        };
    }
}

public static class Complexes
{
    public static ComplexType supplier;
    public static ComplexType packingComplex;
    public static ComplexType buildingComplex;
    public static ComplexType buildSpotComplex;
    public static ComplexType buyBuildSpotComplex;
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
        supplier = new ProducingComplexType()
        {
            name = "Supplier",
            prefab = Resources.Load<Complex>("Prefabs/Supplier"),
            buildTime = 10.0f,
            canBeNextComplex = false,
            desc = "Delivers base resource from your supplier person, it is basically the foundament of each production line.",
            sprite = BuildingIcons.all[6],
            outputStacks = new()
            {
                new(Details.ore, 1.0f),
            },
            maxNextConnections = 2,
        };
        packingComplex = new ComplexType()
        {
            name = "Packing Complex",
            prefab = Resources.Load<Complex>("Prefabs/PackingComplex"),
            chefAllowed = false,
            breakable = false,
            canHaveNextComplex = false,
            buildable = false,
            sprite = BuildingIcons.all[5],
        };
        buildingComplex = new ComplexType()
        {
            name = "Building Complex",
            prefab = Resources.Load<Complex>("Prefabs/BuildingComplex"),
            chefAllowed = false,
            buildable = false,
            canHaveNextComplex = false,
            canBeNextComplex = false,
        };
        buildSpotComplex = new ComplexType()
        {
            name = "Build Spot",
            desc = "You can build any complex here by clicking on it",
            prefab = Resources.Load<Complex>("Prefabs/BuildSpotComplex"),
            chefAllowed = false,
            breakable = false,
            buildable = false,
            canHaveNextComplex = false,
            canBeNextComplex = false,
        };
        buyBuildSpotComplex = new ComplexType()
        {
            name = "Buy Build Spot",
            prefab = Resources.Load<Complex>("Prefabs/BuyBuildSpotComplex"),
            chefAllowed = false,
            breakable = false,
            buildable = false,
            canHaveNextComplex = false,
            canBeNextComplex = false,
            desc = "You can buy a build spot on place of this building for your money",
        };
        smelter = new CraftingComplexType()
        {
            name = "Smelter",
            prefab = Resources.Load<Complex>("Prefabs/Smelter"),
            recipe = Recipes.smeltRecipe,
            buildTime = 15.0f,
            sprite = BuildingIcons.all[0],
            capacity = 10.0f,
            maxNextConnections = 2,
        };
        press = new CraftingComplexType()
        {
            name = "Press",
            prefab = Resources.Load<Complex>("Prefabs/Press"),
            recipe = Recipes.pressRecipe,
            buildTime = 15.0f,
            sprite = BuildingIcons.all[1],
            capacity = 10.0f,
            maxNextConnections = 2,
        };
        rodBendingComplex = new CraftingComplexType()
        {
            name = "Rod Bender",
            prefab = Resources.Load<Complex>("Prefabs/RodBender"),
            recipe = Recipes.bendRodRecipe,
            buildTime = 15.0f,
            sprite = BuildingIcons.all[2],
            capacity = 10.0f,
            maxNextConnections = 2,
        };
        cuttingComplex = new CraftingComplexType()
        {
            name = "Cutter",
            prefab = Resources.Load<Complex>("Prefabs/Cutter"),
            recipe = Recipes.cutScrewsRecipe,
            buildTime = 15.0f,
            sprite = BuildingIcons.all[3],
            capacity = 10.0f,
            maxNextConnections = 2,
        };
        reinforcingComplex = new CraftingComplexType()
        {
            name = "Reinforcer",
            prefab = Resources.Load<Complex>("Prefabs/ReinforcingComplex"),
            recipe = Recipes.reinforcePlateRecipe,
            buildTime = 20.0f,
            sprite = BuildingIcons.all[10],
            capacity = 10.0f,
            maxNextConnections = 2,
        };
        tubeBendingComplex = new CraftingComplexType()
        {
            name = "Tube Bender",
            prefab = Resources.Load<Complex>("Prefabs/TubeBender"),
            recipe = Recipes.bendTubeRecipe,
            buildTime = 15.0f,
            sprite = BuildingIcons.all[11],
            capacity = 10.0f,
            maxNextConnections = 2,
        };
        formingComplex = new CraftingComplexType()
        {
            name = "Former",
            prefab = Resources.Load<Complex>("Prefabs/Former"),
            recipe = Recipes.formGearRecipe,
            buildTime = 15.0f,
            sprite = BuildingIcons.all[12],
            capacity = 10.0f,
            maxNextConnections = 2,
        };
        engineAssemblingComplex = new CraftingComplexType()
        {
            name = "Engine Assembler",
            prefab = Resources.Load<Complex>("Prefabs/EngineAssembler"),
            recipe = Recipes.assemblyEngineRecipe,
            buildTime = 25.0f,
            sprite = BuildingIcons.all[4],
            capacity = 10.0f,
            maxNextConnections = 2,
        };

        all = new()
        {
            supplier, packingComplex, buildingComplex, buildSpotComplex, buyBuildSpotComplex,
            smelter, press, rodBendingComplex, cuttingComplex, reinforcingComplex, tubeBendingComplex, formingComplex, engineAssemblingComplex,
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

    public static ModifiersResearchTech production;
    public static ModifiersResearchTech standardizedComplexes0;
    public static ModifiersResearchTech standardizedComplexes1;
    public static ModifiersResearchTech standardizedComplexes2;
    public static ModifiersResearchTech flexibleComplexes0;
    public static ModifiersResearchTech flexibleComplexes1;
    public static ModifiersResearchTech flexibleComplexes2;

    public static ModifiersResearchTech researching0;

    public static List<ComplexResearchTech> complexTechs;
    public static List<ResearchTech> all;

    public static void Init()
    {
        supply = new()
        {
            researchTime = 35.0f,
            name = "Supply",
            unlock = Complexes.supplier,
        };
        smelting = new()
        {
            researchTime = 35.0f,
            name = "Smelting",
            requiredTechs = new()
            {
                supply
            },
            unlock = Complexes.smelter,
        };
        pressing = new()
        {
            researchTime = 35.0f,
            name = "Pressing",
            requiredTechs = new()
            {
                smelting
            },
            unlock = Complexes.press,
        };
        bending0 = new()
        {
            researchTime = 35.0f,
            name = "Bending 1",
            requiredTechs = new()
            {
                pressing
            },
            unlock = Complexes.rodBendingComplex,
        };
        cutting = new()
        {
            researchTime = 35.0f,
            name = "Cutting",
            requiredTechs = new()
            {
                bending0
            },
            unlock = Complexes.cuttingComplex,
        };
        reinforcing = new()
        {
            researchTime = 35.0f,
            name = "Reinforcing",
            requiredTechs = new()
            {
                cutting
            },
            unlock = Complexes.reinforcingComplex,
        };
        bending1 = new()
        {
            researchTime = 35.0f,
            name = "Bending 2",
            requiredTechs = new()
            {
                pressing
            },
            unlock = Complexes.tubeBendingComplex,
        };
        forming = new()
        {
            researchTime = 35.0f,
            name = "Forming",
            requiredTechs = new()
            {
                smelting
            },
            unlock = Complexes.formingComplex,
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
            },
            unlock = Complexes.engineAssemblingComplex,
        };

        production = new()
        {
            researchTime = 35.0f,
            name = "Production",
            modifiers = new()
            {
                new MaxEffeciencyModifier()
                {
                    Bonus = 0.1f,
                },
                new EffeciencyGrowModifier()
                {
                    Bonus = 0.1f,
                },
            }
        };
        
        standardizedComplexes0 = new()
        {
            researchTime = 35.0f,
            name = "Standardized Complexes 1",
            requiredTechs = new()
            {
                production
            },
            modifiers = new()
            {
                new MaxEffeciencyModifier()
                {
                    Bonus = 0.15f,
                },
            },
        };
        standardizedComplexes1 = new()
        {
            researchTime = 35.0f,
            name = "Standardized Complexes 2",
            requiredTechs = new()
            {
                standardizedComplexes0
            },
            modifiers = new()
            {
                new MaxEffeciencyModifier()
                {
                    Bonus = 0.15f,
                },
            },
        };
        standardizedComplexes2 = new()
        {
            researchTime = 35.0f,
            name = "Standardized Complexes 3",
            requiredTechs = new()
            {
                standardizedComplexes1
            },
            modifiers = new()
            {
                new MaxEffeciencyModifier()
                {
                    Bonus = 0.15f,
                },
            },
        };

        flexibleComplexes0 = new()
        {
            researchTime = 35.0f,
            name = "Flexible Complexes 1",
            requiredTechs = new()
            {
                production
            },
            modifiers = new()
            {
                new MaxEffeciencyModifier()
                {
                    Bonus = 0.1f,
                },
                new EffeciencyGrowModifier()
                {
                    Bonus = 0.1f,
                },
            },
        };
        flexibleComplexes1 = new()
        {
            researchTime = 35.0f,
            name = "Flexible Complexes 2",
            requiredTechs = new()
            {
                flexibleComplexes0
            },
            modifiers = new()
            {
                new MaxEffeciencyModifier()
                {
                    Bonus = 0.1f,
                },
                new EffeciencyGrowModifier()
                {
                    Bonus = 0.1f,
                },
            },
        };
        flexibleComplexes2 = new()
        {
            researchTime = 35.0f,
            name = "Flexible Complexes 3",
            requiredTechs = new()
            {
                flexibleComplexes1
            },
            modifiers = new()
            {
                new MaxEffeciencyModifier()
                {
                    Bonus = 0.1f,
                },
                new EffeciencyGrowModifier()
                {
                    Bonus = 0.1f,
                },
            },
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
            modifiers = new()
            {
                new ResearchSpeedModifier()
                {
                    Bonus = 0.05f,
                },
            }
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
            influencePrice = 150.0f,
            modifiers = new()
            {
                new MaterialPriceModifier()
                {
                    Bonus = 2.0f,
                },
            },
            sprite = Portraits.all[0],
        };
        supplier0 = new()
        {
            name = "Hildegarda Millicent",
            influencePrice = 150.0f,
            modifiers = new()
            {
                new DetailQualityModifier()
                {
                    Bonus = -0.05f,
                },
                new MaterialPriceModifier()
                {
                    Bonus = -4.0f,
                },
            },
            sprite = Portraits.all[4],
        };
        supplier1 = new()
        {
            name = "Stacey Ennis",
            influencePrice = 150.0f,
            modifiers = new()
            {
                new DetailQualityModifier()
                {
                    Bonus = 0.3f
                },
                new MaterialPriceModifier()
                {
                    Bonus = 4f,
                },
            },
            sprite = Portraits.all[5],
        };
        supplier2 = new()
        {
            name = "Jeffer Estone",
            influencePrice = 150.0f,
            modifiers = new()
            {
                new DetailQualityModifier()
                {
                    Bonus = -0.15f
                },
                new MaterialPriceModifier()
                {
                    Bonus = -6.0f,
                },
            },
            sprite = Portraits.all[1],
        };

        all = new()
        {
            baseSupplier, supplier0, supplier1, supplier2, 
        };
    }
}

public static class Chefs
{
    public static ChefType chef0;
    public static ChefType chef1;
    public static ChefType chef2;
    public static ChefType chef3;
    public static ChefType chef4;
    public static ChefType chef5;
    public static ChefType chef6;

    public static List<ChefType> all;

    public static void Init()
    {
        chef0 = new()
        {
            name = "Dayton Tommy",
            maxEffeciencyBonus = 0.1f,
            maxEffeciencyMultiplier = 0.0f,
            effeciencyGrowBonus = 0.05f,
            effeciencyGrowMultiplier = 0.0f,
            influencePrice = 50.0f,
            sprite = Portraits.all[2],
        };
        chef1 = new()
        {
            name = "Elina Sabryna",
            maxEffeciencyBonus = 0.1f,
            maxEffeciencyMultiplier = 0.0f,
            effeciencyGrowBonus = 0.05f,
            effeciencyGrowMultiplier = 0.0f,
            influencePrice = 50.0f,
            sprite = Portraits.all[0],
        };
        chef2 = new()
        {
            name = "Tyron Villem",
            maxEffeciencyBonus = 0.1f,
            maxEffeciencyMultiplier = 0.0f,
            effeciencyGrowBonus = 0.05f,
            effeciencyGrowMultiplier = 0.0f,
            influencePrice = 50.0f,
            sprite = Portraits.all[3],
        };
        chef3 = new()
        {
            name = "Jamison Al",
            maxEffeciencyBonus = 0.1f,
            maxEffeciencyMultiplier = 0.0f,
            effeciencyGrowBonus = 0.05f,
            effeciencyGrowMultiplier = 0.0f,
            influencePrice = 50.0f,
            sprite = Portraits.all[6],
        };
        chef4 = new()
        {
            name = "Adrienne Cor",
            maxEffeciencyBonus = 0.1f,
            maxEffeciencyMultiplier = 0.0f,
            effeciencyGrowBonus = 0.05f,
            effeciencyGrowMultiplier = 0.0f,
            influencePrice = 50.0f,
            sprite = Portraits.all[4],
        };
        chef5 = new()
        {
            name = "Norbert Kimbra",
            maxEffeciencyBonus = 0.1f,
            maxEffeciencyMultiplier = 0.0f,
            effeciencyGrowBonus = 0.05f,
            effeciencyGrowMultiplier = 0.0f,
            influencePrice = 50.0f,
            sprite = Portraits.all[7],
        };
        chef6 = new()
        {
            name = "Rowley Kaarel",
            maxEffeciencyBonus = 0.1f,
            maxEffeciencyMultiplier = 0.0f,
            effeciencyGrowBonus = 0.05f,
            effeciencyGrowMultiplier = 0.0f,
            influencePrice = 50.0f,
            sprite = Portraits.all[8],
        };

        all = new()
        {
            chef0, chef1, chef2, chef3, chef4, chef5, chef6
        };
    }
}

public class ManagerType
{
    public Sprite sprite;
    public string name;
    public ManagerCategory category;

    public List<Modifier> modifiers;

    public float influencePrice;

    public static Dictionary<ManagerCategory, List<ManagerType>> allDic = new();
    public static List<ManagerType> all = new();


    public Manager AsManager()
    {
        var m = new Manager();
        m.type = this;
        return m;
    }

    public static void GInit()
    {
        new ManagerType()
        {
            name = "Jayanti Kantuta",
            category = ManagerCategory.cfo,
            influencePrice = 100.0f,
            modifiers = new()
            {
                new IncomeTaxModifier()
                {
                    Bonus = -0.125f,
                }
            },
            sprite = Portraits.all[2],
        }.Init();
        new ManagerType()
        {
            name = "Hanne Shulamith",
            category = ManagerCategory.cfo,
            influencePrice = 100.0f,
            modifiers = new()
            {
                new MaterialPriceModifier()
                {
                    Bonus = -2f,                    
                },
            },
            sprite = Portraits.all[3],
        }.Init();
        new ManagerType()
        {
            name = "Aamu Pavana",
            category = ManagerCategory.cfo,
            influencePrice = 100.0f,
            modifiers = new()
            {
                new DetailQualityModifier()
                {
                    Bonus = 0.1f,                    
                },
            },
            sprite = Portraits.all[0],
        }.Init();

        new ManagerType()
        {
            name = "Cornélio Noʻoroa",
            category = ManagerCategory.coo,
            influencePrice = 100.0f,
            modifiers = new()
            {
                new MaxEffeciencyModifier()
                {
                    Bonus = 0.15f,
                },
            },
            sprite = Portraits.all[6],
        }.Init();
        new ManagerType()
        {
            name = "Kyllian Admetos",
            category = ManagerCategory.coo,
            influencePrice = 100.0f,
            modifiers = new()
            {
                new MaxEffeciencyModifier()
                {
                    Bonus = 0.1f,
                },
                new EffeciencyGrowModifier()
                {
                    Bonus = 0.1f,
                },
                new BuildSpeedModifier()
                {
                    Bonus = 0.05f,
                }
            },
            sprite = Portraits.all[7],
        }.Init();
        new ManagerType()
        {
            name = "Zawar Balbino",
            category = ManagerCategory.coo,
            influencePrice = 50.0f,
            modifiers = new()
            {
                new InfluenceGrowModifier()
                {
                    Multiplier = 0.2f
                }
            },
            sprite = Portraits.all[8],
        }.Init();

        new ManagerType()
        {
            name = "Upton Joziah",
            category = ManagerCategory.cto,
            influencePrice = 100.0f,
            modifiers = new()
            {
                new IncomeTaxModifier()
                {
                    Bonus = -0.05f,
                },
                new BuildSpeedModifier()
                {
                    Bonus = 0.25f,
                },
            },
            sprite = Portraits.all[9],
        }.Init();
        new ManagerType()
        {
            name = "Maxi Kornelija",
            category = ManagerCategory.cto,
            influencePrice = 100.0f,
            modifiers = new()
            {
                new ResearchSpeedModifier()
                {
                    Bonus = 0.1f,
                },
                // new BuildSpeedModifier()
                // {
                //     Bonus = 0.05f,
                // },
            },
            sprite = Portraits.all[4],
        }.Init();
        new ManagerType()
        {
            name = "Macey Nokuthula",
            category = ManagerCategory.cto,
            influencePrice = 100.0f,
            modifiers = new()
            {
                new DetailQualityModifier()
                {
                    Bonus = 0.1f,
                },
            },
            sprite = Portraits.all[2],
        }.Init();
    }

    public ManagerType Init()
    {
        all.Add(this);
        List<ManagerType> l;
        if (!allDic.TryGetValue(category, out l))
        {
            l = new();
            allDic[category] = l;
        }
        l.Add(this);
        return this;
    }
}

public static class Portraits
{
    public static List<Sprite> all;

    public static void Init()
    {
        all = Resources.LoadAll<Sprite>("Sprites/Portraits").ToList();
    }
}

public static class BuildingIcons
{
    public static List<Sprite> all;

    public static void Init()
    {
        all = Resources.LoadAll<Sprite>("Sprites/Buildings").ToList();
    }
}

public static class Events
{
    public static Event event0;
    public static Event event1;
    public static Event event2;
    public static Event event3;
    public static Event event4;
    public static Event event5;
    public static Event event6;
    public static Event event7;

    public static List<Event> all;

    public static void Init()
    {
event0 = new()
{
    action = () =>
    {
        Vars.Instance.ui.popups.ShowPopup(
            "Welcome",
            "You have inherited a factory. By the way, the civil war in your country, Wrazkoslavia, has recently ended. Who knows what the new regime will bring...",
            null,
            new()
            {
                new()
                {
                    name = "OK",
                },
                new()
                {
                    name = "Get Extra Info",
                    tooltipName = "Get Extra Info",
                    tooltipDesc = "Consequence: the \"tutorial\" will begin",
                    onChoose = () =>
                    {
                        Vars.Instance.ui.popups.ShowPopup(
                            "Money",
                            "You can earn money by selling details (see the \"Resources\" menu).",
                            null,
                            new()
                            {
                                new()
                                {
                                    name = "OK",
                                    onChoose = () =>
                                    {
                                        Vars.Instance.ui.popups.ShowPopup(
                                            "Getting details",
                                            "You can obtain details from production complexes. You can build production complexes on build spots by simply clicking on them. You can also connect two complexes so they can transfer resources between each other. To receive details, you must connect one of your complexes to a packing complex in order for the details to appear in the Resources menu. The starting resource is supplied by a supplier.",
                                            null,
                                            new()
                                            {
                                                new()
                                                {
                                                    name = "OK",
                                                }
                                            });
                                    }
                                }
                            });
                    }
                }
            });
    }
};
        event1 = new()
        {
            requirements = new()
            {
                new DayRequirement()
                {
                    day = 5.0f,
                },
            },
            action = () =>
            {
                Vars.Instance.ui.popups.ShowPopup("New law", "The leader of your country, Jasmin Khushi, has issued a new law requiring all manufacturers to pay their government bills.", null, new()
                {
                    new()
                    {
                        name = "OK"
                    }
                });
            },
        };
        event2 = new()
        {
            requirements = new()
            {
                new DayRequirement()
                {
                    day = 30.0f,
                },
            },
            action = () =>
            {
                Vars.Instance.ui.popups.ShowPopup("Criticism", "The new law is facing widespread criticism from manufacturers.", null, new()
                {
                    new()
                    {
                        name = "OK"
                    }
                });
            },
        };
        event3 = new()
        {
            requirements = new()
            {
                new DayRequirement()
                {
                    day = 70.0f,
                },
            },
            action = () =>
            {
                Vars.Instance.ui.popups.ShowPopup("Mass bankruptcies", "Due to the new law, many businesses are simply unable to pay their bills and are closing. Will you be one of them, or will you be able to survive these difficult times?", null, new()
                {
                    new()
                    {
                        name = "OK"
                    }
                });
            },
        };
        event4 = new()
        {
            requirements = new()
            {
                new DayRequirement()
                {
                    day = 120.0f,
                },
            },
            action = () =>
            {
                Vars.Instance.ui.popups.ShowPopup("Assassination attempt on the president", "Recently, the leader of your country was the target of an assassination attempt by unknown individuals. Fortunately or unfortunately, he managed to survive it.", null, new()
                {
                    new()
                    {
                        name = "OK"
                    }
                });
            },
        };
        event5 = new()
        {
            requirements = new()
            {
                new DayRequirement()
                {
                    day = 200.0f,
                },
            },
            action = () =>
            {
                Vars.Instance.ui.popups.ShowPopup("Tyranny", "There are rumors about the terrible atrocities of Jasmin Khushi", null, new()
                {
                    new()
                    {
                        name = "OK"
                    }
                });
            },
        };
        event6 = new()
        {
            requirements = new()
            {
                new DayRequirement()
                {
                    day = 300.0f,
                },
            },
            action = () =>
            {
                Vars.Instance.ui.popups.ShowPopup("Only a few left", "Due to the situation in the country, there are only a few large producers left, most of them are authorized by the state", null, new()
                {
                    new()
                    {
                        name = "OK"
                    }
                });
            },
        };
        event7 = new()
        {
            requirements = new()
            {
                new DayRequirement()
                {
                    day = 360.0f,
                },
            },
            action = () =>
            {
                Vars.Instance.ui.popups.ShowPopup("Kidnapping", "Recently, your country's leader was kidnapped by a terrorist organization from a neighboring country. It seems like a victory...", null, new()
                {
                    new()
                    {
                        name = "OK"
                    }
                });
            },
        };

        all = new()
        {
            event0, event1, event2, event3, event4, event5, event6, event7
        };
    }
}
