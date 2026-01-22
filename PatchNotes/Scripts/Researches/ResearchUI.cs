using System;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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
            researchesScreenRoot.SetActive(!researchesScreenRoot.activeInHierarchy);
        });

        InitTechCnt(supplyTech, Researches.supply);
        InitTechCnt(smeltingTech, Researches.smelting);
        InitTechCnt(pressingTech, Researches.pressing);
        InitTechCnt(bending0Tech, Researches.bending0);
        InitTechCnt(cuttingTech, Researches.cutting);
        InitTechCnt(reinforcingTech, Researches.reinforcing);
        InitTechCnt(bending1Tech, Researches.bending1);
        InitTechCnt(formingTech, Researches.forming);
        InitTechCnt(assemblingTech, Researches.assembling);
        
        InitTechCnt(productionTech, Researches.production);

        InitTechCnt(standardizedComplexes0Tech, Researches.standardizedComplexes0);
        InitTechCnt(standardizedComplexes1Tech, Researches.standardizedComplexes1);
        InitTechCnt(standardizedComplexes2Tech, Researches.standardizedComplexes2);
        
        InitTechCnt(flexibleComplexes0Tech, Researches.flexibleComplexes0);
        InitTechCnt(flexibleComplexes1Tech, Researches.flexibleComplexes1);
        InitTechCnt(flexibleComplexes2Tech, Researches.flexibleComplexes2);

        InitTechCnt(researching0Tech, Researches.researching0);

        backButton.onClick.AddListener(() => researchesScreenRoot.SetActive(false));
        menuCloseBtn.onClick.AddListener(() => 
        {
            researchMenuRoot.SetActive(false);
            researchesScreenRoot.SetActive(false);
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