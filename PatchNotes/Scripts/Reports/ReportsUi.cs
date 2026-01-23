using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReportsUi : MonoBehaviour
{
    public ReportUiCntPfb reportUiCntPfb;
    public ReportBlockUiCntPfb reportBlockUiCntPfb;

    [NonSerialized] public List<ReportUiCntPfb> reportInstances = new();
    [NonSerialized] public List<ReportBlockUiCntPfb> blockInstances = new();

    public RectTransform reportsContentRootTransform;

    public RectTransform curReportContentRootTransform;
    public Button closeBtn;
    public Button curReportCloseBtn;
    public GameObject curReportRoot;
    public GameObject root;

    public void Init()
    {
        Vars.Instance.reports.onGlobalChange.AddListener(Rebuild);
        Rebuild();
    
        curReportCloseBtn.onClick.AddListener(() => curReportRoot.SetActive(false));
        closeBtn.onClick.AddListener(() => root.SetActive(false));

        root.SetActive(false);
        curReportRoot.SetActive(false);
    }

    public void Update()
    {
        
    }

    public void Rebuild()
    {
        reportInstances.ForEach(i => Destroy(i.gameObject));
        reportInstances.Clear();
        foreach (var report in Vars.Instance.reports.all)
        {
            var scr = Instantiate(reportUiCntPfb, reportsContentRootTransform);
            scr.nameText.text = report.name;
            scr.btn.onClick.AddListener(() =>
            {
                blockInstances.ForEach(b => Destroy(b.gameObject));
                blockInstances.Clear();
                SpawnIncomeBlock($"Orders: {report.orders}");
                foreach (var (k, v) in report.detailIncomes)
                {
                    if (v > 0)
                    {
                        SpawnIncomeBlock($"{k.name}: {(int)v}({(int)k.price} each)");
                    }
                }
                SpawnExpenseBlock($"Materials: {(int)report.totalMaterialsExpense}({(int)report.materialPrice} each)");
                SpawnExpenseBlock($"Income Tax: {(int)report.incomeTaxExpense}({(int)(report.incomeTax * 100.0f)}%)");
                SpawnBlock($"Total(Without Taxes): {(int)report.totalWithoutTaxes}", report.totalWithoutTaxes > 0, report.totalWithoutTaxes < 0, report.totalWithoutTaxes == 0);
                SpawnBlock($"Total: {(int)report.total}", report.total > 0, report.total < 0, report.total == 0);
                curReportRoot.SetActive(true);
            });
            reportInstances.Add(scr);
        }
    }

    public void SpawnExpenseBlock(string text)
    {
        SpawnBlock(text, false, true, false);
    }
    public void SpawnIncomeBlock(string text)
    {
        SpawnBlock(text, true, false, false);
    }
    public void SpawnBlock(string text, bool positive, bool negative, bool neutral)
    {
        var scr = Instantiate(reportBlockUiCntPfb, curReportContentRootTransform);
        scr.positiveState.root.SetActive(positive);
        scr.negativeState.root.SetActive(negative);
        scr.neutralState.root.SetActive(neutral);
        foreach (var s in scr.AllStates)
        {
            s.text.text = text;
        }
        blockInstances.Add(scr);
    }
}

public class Report
{
    public Dictionary<DetailType, float> detailIncomes = new();
    public float totalWithoutTaxes;
    public float total;
    public float orders;
    public float incomeTaxExpense;
    public float incomeTax;
    public string name;
    public float totalMaterialsExpense;
    public float materialPrice;    

    public void AddDetailIncome(DetailType detail, float income)
    {
        if (detailIncomes.ContainsKey(detail))
        {
            detailIncomes[detail] += income;
        }
        else
        {
            detailIncomes[detail] = income;
        }
    }
}

public class ReportsSystem
{
    public List<Report> all;
    public Report cur;

    public UnityEvent onGlobalChange = new();
    
    public float lastUpdateTime;

    public void Init()
    {
        Restart();
    }

    public void Restart()
    {
        cur = new()
        {
            name = ((int)Vars.Instance.time.day).ToString()
        };
        all = new() {cur};
        lastUpdateTime = Vars.Instance.time.month;
        onGlobalChange?.Invoke();
    }

    public void Update()
    {
        cur.incomeTax = Vars.Instance.taxes.IncomeTax;
        cur.incomeTaxExpense = Vars.Instance.income.GetIncomeTaxExpense();
        cur.totalWithoutTaxes = Vars.Instance.income.income;
        cur.total = Vars.Instance.income.IncomeWithIncomeTax;
    }

    public void MonthUpdate()
    {
        cur = new()
        {
            name = ((int)Vars.Instance.time.day).ToString()
        };
        all.Add(cur);
        onGlobalChange?.Invoke();
        lastUpdateTime = Vars.Instance.time.month;
    }
}