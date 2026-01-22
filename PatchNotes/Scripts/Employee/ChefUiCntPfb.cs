using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ChefUiState
{
    public Image image;
    public Button btn;
    public GameObject root;
    public TMP_Text text;
}

public class ChefUiCntPfb : MonoBehaviour
{
    public ChefUiState awailableState;
    public ChefUiState takenState;
    public ChefUiState unawailableState;

    public List<ChefUiState> AllStates => new (){awailableState, takenState, unawailableState};

    public GameObject selectionFrameRoot;

    public TooltipInfoCnt tooltipInfoCnt;
}
