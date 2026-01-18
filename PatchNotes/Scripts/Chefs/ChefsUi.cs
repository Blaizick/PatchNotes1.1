using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChefsUi : MonoBehaviour
{
    public GameObject root;

    public ChefUiCntPfb chefUiCntPfb;
    public AwailableChefUiCntPfb awailableChefUiCntPfb;

    public RectTransform chefsTableRootTransform;

    public GameObject hireChefDialogRoot;
    public TMP_Text hireChefDialogChefNameText;
    public TMP_Text hireChefDialogChefInfoText;
    public Button hireChefDialogHireChefBtn;
    public Button hireChefDialogFireChefBtn;
    public Button hireChefDialogCloseBtn;

    public GameObject awailableChefsTableRoot;
    public RectTransform awailableChefsTableRootTransform;
    public Button awailableChefsTableCloseBtn;

    public Button hireBtn;

    [NonSerialized] public List<ChefUiCntPfb> instances = new();
    [NonSerialized] public List<AwailableChefUiCntPfb> awailableInstances = new();
    public Dictionary<Chef, ChefUiCntPfb> instancesDic = new();

    public void Init()
    {
        hireChefDialogRoot.SetActive(false);
        hireChefDialogCloseBtn.onClick.AddListener(() => hireChefDialogRoot.SetActive(false));
    
        awailableChefsTableRoot.SetActive(false);
        hireBtn.onClick.AddListener(() => awailableChefsTableRoot.SetActive(!awailableChefsTableRoot.activeInHierarchy));
        awailableChefsTableCloseBtn.onClick.AddListener(() => awailableChefsTableRoot.SetActive(false));

        Vars.Instance.chefs.onChange.AddListener(() => Rebuild());
        Rebuild();
    }

    public void Update()
    {
        foreach (var (k, v) in instancesDic)
        {
            v.selectionFrameRoot.SetActive(Vars.Instance.input.selectingComplexesForChef && Vars.Instance.input.selectedChef == k);
        }
    }

    public void Rebuild()
    {
        awailableChefsTableRoot.SetActive(false);
        hireChefDialogRoot.SetActive(false);

        instances.ForEach(i => Destroy(i.gameObject));
        instances.Clear();
        instancesDic.Clear();
        awailableInstances.ForEach(i => Destroy(i.gameObject));
        awailableInstances.Clear();

        foreach (var chef in Vars.Instance.chefs.chefs)
        {
            var script = Instantiate(chefUiCntPfb, chefsTableRootTransform);
            script.nameText.text = chef.type.name;
            script.countText.text = $"{chef.complexes.Count}/{chef.maxComplexes}";
            script.btn.onClick.AddListener(() =>
            {
                Vars.Instance.input.SwitchSelectingComplexesForChefState(chef);
            });
            script.infoBtn.onClick.AddListener(() =>
            {
                ShowHireChefDialog(chef, chef.type, false, true);
            });
            instances.Add(script);
            instancesDic[chef] = script;
        }

        foreach (var chef in Vars.Instance.chefs.awailableChefs)
        {
            var script = Instantiate(awailableChefUiCntPfb, awailableChefsTableRootTransform);
            script.nameText.text = chef.name;
            script.btn.onClick.AddListener(() =>
            {
                ShowHireChefDialog(null, chef, true, false);
            });
            awailableInstances.Add(script);
        }
    }

    public void ShowHireChefDialog(Chef chef, ChefType chefType, bool hireBtn, bool fireBtn)
    {
        hireChefDialogRoot.SetActive(true);

        hireChefDialogHireChefBtn.onClick.RemoveAllListeners();
        hireChefDialogFireChefBtn.onClick.RemoveAllListeners();

        hireChefDialogHireChefBtn.onClick.AddListener(() => 
        {
            Vars.Instance.chefs.HireChef(chefType);
            hireChefDialogRoot.SetActive(false);
        });
        hireChefDialogFireChefBtn.onClick.AddListener(() =>
        {
            Vars.Instance.chefs.FireChef(chef);
            hireChefDialogRoot.SetActive(false);
        });

        hireChefDialogHireChefBtn.gameObject.SetActive(hireBtn);
        hireChefDialogFireChefBtn.gameObject.SetActive(fireBtn);

        hireChefDialogChefInfoText.text = $"Salary {(int)chefType.salary}\n" +
                                          $"Max Effeciency +{(int)chefType.maxEffeciencyMultiplier}%\n" +
                                          $"Effeciency Grow +{(int)chefType.effeciencyGrowMultiplier}%\n";
    
        hireChefDialogChefNameText.text = chefType.name;
    }
}

public class ChefsSystem
{
    public List<ChefType> awailableChefs = new();

    public List<Chef> chefs = new();

    public UnityEvent onChange = new();

    public float lastUpdateTime = 0.0f;
    public float lastChefsPayMonth = 0.0f;

    public void Init()
    {
        Restart();
    }

    public void Restart()
    {
        chefs.Clear();
        lastUpdateTime = Vars.Instance.time.day;
        lastChefsPayMonth = Vars.Instance.time.month;
        UpdateAwailableChefs();
    }

    public void Update()
    {
        if (Vars.Instance.time.day - lastUpdateTime > 10.0f)
        {
            UpdateAwailableChefs();
            lastUpdateTime = Vars.Instance.time.day;
        }
        if (Vars.Instance.time.month - lastChefsPayMonth > 1.0f)
        {
            foreach (var chef in chefs)
            {
                Vars.Instance.moneySystem.Take(chef.type.salary);
            }
            lastChefsPayMonth = Vars.Instance.time.month;
        }

        foreach (var chef in chefs)
        {
            chef.Update();
        }
    }

    public void UpdateAwailableChefs()
    {
        awailableChefs.Clear();
        for (int i = 0; i < 5; i++)
        {
            var points = UnityEngine.Random.Range(10.0f, 30.0f);
            ChefType type = new()
            {
                salary = points * 5.0f,
                points = points,
                effeciencyGrowMultiplier = UnityEngine.Random.Range(0.0f, points),
            };
            type.maxEffeciencyMultiplier = points - type.effeciencyGrowMultiplier;
            awailableChefs.Add(type);
        }
        onChange?.Invoke();
    }

    public void HireChef(ChefType chef)
    {
        awailableChefs.Remove(chef);
        chefs.Add(chef.AsChef());
        onChange?.Invoke();
    }        
    public void FireChef(Chef chef)
    {
        chefs.Remove(chef);
        chef.dead = true;
        onChange?.Invoke();
    }
}

public class Chef
{
    public ChefType type;

    public List<Complex> complexes = new();
    public int maxComplexes = 5;

    public bool dead = false;

    public void Init()
    {
        
    }

    public void Update()
    {
        complexes.RemoveAll(i => !i);
        foreach (var i in complexes)
        {
            i.effeciencySystem.maxEffeciencyMultiplier += type.maxEffeciencyMultiplier;
            i.effeciencySystem.maxEffeciencyMultiplier += type.effeciencyGrowMultiplier;
        }
    }

    public bool TryAddComplex(Complex complex)
    {
        if (complexes.Count >= maxComplexes)
        {
            return false;
        }
        complexes.Add(complex);
        return true;
    }

    public void SwitchComplex(Complex complex)
    {
        if (complexes.Contains(complex))
            TryRemoveComplex(complex);
        else
            TryAddComplex(complex);
    }

    public bool TryRemoveComplex(Complex complex)
    {
        int id = complexes.IndexOf(complex);
        if (id < 0)
        {
            return false;
        }
        complexes.RemoveAt(id);
        return true;
    }
}

public class ChefType
{
    public string name = "Template Name";

    public float points;
    public float salary;

    public float maxEffeciencyMultiplier;
    public float effeciencyGrowMultiplier;

    public Chef AsChef()
    {
        var chef = new Chef()
        {
            type = this
        };
        chef.Init();
        return chef;
    }
}