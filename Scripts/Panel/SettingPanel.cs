using UnityEngine;
using UnityEngine.UI;

namespace Panel
{
    public class SettingPanel : BasePanel
    {
        private Button m_ReturnBtn;
        private Slider m_MusicVolumeSlider;
        private Slider musicEffectVolumeSlider;

        public override void Init()
        {
            base.Init();
            m_ReturnBtn = transform.Find("ReturnBtn").GetComponent<Button>();

            m_MusicVolumeSlider = transform.Find("Panel/MusicVolume/Slider").GetComponent<Slider>();
            musicEffectVolumeSlider = transform.Find("Panel/SoundEffectVolume/Slider").GetComponent<Slider>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //if (UIPanelManager.Instance.GetComponentInChildren<SavePanel>()
            //    && UIPanelManager.Instance.GetComponentInChildren<GameStartPanel>())
            //{
            //    ReturnBtn.gameObject.SetActive(!UIPanelManager.Instance.GetPanel(UIPanelType.GameStart).gameObject.activeSelf
            //    && !UIPanelManager.Instance.GetPanel(UIPanelType.Save).gameObject.activeSelf);
            //}
            //else
            //    ReturnBtn.gameObject.SetActive(false);
        
            m_ReturnBtn.onClick.AddListener(OnReturnBtnClick);
            m_MusicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        }

        public override void OnExit()
        {
            base.OnExit();
            m_ReturnBtn.onClick.RemoveListener(OnReturnBtnClick);
            m_MusicVolumeSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
        }

        private void OnReturnBtnClick()
        {
            //返回主界面自动保存
            GameSaveManager.Instance.SaveGame();
            UIPanelManager.Instance.PopPanel();
            UIPanelManager.Instance.PopPanel();
        }

        private void ChangeMusicVolume(float value)
        {
            GameObject musicGam = GameObject.Find("Function").gameObject;
            musicGam.GetComponent<AudioSource>().volume = (value / 4);
        }
    }
}