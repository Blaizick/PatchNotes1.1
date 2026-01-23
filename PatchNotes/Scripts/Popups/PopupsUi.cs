using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupsUi : MonoBehaviour
{
    public PopupOptionUiCntPfb popupOptionUiCntPfb;
    public Popup popupUiCntPfb; 
    public RectTransform root;

    [NonSerialized] public List<Popup> instances = new();
    public void Init()
    {
        
    }

    public void Restart()
    {
        foreach (var i in instances)
        {
            Destroy(i.gameObject);
        }
        instances.Clear();
    }

    public void ShowPopup(string title, string desc, UnityAction onClose, List<PopupOption> options)
    {
        var scr = Instantiate(popupUiCntPfb, root);
        scr.titleText.text = title;
        scr.descText.text = desc;
        scr.closeBtn.onClick.AddListener(() =>
        {
            instances.Remove(scr);
            Destroy(scr.gameObject);
            onClose?.Invoke();
        });
        if (options != null)
        {
            foreach (var o in options)
            {
                var s = Instantiate(popupOptionUiCntPfb, scr.optionsRootTransform);
                s.btn.onClick.AddListener(() =>
                {
                    instances.Remove(scr);
                    Destroy(scr.gameObject);
                    o.onChoose?.Invoke();
                });
                s.text.text = o.name;
                s.tooltipInfoCnt.title = o.tooltipName;
                s.tooltipInfoCnt.desc = o.tooltipDesc;
            }
        }
        instances.Add(scr);
        scr.Init();
    }
}

public class PopupOption
{
    public string name;
    public string tooltipName;
    public string tooltipDesc;
    public UnityAction onChoose;
}