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
        ShowPopup(new()
        {
            title = title,
            desc = desc,
            onClose = onClose,
            options = options,
        });
    }
    public void ShowPopup(PopupInfo popupInfo)
    {
        if (popupInfo == null) return;

        var scr = Instantiate(popupUiCntPfb, root);
        scr.titleText.text = popupInfo.title;
        scr.descText.text = popupInfo.desc;
        scr.closeBtn.onClick.AddListener(() =>
        {
            instances.Remove(scr);
            Destroy(scr.gameObject);
            popupInfo.onClose?.Invoke();
            Vars.Instance.audioManager.Play(Sounds.uiClick);
        });

        if (popupInfo.options != null)
        {
            foreach (var o in popupInfo.options)
            {
                var s = Instantiate(popupOptionUiCntPfb, scr.optionsRootTransform);
                s.btn.onClick.AddListener(() =>
                {
                    instances.Remove(scr);
                    Destroy(scr.gameObject);
                    o.onChoose?.Invoke();
                    Vars.Instance.audioManager.Play(Sounds.uiClick);
                });
                s.text.text = o.name;
                s.tooltipInfoCnt.title = o.tooltipName;
                s.tooltipInfoCnt.desc = o.tooltipDesc;
            }
        }
        scr.Init();
        
        instances.Add(scr);
        Vars.Instance.audioManager.Play(Sounds.uiPopUp);
    }
}

public class PopupOption
{
    public string name;
    public string tooltipName;
    public string tooltipDesc;
    public UnityAction onChoose;
}

public class PopupInfo
{
    public string title;
    public string desc;
    public UnityAction onClose;
    public List<PopupOption> options;
}