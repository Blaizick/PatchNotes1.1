using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlsSettingsUi : MonoBehaviour
{
    public GameObject root;
    public RectTransform rebindsRootTransform;
    public Button closeBtn;
    public RebindUiCntPfb rebindUiCntPfb;
    public GameObject rebindScreenRoot;

    public void Init()
    {
        rebindScreenRoot.SetActive(false);
        root.SetActive(false);

        closeBtn.onClick.AddListener(() => root.SetActive(false));
        
        var p = Vars.Instance.input.actions.Player;

        InitAction("Speed 1", p.Speed0);
        InitAction("Speed 2", p.Speed1);
        InitAction("Speed 3", p.Speed2);
        InitAction("Speed 4", p.Speed3);
        InitAction("Speed 5", p.Speed4);
        InitAction("Pause Time", p.Pause);
        InitAction("Break", p.Break);
        InitAction("Pause Menu", p.PauseMenu);
    }

    public void InitAction(string name, InputAction action)
    {
        if (action.bindings.Count <= 0)
        {
            return;
        }
        var bind = action.bindings[0];
        if (bind.isComposite || bind.isPartOfComposite)
        {
            return;
        }
                    
        var scr = Instantiate(rebindUiCntPfb, rebindsRootTransform);

        scr.rebindNameText.text = name;
        scr.curRebindText.text = InputControlPath.ToHumanReadableString(bind.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        scr.rebindBtn.onClick.AddListener(() =>
        {
            rebindScreenRoot.SetActive(true);
            Vars.Instance.rebinds.StartRebind(action, 0, () =>
            {
                rebindScreenRoot.SetActive(false);
                bind = action.bindings[0];
                scr.curRebindText.text = InputControlPath.ToHumanReadableString(bind.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            }, () =>
            {
                rebindScreenRoot.SetActive(false);
            });
        });
        scr.unbindBtn.onClick.AddListener(() =>
        {
            Vars.Instance.rebinds.Unbind(action, 0);
            scr.curRebindText.text = "None";
        });
        scr.resetBtn.onClick.AddListener(() =>
        {
            Vars.Instance.rebinds.Reset(action);
            bind = action.bindings[0];
            scr.curRebindText.text = InputControlPath.ToHumanReadableString(bind.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        });
    }
}

public class RebindSystem
{
    private const string PlayerPrefsRebindsKey = "rebinds";
    
    public InputActionAsset actions;

    public void Init()
    {
        actions = Vars.Instance.input.actions.asset;
        LoadRebinds();
    }

    public void Update()
    {
        
    }


    public void SaveRebinds()
    {
        var json = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(PlayerPrefsRebindsKey, json);
        PlayerPrefs.Save();
    }

    public void LoadRebinds()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsRebindsKey))
            return;

        var json = PlayerPrefs.GetString(PlayerPrefsRebindsKey);
        actions.LoadBindingOverridesFromJson(json);
    }

    public void ResetRebinds()
    {
        foreach (var map in actions.actionMaps)
            map.RemoveAllBindingOverrides();

        PlayerPrefs.DeleteKey(PlayerPrefsRebindsKey);
    }

    public void Unbind(InputAction action, int bindId)
    {
        action.ApplyBindingOverride(bindId, "");
        SaveRebinds();
    }

    public void StartRebind(InputAction action, int bindId, UnityAction onComplete, UnityAction onCancel)
    {
        action.Disable();

        action.PerformInteractiveRebinding(bindId)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(op =>
            {
                op.Dispose();
                action.Enable();
                SaveRebinds();
                onComplete?.Invoke();
            }).OnCancel(c => onCancel?.Invoke()).Start();
    }

    public void Reset(InputAction action)
    {
        action.RemoveBindingOverride(0);
        SaveRebinds();
    }
}
