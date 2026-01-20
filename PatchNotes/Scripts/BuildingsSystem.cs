using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingsSystem : MonoBehaviour
{
    public BuildSpot buildSpotPfb;
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

    public void StartBuilding(BuildSpot spot, ComplexType complexType)
    {
        var id = buildings.IndexOf(spot.gameObject);
        Destroy(buildings[id].gameObject);
        var buildingComplex = Instantiate(buildingComplexPfb);
        buildings[id] =  buildingComplex.gameObject;
        complexes.Add(buildingComplex);
        buildingComplex.buildingComplex = complexType;
        buildingComplex.Init();
    }
    public void FinishBuilding(BuildingComplex buildingComplex)
    {
        var id = buildings.IndexOf(buildingComplex.gameObject);
        complexes.Remove(buildingComplex);
        Destroy(buildings[id].gameObject);
        var complex = SpawnComplex(buildingComplex.buildingComplex);
        buildings[id] = complex.gameObject;
        complexes.Add(complex);        
    }

    public bool CanBreak(GameObject go)
    {
        return go.TryGetComponent<Complex>(out var c) && c.CanBreak;
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

    public BuildSpot SpawnBuildSpot()
    {
        var script = Instantiate(buildSpotPfb);
        // script.onConvert.AddListener(complex =>
        // {
        //     complexes.Add(complex);
        //     var id = buildings.IndexOf(script.gameObject);
        //     if (id >= 0)
        //     {
        //         buildings[id] = complex.gameObject;
        //     }
        // });
        script.Init();
        return script;
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
        var script = Instantiate(buyBuildSpotComplexPfb);
        script.Init();
        script.price = Vars.Instance.buildSpotPriceSystem.GetPrice();
        Vars.Instance.buildSpotPriceSystem.OnBuy();
        return script;
    }

    public Complex SpawnComplex(ComplexType complexType)
    {
        var scr = Instantiate(complexType.prefab);
        scr.type = complexType;
        scr.Init();
        return scr;
    }
}