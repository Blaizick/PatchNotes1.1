using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUICntPfb : MonoBehaviour
{
    public GameObject awailableStateRoot;
    public GameObject takenStateRoot;
    
    public TMP_Text nameText;
    public Button btn;

    public Image awailableStateImg;
    public Image takenStateImg;

    public TMP_Text timeLeftText;
    public Image timeLeftFiller;

    public TooltipInfoCnt tooltipInfoCnt;
}