using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ResearchTechUiState
{
    public Button btn;
    public TMP_Text nameText;
    public GameObject root;
}

public class ResearchTechUICntPfb : MonoBehaviour
{
    public ResearchTechUiState researchedState;
    public ResearchTechUiState awailableState;
    public ResearchTechUiState unawailableState;

    public List<ResearchTechUiState> AllStates => new()
    {
        researchedState,
        awailableState,
        unawailableState,
    };

    public TooltipInfoCnt tooltipInfoCnt;
}