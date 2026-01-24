using UnityEngine;
using UnityEngine.UI;

public class MenuUi : MonoBehaviour
{
    public Button closeBtn;
    public GameObject root;

    public ControlsSettingsUi controlsSettingsUi;

    public Button controlsBtn;
    public Button restartBtn;

    public void Init()
    {
        restartBtn.onClick.AddListener(() =>
        {
            Vars.Instance.Restart();
            root.SetActive(false);
            controlsSettingsUi.root.SetActive(false);
        });
        controlsBtn.onClick.AddListener(() =>
        {
            controlsSettingsUi.root.SetActive(!controlsSettingsUi.root.activeInHierarchy);
        });
        closeBtn.onClick.AddListener(() =>
        {
            root.SetActive(false);
            controlsSettingsUi.root.SetActive(false);
        });
        controlsSettingsUi.Init();
    }

    public void Update()
    {
        
    }
}