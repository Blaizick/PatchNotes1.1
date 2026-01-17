using System.Collections.Generic;
using UnityEngine;

public class BuildingsSystem : MonoBehaviour
{
    public BuildSpot buildSpotPfb;

    public List<GameObject> buildings = new();

    public Vector2 anchor;
    public int buildingsInRow;
    public Vector2 spacing;

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

        AddBuildSpot();
        AddBuildSpot();
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
                go.transform.position = new Vector2(anchor.x + column * spacing.x, anchor.y + row * spacing.y);
            }
        }       
    } 
    public void AddBuildSpot()
    {
        var script = Instantiate(buildSpotPfb);
        script.onConvert.AddListener(complex =>
        {
            var id = buildings.IndexOf(script.gameObject);
            if (id >= 0)
            {
                buildings[id] = complex.gameObject;
            }
        });
        buildings.Add(script.gameObject);
        script.Init();
    }
}