using UnityEngine;
using UnityEngine.UI;

namespace Panel
{
    public class StorePanel : BasePanel
    {
        private Button m_ReturnBtn;
        private Button m_SettingBtn;

        public override void Init()
        {
            base.Init();
            CanvasGroup = transform.GetComponent<CanvasGroup>();
            m_SettingBtn = transform.Find("SettingBtn").GetComponent<Button>();
            m_ReturnBtn = transform.Find("ReturnBtn").GetComponent<Button>();
        }

        public override void OnEnter(object intent = null)
        {
            base.OnEnter();
            m_ReturnBtn.onClick.AddListener(OnReturnButtonClick);
            m_SettingBtn.onClick.AddListener(OnSettingButtonClick);
        }

        public override void OnExit()
        {
            base.OnExit();
            m_ReturnBtn.onClick.RemoveListener(OnReturnButtonClick);
            m_SettingBtn.onClick.RemoveListener(OnSettingButtonClick);
        }

        private void OnReturnButtonClick()
        {
            UIPanelManager.Instance.PopPanel();
        }

        private void OnSettingButtonClick()
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.Setting);
        }
    }
}