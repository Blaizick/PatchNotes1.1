using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResearchUI : MonoBehaviour
{
    public GameObject researchesScreenRoot;

    public GameObject researchMenuRoot;

    public Button researchSlot0Btn;
    public TMP_Text  researchSlot0Txt;
    public Image researchSlot0Filler;

    [Space]
    public ResearchTechUICntPfb supplyTech;
    public ResearchTechUICntPfb smeltingTech;
    public ResearchTechUICntPfb pressingTech;
    public ResearchTechUICntPfb bending0Tech;
    public ResearchTechUICntPfb cuttingTech;
    public ResearchTechUICntPfb reinforcingTech;
    public ResearchTechUICntPfb bending1Tech;
    public ResearchTechUICntPfb formingTech;
    public ResearchTechUICntPfb assemblingTech;

    public ResearchTechUICntPfb productionTech;

    public ResearchTechUICntPfb standardizedComplexes0Tech;
    public ResearchTechUICntPfb standardizedComplexes1Tech;
    public ResearchTechUICntPfb standardizedComplexes2Tech;

    public ResearchTechUICntPfb flexibleComplexes0Tech;
    public ResearchTechUICntPfb flexibleComplexes1Tech;
    public ResearchTechUICntPfb flexibleComplexes2Tech;

    public ResearchTechUICntPfb researching0Tech;

    [Space]

    public Button openResearchMenuBtn;

    [NonSerialized] public Dictionary<ResearchTech, ResearchTechUICntPfb> instancesDic = new();
    [NonSerialized] public List<ResearchTechUICntPfb> instances = new();

    public Button backButton;
    public Button menuCloseBtn;

    public TMP_Text researchSpeedBuffText;
    public TMP_Text researchTimeText;

    public void Init()
    {
        researchMenuRoot.SetActive(false);

        researchSlot0Btn.onClick.AddListener(() =>
        {
            Vars.Instance.audioManager.Play(Sounds.uiClick);
            researchesScreenRoot.SetActive(!researchesScreenRoot.activeInHierarchy);
        });

        InitTechCnt(supplyTech, Researches.supply);
        InitTechCnt(smeltingTech, Researches.smelting);
        InitTechCnt(pressingTech, Researches.pressing);
        InitTechCnt(bending0Tech, Researches.bending0);
        InitTechCnt(cuttingTech, Researches.cutting);
        InitTechCnt(reinforcingTech, Researches.reinforcing);
        InitTechCnt(bending1Tech, Researches.bending1);
        InitTechCnt(formingTech, Researches.shaping);
        InitTechCnt(assemblingTech, Researches.assembling);
        
        InitTechCnt(productionTech, Researches.production);

        InitTechCnt(standardizedComplexes0Tech, Researches.standardizedComplexes0);
        InitTechCnt(standardizedComplexes1Tech, Researches.standardizedComplexes1);
        InitTechCnt(standardizedComplexes2Tech, Researches.standardizedComplexes2);
        
        InitTechCnt(flexibleComplexes0Tech, Researches.flexibleComplexes0);
        InitTechCnt(flexibleComplexes1Tech, Researches.flexibleComplexes1);
        InitTechCnt(flexibleComplexes2Tech, Researches.flexibleComplexes2);

        InitTechCnt(researching0Tech, Researches.researching0);

        backButton.onClick.AddListener(() => 
        {
            researchesScreenRoot.SetActive(false);
            Vars.Instance.audioManager.Play(Sounds.uiClick);
        });
        menuCloseBtn.onClick.AddListener(() => 
        {
            Vars.Instance.audioManager.Play(Sounds.uiClick);
            researchMenuRoot.SetActive(false);
            researchesScreenRoot.SetActive(false);
        });

        Vars.Instance.researches.onResearch.AddListener(r =>
        {
            Vars.Instance.ui.popups.ShowPopup(r.name, "Researched", null, new()
            {
                new()
                {
                    name = "OK"
                }
            });
        });
    }

    public void Update()
    {
        var researches = Vars.Instance.researches;

        foreach (var (k, v) in instancesDic)
        {
            foreach (var i in v.AllStates)
            {
                i.root.SetActive(false);
            }

            if (researches.IsResearched(k))
            {
                v.researchedState.root.SetActive(true);
            }
            else if (researches.CanStartResearch(k))
            {
                v.awailableState.root.SetActive(true);
            }
            else
            {
                v.unawailableState.root.SetActive(true);
            }

            v.tooltipInfoCnt.title = k.name;
            v.tooltipInfoCnt.desc = k.GetDesc();
        }

        researchSpeedBuffText.text = $"+{(int)(Vars.Instance.researches.ResearchSpeed * 100)}%";

        researchSlot0Txt.text = researches.research == null ? "Empty" : researches.research.name;
        if (researches.research == null)
        {
            researchSlot0Filler.fillAmount = researches.savedResearchTime / ResearchSystem.MaxSavedResearchTime;
        }
        else
        {
            researchSlot0Filler.fillAmount = researches.researchProgress;
        }

        researchTimeText.text = researches.research == null ? string.Empty : $"{(int)researches.DaysLeft} days";
    }

    public void InitTechCnt(ResearchTechUICntPfb cnt, ResearchTech tech)
    {
        foreach (var state in cnt.AllStates)
        {
            state.nameText.text = tech.name;
            state.btn.onClick.AddListener(() =>
            {
                Vars.Instance.audioManager.Play(Sounds.uiClick);
                if (Vars.Instance.researches.research != tech)
                {
                    Vars.Instance.researches.StartResearch(tech);
                }
                researchesScreenRoot.SetActive(false);
            });
        }
        
        instances.Add(cnt);
        instancesDic[tech] = cnt;
    }
}

public class ResearchTech
{
    /// <summary>
    /// Days to research tech
    /// </summary>
    public float researchTime;
    public string name;
    public List<ResearchTech> requiredTechs;
    public List<ResearchTech> exclusive;

    public virtual void Research() {}

    public virtual string GetDesc() => $"Research Time: {(int)Vars.Instance.researches.GetTechResearchTime(researchTime, Vars.Instance.researches.TimeAsProgress(Vars.Instance.researches.savedResearchTime, researchTime))} days\n\n";
}

public class ComplexResearchTech : ResearchTech
{
    public ComplexType unlock;

    public override string GetDesc()
    {
        string str = base.GetDesc();
        if (unlock != null)
        {
            str += $"Unlocks: {unlock.GetName()}\n";
            str += $"{unlock.GetDesc()}";
        }
        return str;
    }
}

public class ModifiersResearchTech : ResearchTech
{
    public List<Modifier> modifiers;

    public override void Research()
    {
        if (modifiers != null)
        {
            foreach (var modifier in modifiers)
            {
                Vars.Instance.modifiers.AddModifier(modifier);
                // modifier.Apply();
            }
        }
    }

    public override string GetDesc()
    {
        string str = base.GetDesc();
        if (modifiers != null)
        {
            foreach (var m in modifiers)
            {
                str += $"{m}";
            }    
        }
        return str;
    }
}

public class ResearchSystem
{
    public ResearchTech research;
    public float researchProgress;
    public float savedResearchTime;
    public const float MaxSavedResearchTime = 30.0f;

    public List<ResearchTech> awailableTechs = new();
    public List<ResearchTech> researched;

    public UnityEvent<ResearchTech> onResearch = new();

    public void Init()
    {
        Restart();
    }

    public void Restart()
    {
        research = null;
        researched = new()
        {
            Researches.supply,
            Researches.smelting
        };    
        UpdateAwailableResearches();
        savedResearchTime = 0.0f;
    }

    public void Update()
    {
        if (research == null)
        {
            savedResearchTime += Vars.Instance.time.deltaDay;
            savedResearchTime = Mathf.Clamp(savedResearchTime, 0, MaxSavedResearchTime);
        }
        else
        {
            researchProgress += TimeAsProgress(Vars.Instance.time.deltaDay);
            if (researchProgress > 1.0f)
            {
                onResearch?.Invoke(research);
                research.Research();
                researched.Add(research);
                research = null;
                UpdateAwailableResearches();
            }
        }
    }

    public void UpdateAwailableResearches()
    {
        awailableTechs.Clear();
        foreach (var tech in Researches.all)
        {
            if (!researched.Contains(tech))
            {
                if (tech.requiredTechs != null)
                {
                    bool can = true;
                    foreach (var req in tech.requiredTechs)
                    {
                        if (!researched.Contains(req))
                        {
                            can = false;
                            break;
                        }
                    }
                    if (!can)
                    {
                        continue;
                    }
                }
                if (tech.exclusive != null)
                {
                    bool can = true;
                    foreach (var exc in tech.exclusive)
                    {
                        if (researched.Contains(exc))
                        {
                            can = false;
                            break;
                        }
                    }
                    if (!can)
                    {
                        continue;
                    }
                }
                awailableTechs.Add(tech);
            }
        }
    }

    public bool IsResearched(ResearchTech tech)
    {
        return researched.Contains(tech);
    }

    public void StartResearch(ResearchTech tech)
    {
        research = tech;
        researchProgress = TimeAsProgress(savedResearchTime);
        savedResearchTime = 0.0f;
    }

    public bool CanStartResearch(ResearchTech tech)
    {
        return awailableTechs.Contains(tech);
    }
    public float TimeAsProgress(float t, float researchTime)
    {
        return t / researchTime * ResearchSpeed;
    }
    public float TimeAsProgress(float t)
    {
        return TimeAsProgress(t, research.researchTime);
    }

    public float ResearchSpeed => 1 + Vars.Instance.modifiers.GetBonus<ResearchSpeedModifier>();
    public float DaysLeft => GetTechResearchTime(research.researchTime, researchProgress);

    public float GetTechResearchTime(float timeReq, float progress)
    {
        return (1.0f - progress) * timeReq / ResearchSpeed;
    }
}
