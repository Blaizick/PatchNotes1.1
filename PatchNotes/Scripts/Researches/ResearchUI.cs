using System;
using System.Collections.Generic;
using TMPro;
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
    public ResearchTechUICntPfb supplierTech;
    public ResearchTechUICntPfb smelterTech;
    public ResearchTechUICntPfb pressTech;


    public ResearchTechUICntPfb productionTech;

    public ResearchTechUICntPfb standardizedComplexes0Tech;
    public ResearchTechUICntPfb standardizedComplexes1Tech;
    public ResearchTechUICntPfb standardizedComplexes2Tech;

    public ResearchTechUICntPfb flexibleComplexes0Tech;
    public ResearchTechUICntPfb flexibleComplexes1Tech;
    public ResearchTechUICntPfb flexibleComplexes2Tech;

    [Space]

    public Button openResearchMenuBtn;


    [NonSerialized] public Dictionary<ResearchTech, ResearchTechUICntPfb> instancesDic = new();
    [NonSerialized] public List<ResearchTechUICntPfb> instances = new();

    public Button backButton;

    public void Init()
    {
        researchMenuRoot.SetActive(false);

        researchSlot0Btn.onClick.AddListener(() =>
        {
            researchesScreenRoot.SetActive(!researchesScreenRoot.activeInHierarchy);
        });

        InitTechCnt(supplierTech, Researches.supplier);
        InitTechCnt(smelterTech, Researches.smelter);
        InitTechCnt(pressTech, Researches.press);
        
        InitTechCnt(productionTech, Researches.production);

        InitTechCnt(standardizedComplexes0Tech, Researches.standardizedComplexes0);
        InitTechCnt(standardizedComplexes1Tech, Researches.standardizedComplexes1);
        InitTechCnt(standardizedComplexes2Tech, Researches.standardizedComplexes2);
        
        InitTechCnt(flexibleComplexes0Tech, Researches.flexibleComplexes0);
        InitTechCnt(flexibleComplexes1Tech, Researches.flexibleComplexes1);
        InitTechCnt(flexibleComplexes2Tech, Researches.flexibleComplexes2);

        backButton.onClick.AddListener(() => researchesScreenRoot.SetActive(false));
    }

    public void Update()
    {
        var researches = Vars.Instance.researches;

        foreach (var (k, v) in instancesDic)
        {
            v.btn.interactable = researches.CanStartResearch(k);
        }

        researchSlot0Txt.text = researches.research == null ? "Empty" : researches.research.name;
        researchSlot0Filler.fillAmount = researches.research == null ? 1.0f : researches.ResearchProgress;
    }

    public void InitTechCnt(ResearchTechUICntPfb cnt, ResearchTech tech)
    {
        cnt.nameText.text = tech.name;
        cnt.btn.onClick.AddListener(() =>
        {
            if (Vars.Instance.researches.research != tech)
            {
                Vars.Instance.researches.StartResearch(tech);
            }
            researchesScreenRoot.SetActive(false);
        });
        cnt.tech = tech;
        instances.Add(cnt);
        instancesDic[tech] = cnt;
    }
}