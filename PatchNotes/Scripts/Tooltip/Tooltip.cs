using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public RectTransform rootTransform;
    public GameObject root;

    public TMP_Text titleText;
    public TMP_Text descText;

    public List<LayoutElement> layoutElements = new();

    public int characterWrapLimit = 100;

    public void Init()
    {
        Hide();
    }

    public void Update()
    {
        int titleLen = string.IsNullOrEmpty(titleText.text) ? 0 : titleText.text.Length;
        int descLen = string.IsNullOrEmpty(descText.text) ? 0 : descText.text.Length;
        foreach (var i in layoutElements)
        {
            i.enabled = (titleLen > characterWrapLimit || descLen > characterWrapLimit) ? true : false;            
        }
    }

    public void Show(string title, string desc)
    {
        Set(title, desc);
        root.SetActive(true);
    }
    public void Set(string title, string desc)
    {
        titleText.text = title;
        descText.text = desc;        
    }
    public void Hide()
    {
        root.SetActive(false);
    }

    public void SetPosition(Vector2 pos)
    {
        rootTransform.anchoredPosition = pos;
    }
}