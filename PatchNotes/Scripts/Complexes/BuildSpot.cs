using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class BuildSpot : MonoBehaviour, IPointerClickHandler
{
    public BuildSpotType type;

    public UnityEvent<Complex> onConvert = new();

    public void Init()
    {
        type = BuildSpots.spot0;
    }

    public void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        List<ComplexType> complexes = new();
        foreach (var c in Complexes.all)
        {
            if (Vars.Instance.researches.IsResearched(c.research))
            {
                complexes.Add(c);
            }
        }
        Vars.Instance.ui.RebuildBuildComplexDialog(complexes, c =>
        {
            // var script = Instantiate(c.prefab);
            // script.type = c;
            // script.transform.position = transform.position;
            // script.Init();

            // onConvert?.Invoke(script);
            
            Vars.Instance.buildSystem.StartBuilding(this, c);
            Vars.Instance.ui.buildComplexDialogRoot.SetActive(false);
            // Destroy(gameObject);
        });
    }
}