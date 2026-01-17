using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BuildSpot : MonoBehaviour, IPointerClickHandler
{
    public BuildSpotType type;

    public void Init()
    {
        type = BuildSpots.spot0;
    }

    public void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        List<ComplexType> complexes = new();
        foreach (var c in type.buildComplexes)
        {
            if (Vars.Instance.researches.IsResearched(c.research))
            {
                complexes.Add(c);
            }
        }
        Vars.Instance.ui.RebuildBuildComplexDialog(complexes, c =>
        {
            var go = Instantiate(c.prefab);
            go.transform.position = transform.position;
            go.GetComponent<Complex>().Init();
            
            Vars.Instance.ui.buildComplexDialogRoot.SetActive(false);
            Destroy(gameObject);
        });
    }
}