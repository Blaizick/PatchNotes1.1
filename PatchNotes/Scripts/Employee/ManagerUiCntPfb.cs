using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ManagerUiState
{
    public TMP_Text text;
    public Button btn;
    public Image image;
    public GameObject root;
}

public class ManagerUiCntPfb : MonoBehaviour
{
    public ManagerUiState awailableState;
    public ManagerUiState unawailableState;
    public ManagerUiState takenState;
    public List<ManagerUiState> AllStates => new(){awailableState, unawailableState, takenState};
    public TooltipInfoCnt tooltipInfoCnt;
}