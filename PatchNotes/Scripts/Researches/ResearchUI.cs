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
    [Space]

    public Button openResearchMenuBtn;

    [NonSerialized] public List<ResearchTechUICntPfb> instances = new();

    public void Init()
    {
        researchMenuRoot.SetActive(false);

        openResearchMenuBtn.onClick.AddListener(() => researchMenuRoot.SetActive(!researchMenuRoot.activeInHierarchy));
        researchSlot0Btn.onClick.AddListener(() =>
        {
            researchesScreenRoot.SetActive(!researchesScreenRoot.activeInHierarchy);
        });

        InitTechCnt(supplierTech, Researches.supplierTech);
        InitTechCnt(smelterTech, Researches.smelterTech);
    }

    public void Update()
    {
        var researches = Vars.Instance.researches;

        foreach (var i in instances)
        {
            i.btn.interactable = !researches.IsResearched(i.tech);
        }

        researchSlot0Txt.text = researches.research == null ? "Empty" : researches.research.name;
        researchSlot0Filler.fillAmount = researches.research == null ? 1.0f : researches.ResearchProgress;
    }

    public void InitTechCnt(ResearchTechUICntPfb cnt, ResearchTech tech)
    {
        cnt.nameText.text = tech.name;
        cnt.btn.onClick.AddListener(() =>
        {
            Vars.Instance.researches.StartResearch(tech);
            researchesScreenRoot.SetActive(false);
        });
        cnt.tech = tech;
        instances.Add(cnt);
    }
}