using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class SerializedManagerSlot
{
    public string categoryName;
    public ManagerSlotUiCntPfb slot;
}

public class EmployeeUi : MonoBehaviour
{
    public GameObject root;

    public Button chefsMenuBtn;
    public Button managersMenuBtn;

    public GameObject chefsMenuRoot;
    public GameObject managersMenuRoot;

    public ChefUiCntPfb chefUiCntPfb;
    public AwailableChefUiCntPfb awailableChefUiCntPfb;

    public RectTransform chefsTableRootTransform;

    public GameObject infoDialogRoot;
    public TMP_Text infoDialogChefNameText;
    public TMP_Text infoDialogChefInfoText;
    public Button infoDialogHireChefBtn;
    public Button infoDialogFireChefBtn;
    public Button infoDialogCloseBtn;

    public GameObject awailableTableRoot;
    public RectTransform awailableTableRootTransform;
    public Button awailableTableCloseBtn;

    public Button hireBtn;

    [NonSerialized] public List<ChefUiCntPfb> instances = new();
    [NonSerialized] public List<AwailableChefUiCntPfb> awailableInstances = new();
    public Dictionary<Chef, ChefUiCntPfb> instancesDic = new();

    public Button closeBtn;

    public ManagerUiCntPfb managerUiCntPfb;

    public List<SerializedManagerSlot> slots;
    public Dictionary<ManagerCategory, ManagerSlotUiCntPfb> slotsDic = new();

    public RectTransform awailableManagersTableRoot;
    public GameObject awailableManagersRoot;
    public Button awailableManagersCloseBtn;
    [NonSerialized] public List<ManagerUiCntPfb> managerInstances = new();

    public TMP_Text awailableManagersLabel;

    public void Init()
    {
        chefsMenuRoot.SetActive(true);
        managersMenuRoot.SetActive(false);

        infoDialogRoot.SetActive(false);
        infoDialogCloseBtn.onClick.AddListener(() => infoDialogRoot.SetActive(false));
    
        awailableTableRoot.SetActive(false);
        hireBtn.onClick.AddListener(() => awailableTableRoot.SetActive(!awailableTableRoot.activeInHierarchy));
        awailableTableCloseBtn.onClick.AddListener(() => awailableTableRoot.SetActive(false));

        Vars.Instance.chefs.onChange.AddListener(() => RebuildChefsUi());
        RebuildChefsUi();

        chefsMenuBtn.onClick.AddListener(() =>
        {
            chefsMenuRoot.SetActive(true);
            managersMenuRoot.SetActive(false);
            awailableTableRoot.SetActive(false);
            infoDialogRoot.SetActive(false);
        });
        managersMenuBtn.onClick.AddListener(() =>
        {
            chefsMenuRoot.SetActive(false);
            managersMenuRoot.SetActive(true);
            awailableTableRoot.SetActive(false);
            infoDialogRoot.SetActive(false);
        });

        closeBtn.onClick.AddListener(() => root.SetActive(false));
    
        foreach (var s in slots)
        {
            var category = ManagerCategory.FromName(s.categoryName);
            slotsDic[category] = s.slot;
            s.slot.btn.onClick.AddListener(() =>
            {
                ShowAwailableManagers(ManagerType.allDic[category], $"Choose {category.name} manager", m =>
                {
                    Vars.Instance.managers.SetManager(category, m.AsManager());
                }, null);
            });
        }

        awailableManagersRoot.SetActive(false);
    }

    public void Update()
    {
        foreach (var (k, v) in slotsDic)
        {
            var m = Vars.Instance.managers.managers[k];
        }

        chefsMenuBtn.interactable = !chefsMenuRoot.activeInHierarchy;
        managersMenuBtn.interactable = !managersMenuRoot.activeInHierarchy;

        foreach (var (k, v) in instancesDic)
        {
            v.countText.text = $"{k.complexes.Count}/{k.maxComplexes}";
            v.selectionFrameRoot.SetActive(Vars.Instance.input.selectingComplexesForChef && Vars.Instance.input.selectedChef == k);
        }
    }

    public void ShowAwailableManagers(List<ManagerType> managers, string label, UnityAction<ManagerType> onSelected, UnityAction onQuit)
    {
        managerInstances.ForEach(i => Destroy(i.gameObject));
        managerInstances.Clear();

        awailableManagersCloseBtn.onClick.RemoveAllListeners();
        awailableManagersCloseBtn.onClick.AddListener(() =>
        {
            awailableManagersRoot.SetActive(false);
            onQuit?.Invoke();
        });

        awailableManagersLabel.text = label;

        foreach (var m in managers)
        {
            var scr = Instantiate(managerUiCntPfb, awailableManagersTableRoot);
            scr.btn.onClick.AddListener(() =>
            {
                awailableManagersRoot.SetActive(false);
                onSelected?.Invoke(m);
            });
            scr.name = m.name;
            managerInstances.Add(scr);
        }

        awailableManagersRoot.SetActive(true);
    }

    public void RebuildChefsUi()
    {
        awailableTableRoot.SetActive(false);
        infoDialogRoot.SetActive(false);

        instances.ForEach(i => Destroy(i.gameObject));
        instances.Clear();
        instancesDic.Clear();
        awailableInstances.ForEach(i => Destroy(i.gameObject));
        awailableInstances.Clear();

        foreach (var chef in Vars.Instance.chefs.chefs)
        {
            var script = Instantiate(chefUiCntPfb, chefsTableRootTransform);
            script.nameText.text = chef.type.name;
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
            var script = Instantiate(awailableChefUiCntPfb, awailableTableRootTransform);
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
        infoDialogRoot.SetActive(true);

        infoDialogHireChefBtn.onClick.RemoveAllListeners();
        infoDialogFireChefBtn.onClick.RemoveAllListeners();

        infoDialogHireChefBtn.onClick.AddListener(() => 
        {
            Vars.Instance.chefs.HireChef(chefType);
            infoDialogRoot.SetActive(false);
        });
        infoDialogFireChefBtn.onClick.AddListener(() =>
        {
            Vars.Instance.chefs.FireChef(chef);
            infoDialogRoot.SetActive(false);
        });

        infoDialogHireChefBtn.gameObject.SetActive(hireBtn);
        infoDialogFireChefBtn.gameObject.SetActive(fireBtn);

        infoDialogChefInfoText.text = $"Salary {(int)chefType.salary}\n" +
                                          $"Max Effeciency +{(int)chefType.maxEffeciencyMultiplier}%\n" +
                                          $"Effeciency Grow +{(int)chefType.effeciencyGrowMultiplier}%\n";
    
        infoDialogChefNameText.text = chefType.name;
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

public class ManagersSystem
{
    public Dictionary<ManagerCategory, List<ManagerType>> awailableManagers;

    public Dictionary<ManagerCategory, Manager> managers;

    public UnityEvent onChange = new();

    public void Init()
    {
        Restart();
    }

    public void Restart()
    {
        awailableManagers = new();
        managers = new();
        foreach (var i in ManagerCategory.all)
        {
            managers[i] = null;
        }
        onChange?.Invoke();
    }

    public void Update()
    {
        
    }

    public void SetManager(ManagerCategory category, Manager manager)
    {
        managers[category] = manager;
        onChange?.Invoke();
    }
}

public class ManagerCategory
{
    public string name;

    public static ManagerCategory cfo;
    public static ManagerCategory coo;
    public static ManagerCategory cto;

    public static List<ManagerCategory> all;

    public static void GInit()
    {
        cfo = new()
        {
            name = "CFO"    
        };
        coo = new()
        {
            name = "COO"
        };
        cto = new()
        {
            name = "CTO"            
        };

        all = new()
        {
            cfo, coo, cto
        };
    }

    public static ManagerCategory FromName(string name)
    {
        return all.Find(i => i.name == name);
    }
}

public class Manager
{
    public ManagerType type;
}

public class ManagerType
{
    public string name;
    public ManagerCategory category;

    public float maxEffeciencyBonus;
    public float maxEffeciencyMultiplier;
    public float effeciencyGrowBonus;
    public float effeciencyGrowMultiplier;
    public float researchSpeedBonus;
    public float researchSpeedMultiplier;

    public static Dictionary<ManagerCategory, List<ManagerType>> allDic = new();
    public static List<ManagerType> all = new();


    public Manager AsManager()
    {
        return new Manager()
        {
            type = this
        };
    }

    public static void GInit()
    {
        new ManagerType()
        {
            category = ManagerCategory.cfo
        }.Init();
        new ManagerType()
        {
            category = ManagerCategory.cfo
        }.Init();
        new ManagerType()
        {
            category = ManagerCategory.cfo
        }.Init();

        new ManagerType()
        {
            category = ManagerCategory.coo
        }.Init();
        new ManagerType()
        {
            category = ManagerCategory.coo
        }.Init();
        new ManagerType()
        {
            category = ManagerCategory.coo
        }.Init();

        new ManagerType()
        {
            category = ManagerCategory.cto
        }.Init();
        new ManagerType()
        {
            category = ManagerCategory.cto
        }.Init();
        new ManagerType()
        {
            category = ManagerCategory.cto
        }.Init();
    }

    public ManagerType Init()
    {
        all.Add(this);
        List<ManagerType> l;
        if (!allDic.TryGetValue(category, out l))
        {
            l = new();
            allDic[category] = l;
        }
        l.Add(this);
        return this;
    }
}