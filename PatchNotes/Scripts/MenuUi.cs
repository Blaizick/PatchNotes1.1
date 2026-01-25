using UnityEngine;
using UnityEngine.UI;

public class MenuUi : MonoBehaviour
{
    public Button closeBtn;
    public GameObject root;

    public ControlsSettingsUi controlsSettingsUi;

    public Button controlsBtn;
    public Button restartBtn;

    public Slider soundGlobalVolumeSlider;
    public GameObject soundRoot;
    public Button soundBtn;
    public Button closeSoundBtn;

    public void Init()
    {
        restartBtn.onClick.AddListener(() =>
        {
            Vars.Instance.audioManager.Play(Sounds.uiClick);
            Vars.Instance.Restart();
            root.SetActive(false);
            controlsSettingsUi.root.SetActive(false);
        });
        controlsBtn.onClick.AddListener(() =>
        {
            Vars.Instance.audioManager.Play(Sounds.uiClick);
            controlsSettingsUi.root.SetActive(!controlsSettingsUi.root.activeInHierarchy);
        });
        closeBtn.onClick.AddListener(() =>
        {
            Vars.Instance.audioManager.Play(Sounds.uiClick);
            root.SetActive(false);
            soundRoot.SetActive(false);
            controlsSettingsUi.root.SetActive(false);
        });
        closeSoundBtn.onClick.AddListener(() =>
        {
            soundRoot.SetActive(false);
            Vars.Instance.audioManager.Play(Sounds.uiClick);
        });
        soundBtn.onClick.AddListener(() =>
        {
            Vars.Instance.audioManager.Play(Sounds.uiClick);
            soundRoot.SetActive(true);
        });
        controlsSettingsUi.Init();
    
        soundRoot.SetActive(false);

        soundGlobalVolumeSlider.minValue = 0.0f;
        soundGlobalVolumeSlider.maxValue = 100.0f;
        soundGlobalVolumeSlider.onValueChanged.AddListener(v =>
        {
            Vars.Instance.audioManager.GlobalVolume = v;
        });

        root.SetActive(false);
    }

    public void Update()
    {
        soundGlobalVolumeSlider.value = Vars.Instance.audioManager.GlobalVolume;
    }
}