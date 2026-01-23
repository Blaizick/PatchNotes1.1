using System.Collections.Generic;
using UnityEngine;

public class BuildingsSystem : MonoBehaviour
{
    public BuildSpotComplex buildSpotPfb;
    public BuyBuildSpotComplex buyBuildSpotComplexPfb; 

    public List<GameObject> buildings = new();
    public List<Complex> complexes = new();

    public Vector2 anchor;
    public int buildingsInRow;
    public Vector2 spacing;

    public BuildingComplex buildingComplexPfb;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(anchor, 0.1f);
    }

    public void Init()
    {
        Restart();
    }   

    public void Restart()
    {
        buildings.ForEach(b => Destroy(b));
        buildings.Clear();
        complexes.Clear();

        AddBuildSpot();
        AddBuildSpot();
        AddBuildSpot();
        buildings.Add(SpawnBuyBuildSpot().gameObject);
    }

    public void Update()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            int row = i / buildingsInRow;
            int column = i % buildingsInRow;

            var go = buildings[i];
            if (go)
            {
                go.transform.position = new Vector2(anchor.x + column * spacing.x, anchor.y - row * spacing.y);
            }
        }       
    } 
    public void AddBuildSpot()
    {
        buildings.Add(SpawnBuildSpot().gameObject);
    }

    public void StartBuilding(BuildSpotComplex spot, ComplexType complexType)
    {
        var id = buildings.IndexOf(spot.gameObject);
        Destroy(buildings[id].gameObject);

        var buildingComplex = (BuildingComplex)Complexes.buildingComplex.AsComplex();
        buildingComplex.Init();
        buildings[id] = buildingComplex.gameObject;
        buildingComplex.buildingComplex = complexType;
        buildingComplex.Init();
        complexes.Add(buildingComplex);
        
    }
    public void FinishBuilding(BuildingComplex buildingComplex)
    {
        var id = buildings.IndexOf(buildingComplex.gameObject);
        complexes.Remove(buildingComplex);
        Destroy(buildings[id].gameObject);

        var complex = buildingComplex.buildingComplex.AsComplex();
        complex.Init();
        buildings[id] = complex.gameObject;
        complexes.Add(complex);        
    }

    public bool CanBreak(GameObject go)
    {
        return go.TryGetComponent<Complex>(out var c) && c.type.breakable;
    }
    public void Break(GameObject go)
    {
        var complex = go.GetComponent<Complex>();
        var id = buildings.IndexOf(complex.gameObject);
        if (id >= 0)
        {
            complexes.Remove(complex);
            Destroy(buildings[id]);
            buildings[id] = SpawnBuildSpot().gameObject;
        }
        buildings.Remove(go);
        Destroy(go);
    }
    
    public bool CanHaveChef(GameObject go)
    {
        return go != null && go.TryGetComponent<Complex>(out var c) && c.type.chefAllowed;
    }

    public BuildSpotComplex SpawnBuildSpot()
    {
        var scr = (BuildSpotComplex)Complexes.buildSpotComplex.AsComplex();
        scr.Init();
        return scr;
    }

    public void BuySpot(GameObject go)
    {
        var id = buildings.IndexOf(go);
        if (id >= 0)
        {
            Destroy(go);
            buildings[id] = SpawnBuildSpot().gameObject;
            buildings.Add(SpawnBuyBuildSpot().gameObject);
        }
    }

    public BuyBuildSpotComplex SpawnBuyBuildSpot()
    {
        var scr = (BuyBuildSpotComplex)Complexes.buyBuildSpotComplex.AsComplex();
        scr.Init();
        scr.price = Vars.Instance.buildSpotPrice.GetPrice();
        Vars.Instance.buildSpotPrice.OnBuy();
        return scr;
    }
}