using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    private Button ReturnBtn;
    private Slider musicVolumeSlider;
    private Slider musicEffectVolumeSlider;

    public override void Init()
    {
        base.Init();
        ReturnBtn = transform.Find("ReturnBtn").GetComponent<Button>();

        musicVolumeSlider = transform.Find("Panel/MusicVolume/Slider").GetComponent<Slider>();
        musicEffectVolumeSlider = transform.Find("Panel/SoundEffectVolume/Slider").GetComponent<Slider>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        ReturnBtn.gameObject.SetActive(!UIPanelManager.Instance.GetPanel(UIPanelType.GameStart).gameObject.activeSelf);
        ReturnBtn.onClick.AddListener(OnReturnBtnClick);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
    }

    public override void OnExit()
    {
        base.OnExit();
        ReturnBtn.onClick.RemoveListener(OnReturnBtnClick);
        musicVolumeSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
    }

    private void OnReturnBtnClick()
    {
        //返回主界面自动保存
        GameSaveManager.instance.SaveGame();
        UIPanelManager.Instance.PopPanel();
        UIPanelManager.Instance.PopPanel();
    }

    private void ChangeMusicVolume(float value)
    {
        GameObject MusicGam = GameObject.Find("Function").gameObject;
        MusicGam.GetComponent<AudioSource>().volume = (value / 4);
    }
}