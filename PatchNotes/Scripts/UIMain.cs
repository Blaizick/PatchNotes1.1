using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class UIMAin : MonoBehaviour
{
    public TMP_Text moneyText;

    public TMP_Text orderMoneyText;
    public TMP_Text orderTimeText;

    public TMP_Text detailsText;

    public RectTransform detailsRootTransform;
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

    public void Init()
    {
        foreach (var d in Details.all)
        {
            var script = Instantiate(detailUIContainerPrefab, detailsRootTransform);
            script.nameText.text = d.name;
            script.sellAllBtn.onClick.AddListener(() =>
            {
                Vars.Instance.detailsSystem.SellAll(d);
            });
        }

        winScreenRestartButton.onClick.AddListener(() => Vars.Instance.Restart());
        loseScreenRestartButton.onClick.AddListener(() => Vars.Instance.Restart());

        buildComplexDialogRoot.SetActive(false);

        researchUI.Init();
    }

    public void Update()
    {
        moneyText.text = ((int)Vars.Instance.moneySystem.money).ToString();
    
        orderMoneyText.text = $"{Mathf.Clamp(Vars.Instance.moneySystem.money, 0, Vars.Instance.orders.curOrder.money)}/{Vars.Instance.orders.curOrder.money}";
        orderTimeText.text = $"{(int)(Vars.Instance.orders.TimeLeft)} sec";
    
        StringBuilder sb = new();
        foreach (var (k, v) in Vars.Instance.detailsSystem.details)
        {
            sb.Append($"{k.name}: {(int)v}\n");
        }
        detailsText.text = sb.ToString();
    
        winScreenRoot.SetActive(Vars.Instance.state.IsWin);
        loseScreenRoot.SetActive(Vars.Instance.state.IsLose);
    
        researchUI.Update();

        timeText.text = ((int)Vars.Instance.time.day).ToString();
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
                // buildComplexDialogRoot.SetActive(false);
                // Instantiate(i.prefab);
            });

            complexInstances.Add(script);
        }
    }
}