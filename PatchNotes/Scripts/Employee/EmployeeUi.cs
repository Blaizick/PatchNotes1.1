using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
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

    public RectTransform chefsTableRootTransform;

    public GameObject awailableTableRoot;
    public RectTransform awailableTableRootTransform;
    public Button awailableTableCloseBtn;

    public Button hireBtn;

    [NonSerialized] public List<ChefUiCntPfb> takenInstances = new();
    [NonSerialized] public List<ChefUiCntPfb> awailableInstances = new();
    public Dictionary<Chef, ChefUiCntPfb> takenInstancesDic = new();
    public Dictionary<ChefType, ChefUiCntPfb> awailableInstancesDic = new();

    public Button closeBtn;

    public ManagerUiCntPfb managerUiCntPfb;

    public List<SerializedManagerSlot> slots;
    public Dictionary<ManagerCategory, ManagerSlotUiCntPfb> slotsDic = new();

    public RectTransform awailableManagersTableRoot;
    public GameObject awailableManagersRoot;
    public Button awailableManagersCloseBtn;
    [NonSerialized] public List<ManagerUiCntPfb> managerInstances = new();
    public Dictionary<ManagerType, ManagerUiCntPfb> managerInstancesDic = new();

    public TMP_Text awailableManagersLabel;

    public void Init()
    {
        chefsMenuRoot.SetActive(true);
        managersMenuRoot.SetActive(false);

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
        });
        managersMenuBtn.onClick.AddListener(() =>
        {
            chefsMenuRoot.SetActive(false);
            managersMenuRoot.SetActive(true);
            awailableTableRoot.SetActive(false);
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
        root.SetActive(false);
    }

    public void Update()
    {
        foreach (var (k, v) in slotsDic)
        {
            var m = Vars.Instance.managers.managers[k];
            if (m == null)
            {
                v.image.sprite = null;
                v.tooltipInfoCnt.enabled = false;
            }
            else
            {
                v.image.sprite = m.type.sprite;
                v.tooltipInfoCnt.enabled = true;
                SetManagerTooltip(m.type, v.tooltipInfoCnt);
            }
        }

        chefsMenuBtn.interactable = !chefsMenuRoot.activeInHierarchy;
        managersMenuBtn.interactable = !managersMenuRoot.activeInHierarchy;

        foreach (var (k, v) in takenInstancesDic)
        {
            foreach (var state in v.AllStates)
            {
                state.text.text = $"{k.complexes.Count}/{k.maxComplexes}";
            }
            v.selectionFrameRoot.SetActive(Vars.Instance.input.selectingComplexesForChef && Vars.Instance.input.selectedChef == k);
        }
        foreach (var (k, v) in awailableInstancesDic)
        {
            bool t = Vars.Instance.influence.HasEnought(k.influencePrice);
            v.awailableState.root.SetActive(t);
            v.unawailableState.root.SetActive(!t);
        }

        foreach (var (k, v) in managerInstancesDic)
        {
            foreach (var state in v.AllStates)
            {
                state.image.sprite = k.sprite;
                state.root.SetActive(false);
            }
            if (Vars.Instance.managers.HasSameManager(k))
            {
                v.takenState.root.SetActive(true);                
            }
            else
            {
                bool t = Vars.Instance.influence.HasEnought(k.influencePrice);
                v.awailableState.root.SetActive(t);
                v.unawailableState.root.SetActive(!t);    
            }
        }
    }


    public void ShowAwailableManagers(List<ManagerType> managers, string label, UnityAction<ManagerType> onSelected, UnityAction onQuit)
    {
        managerInstancesDic.Clear();
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
            foreach (var state in scr.AllStates)
            {
                state.btn.onClick.AddListener(() =>
                {
                    awailableManagersRoot.SetActive(false);
                    onSelected?.Invoke(m);
                });
                state.text.text = ((int)m.influencePrice).ToString();
                state.root.SetActive(false);
            }
            SetManagerTooltip(m, scr.tooltipInfoCnt);
            managerInstancesDic[m] = scr;
            managerInstances.Add(scr);
        }

        awailableManagersRoot.SetActive(true);
    }

    public void SetManagerTooltip(ManagerType type, TooltipInfoCnt tooltipInfoCnt)
    {
        tooltipInfoCnt.title = type.name;
        tooltipInfoCnt.desc = $"Price: {type.influencePrice} influence\n\n";
        if (type.modifiers != null)
        {
            foreach (var m in type.modifiers)
            {
                tooltipInfoCnt.desc += $"{m}";
            }    
        }
    }

    public void RebuildChefsUi()
    {
        awailableTableRoot.SetActive(false);

        takenInstances.ForEach(i => Destroy(i.gameObject));
        takenInstances.Clear();
        takenInstancesDic.Clear();
        awailableInstances.ForEach(i => Destroy(i.gameObject));
        awailableInstances.Clear();
        awailableInstancesDic.Clear();

        foreach (var chef in Vars.Instance.chefs.chefs)
        {
            var script = Instantiate(chefUiCntPfb, chefsTableRootTransform);
            foreach (var state in script.AllStates)
            {
                state.btn.onClick.AddListener(() =>
                {
                    Vars.Instance.input.SwitchSelectingComplexesForChefState(chef);
                });
                state.image.sprite = chef.type.sprite;
                state.root.SetActive(false);
            }
            script.takenState.root.SetActive(true);
            SetChefTooltip(chef.type, script.tooltipInfoCnt);
            takenInstances.Add(script);
            takenInstancesDic[chef] = script;
        }

        foreach (var chef in Vars.Instance.chefs.awailableChefs)
        {
            var script = Instantiate(chefUiCntPfb, awailableTableRootTransform);
            foreach (var state in script.AllStates)
            {
                state.btn.onClick.AddListener(() =>
                {
                    Vars.Instance.chefs.HireChef(chef);
                });
                state.text.text = ((int)chef.influencePrice).ToString();
                state.root.SetActive(false);
                state.image.sprite = chef.sprite;
            }
            SetChefTooltip(chef, script.tooltipInfoCnt);
            awailableInstances.Add(script);
            awailableInstancesDic[chef] = script;
        }
    }

    public void SetChefTooltip(ChefType chef, TooltipInfoCnt tooltipInfoCnt)
    {
        tooltipInfoCnt.title = chef.name;
        tooltipInfoCnt.desc = $"Price: {chef.influencePrice} influence\n\n";
        if (chef.maxEffeciencyBonus != 0.0f)
        {
            tooltipInfoCnt.desc += $"Max Effeciency: +{chef.maxEffeciencyBonus}\n";
        }
        if (chef.maxEffeciencyMultiplier != 0.0f)
        {
            tooltipInfoCnt.desc += $"Max Effeciency: +{(int)(chef.maxEffeciencyMultiplier * 100.0f)}%\n";
        }
        if (chef.effeciencyGrowBonus != 0.0f)
        {
            tooltipInfoCnt.desc += $"Effeciency Grow: +{chef.effeciencyGrowBonus}\n";
        }
        if (chef.effeciencyGrowMultiplier != 0.0f)
        {
            tooltipInfoCnt.desc += $"Effeciency Grow: +{(int)(chef.effeciencyGrowMultiplier * 100.0f)}%\n";
        }
    }
}

public class ChefsSystem
{
    public List<ChefType> awailableChefs = new();

    public List<Chef> chefs = new();

    public UnityEvent onChange = new();

    public void Init()
    {
        Restart();
    }

    public void Restart()
    {
        chefs.Clear();
        awailableChefs = new(Chefs.all);
    }

    public void Update()
    {
        foreach (var chef in chefs)
        {
            chef.Update();
        }
    }

    public void HireChef(ChefType chef)
    {
        if (Vars.Instance.influence.HasEnought(chef.influencePrice))
        {
            Vars.Instance.influence.Take(chef.influencePrice);
            awailableChefs.Remove(chef);
            chefs.Add(chef.AsChef());
            onChange?.Invoke();
        }
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
            if (i.affectedByChef)
            {
                i.effeciencySystem.maxEffeciencyBonus += type.maxEffeciencyBonus;
                i.effeciencySystem.maxEffeciencyMultiplier += type.maxEffeciencyMultiplier;
                i.effeciencySystem.effeciencyGrowBonus += type.effeciencyGrowBonus;
                i.effeciencySystem.effeciencyGrowMultiplier += type.effeciencyGrowMultiplier;
            
                i.affectedByChef = true;
            }
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

    public float influencePrice;

    public float maxEffeciencyBonus;
    public float maxEffeciencyMultiplier;
    public float effeciencyGrowBonus;
    public float effeciencyGrowMultiplier;

    public Sprite sprite;

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
        Vars.Instance.influence.Take(manager.type.influencePrice);
        if (managers[category] != null)
        {
            managers[category].OnLeave();
        }
        manager.OnTake();
        managers[category] = manager;
        onChange?.Invoke();
    }

    public bool HasSameManager(ManagerType type)
    {
        var m = managers[type.category];
        if (m == null && type == null)
            return true;
        if (m == null || type == null)
            return false;
        if (m.type == type)
            return true;
        return false;
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

    public void OnTake()
    {
        if (type.modifiers != null)
        {
            foreach (var m in type.modifiers)
            {
                Vars.Instance.modifiers.AddModifier(m);
            }
        }
    }
    public void OnLeave()
    {
        if (type.modifiers != null)
        {
            foreach (var m in type.modifiers)
            {
                Vars.Instance.modifiers.RemoveModifier(m);
            }
        }
    }
}