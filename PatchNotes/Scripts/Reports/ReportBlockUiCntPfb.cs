using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
public class ReportBlockUiState
{
    public TMP_Text text;
    public GameObject root;
}

public class ReportBlockUiCntPfb : MonoBehaviour
{
    public ReportBlockUiState positiveState;
    public ReportBlockUiState negativeState;
    public ReportBlockUiState neutralState;
    public List<ReportBlockUiState> AllStates => new()
    {
        positiveState,
        negativeState,
        neutralState 
    };
    public enum StatesEnum
    {
        Positive,
        Negative,
        Neutral
    }
}