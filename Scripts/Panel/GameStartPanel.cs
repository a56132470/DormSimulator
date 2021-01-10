using UnityEngine;
using UnityEngine.UI;

namespace Panel
{
    public class GameStartPanel : BasePanel
    {
        private Button m_StartButton;
        private Button m_SettingButton;

        private void Update()
        {
            // 点击esc退出
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public override void Init()
        {
            base.Init();
            m_StartButton = transform.Find("StartBtn").GetComponent<Button>();
            m_SettingButton = transform.Find("SettingBtn").GetComponent<Button>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            m_StartButton.onClick.AddListener(OnStartButtonClick);
            m_SettingButton.onClick.AddListener(OnSettingButtonClick);
        }

        public override void OnExit()
        {
            base.OnExit();
            m_StartButton.onClick.RemoveListener(OnStartButtonClick);
            m_SettingButton.onClick.RemoveListener(OnSettingButtonClick);
        }
        public override void OnPause()
        {
            base.OnPause();
            gameObject.SetActive(false);
        }
        public override void OnResume()
        {
            base.OnResume();
            gameObject.SetActive(true);
        }
        /// <summary>
        /// 开始游戏按钮
        /// </summary>
        private void OnStartButtonClick()
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.Save);
        }

        /// <summary>
        /// 设置按钮
        /// </summary>
        private void OnSettingButtonClick()
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.Setting);
        }
    }
}