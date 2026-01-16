using UnityEngine;

public class ProductionComplex : MonoBehaviour
{
    public void Init()
    {
        
    }

    public void Update()
    {
        Vars.Instance.detailsSystem.Add(Details.ironPlate, 0.5f * Time.deltaTime);
    }
}