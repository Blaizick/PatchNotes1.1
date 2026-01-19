using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMAin : MonoBehaviour
{
    public TMP_Text moneyText;

    public TMP_Text orderMoneyText;
    public TMP_Text orderTimeText;

    public Button resourcesBtn;
    public GameObject resourcesRoot;
    public RectTransform detailsContentRootTransform;
    public DetailUIContainerPrefab detailUIContainerPrefab;

    public GameObject winScreenRoot;
    public GameObject loseScreenRoot;

    public Button winScreenRestartButton;
    public Button loseScreenRestartButton;

    public GameObject buildComplexDialogRoot;
    public RectTransform buildComplexDialogContentRootTransform;
    public BuildComplexUICntPfb buildComplexPfb;
    [NonSerialized] public List<BuildComplexUICntPfb> complexInstances = new();

    public ResearchUI researchUI;

    public TMP_Text timeText;
    public GameObject speedRunStateRoot;
    public GameObject speedPauseStateRoot;
    public Image speedFiller;
    public Button nextSpeedBtn;
    public Button prevSpeedBtn;
    public Button pauseBtn;

    public CooperationUi cooperationUi;

    public Button buildComplexDialogBackButton;

    public GameObject confirmDialogRoot;
    public TMP_Text confirmDialogText;
    public Button confirmDialogConfirmButton;
    public Button confirmDialogBackButton;

    public Button chefsMenuBtn;

    public Button resourcesCloseBtn;

    [NonSerialized] public Dictionary<DetailType, DetailUIContainerPrefab> detailInstances = new();

    public EmployeeUi employeeUi;

    public TMP_Text influenceText;

    public ReportsUi reportsUi;
    public Button reportsBtn;

    public void Init()
    {
        foreach (var d in Details.all)
        {
            var script = Instantiate(detailUIContainerPrefab, detailsContentRootTransform);
            script.nameText.text = d.name;
            script.sellAllBtn.onClick.AddListener(() =>
            {
                Vars.Instance.detailsSystem.SellAll(d);
            });
            detailInstances[d] = script;
        }

        winScreenRestartButton.onClick.AddListener(() => Vars.Instance.Restart());
        loseScreenRestartButton.onClick.AddListener(() => Vars.Instance.Restart());

        buildComplexDialogRoot.SetActive(false);

        researchUI.Init();

        nextSpeedBtn.onClick.AddListener(() => Vars.Instance.speedSystem.NextSpeed());
        prevSpeedBtn.onClick.AddListener(() => Vars.Instance.speedSystem.PrevSpeed());

        pauseBtn.onClick.AddListener(() => Vars.Instance.speedSystem.ChangePauseState());

        resourcesBtn.onClick.AddListener(() => SetMenuActive(resourcesRoot, !resourcesRoot.activeInHierarchy));
        cooperationUi.ordersBtn.onClick.AddListener(() => 
            SetMenuActive(cooperationUi.root, !cooperationUi.root.activeInHierarchy));
        researchUI.openResearchMenuBtn.onClick.AddListener(() => 
            SetMenuActive(researchUI.researchMenuRoot, !researchUI.researchMenuRoot.activeInHierarchy));
        chefsMenuBtn.onClick.AddListener(() => SetMenuActive(employeeUi.root, !employeeUi.root.activeInHierarchy));
        reportsBtn.onClick.AddListener(() => SetMenuActive(reportsUi.root, !reportsUi.root.activeInHierarchy));

        cooperationUi.Init();

        buildComplexDialogBackButton.onClick.AddListener(() => buildComplexDialogRoot.SetActive(false));

        employeeUi.Init();

        resourcesCloseBtn.onClick.AddListener(() => resourcesRoot.SetActive(false));
    
        reportsUi.Init();
    }

    public void Update()
    {
        var money = Vars.Instance.moneySystem.money;
        moneyText.text = ((int)money).ToString();
    
        var targetMoney = ((MoneyOrderType)Vars.Instance.orders.curOrder.type).requiredMoney;
        orderMoneyText.text = $"{(int)Mathf.Clamp(money, 0, targetMoney)}/{(int)targetMoney}";
        orderTimeText.text = $"{(int)Vars.Instance.orders.TimeLeft}";
    
        winScreenRoot.SetActive(Vars.Instance.state.IsWin);
        loseScreenRoot.SetActive(Vars.Instance.state.IsLose);
    
        speedRunStateRoot.SetActive(!Vars.Instance.speedSystem.pause);            
        speedPauseStateRoot.SetActive(Vars.Instance.speedSystem.pause);            
        if (!Vars.Instance.speedSystem.pause)
        {
            speedFiller.fillAmount = (Vars.Instance.speedSystem.speedId + 1) / (float)Vars.Instance.speedSystem.speeds.Count;            
        }
        timeText.text = ((int)Vars.Instance.time.day).ToString();
    
        foreach (var (k, v) in detailInstances)
        {
            v.countText.text = ((int)Vars.Instance.detailsSystem.GetCount(k)).ToString();
        }

        influenceText.text = $"{(int)Vars.Instance.influence.influence}";
    }

    public void RebuildBuildComplexDialog(List<ComplexType> complexes, UnityAction<ComplexType> onSuccess)
    {
        buildComplexDialogRoot.SetActive(true);
        
        foreach (var i in complexInstances)
        {
            Destroy(i.gameObject);
        }
        complexInstances.Clear();

        foreach (var i in complexes)
        {
            var script = Instantiate(buildComplexPfb, buildComplexDialogContentRootTransform);
        
            script.nameText.text = i.name;
            script.buildBtn.onClick.AddListener(() =>
            {
                onSuccess(i);
            });

            complexInstances.Add(script);
        }
    }

    public void ShowConfirmDialog(string text, UnityAction onConfirm, UnityAction onCancel)
    {
        if (confirmDialogRoot.activeInHierarchy)
        {
            confirmDialogBackButton.onClick?.Invoke();
        }

        confirmDialogBackButton.onClick.RemoveAllListeners();
        confirmDialogConfirmButton.onClick.RemoveAllListeners();
        
        confirmDialogConfirmButton.onClick.AddListener(() =>
        {
            confirmDialogRoot.SetActive(false);
            onConfirm?.Invoke();
        });
        confirmDialogBackButton.onClick.AddListener(() =>
        {
            confirmDialogRoot.SetActive(false);
            onCancel?.Invoke();
        });

        confirmDialogText.text = text;

        confirmDialogRoot.SetActive(true);
    }

    public void SetMenuActive(GameObject menu, bool v)
    {
        resourcesRoot.SetActive(false);
        researchUI.researchMenuRoot.SetActive(false);
        cooperationUi.root.SetActive(false);
        employeeUi.root.SetActive(false);
        reportsUi.root.SetActive(false);
        menu.SetActive(v);
    }
}