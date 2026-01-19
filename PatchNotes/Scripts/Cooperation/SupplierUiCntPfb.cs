using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SupplierUiState
{
    public GameObject root;
    public TMP_Text influencePriceText;
    public Image image;
    public Button btn;
}

public class SupplierUiCntPfb : MonoBehaviour
{
    public SupplierUiState takenState;
    public SupplierUiState awailableState;
    public SupplierUiState unawailableStates;
    public List<SupplierUiState> AllStates => new()
    {
        takenState,
        awailableState,
        unawailableStates,
    };
}