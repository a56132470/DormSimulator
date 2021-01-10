using UnityEngine;
using UnityEngine.UI;

namespace Panel
{
    public class EndPanel : BasePanel
    {
        private Button m_EndButton;

        private GameObject m_CreateSaveTipPanel;
        private Button m_CST_CancelBtn;
        private Button m_CST_DetermineBtn;

        private GameObject m_CreateRolePanel;
        private Button m_CRP_DetermineBtn;
        private InputField m_CRP_CreateNameInputField;

        public override void Init()
        {
            base.Init();
            transform.Find("Text").GetComponent<Text>();
            m_EndButton = transform.Find("ImageBtn").GetComponent<Button>();

            m_CreateSaveTipPanel = transform.Find("CreateSaveTipPanel").gameObject;
            m_CST_CancelBtn = m_CreateSaveTipPanel.transform.Find("CST_CancelBtn").GetComponent<Button>();
            m_CST_DetermineBtn = m_CreateSaveTipPanel.transform.Find("CST_DetermineBtn").GetComponent<Button>();

            m_CreateRolePanel = transform.Find("CreateRolePanel").gameObject;
            m_CRP_DetermineBtn = m_CreateRolePanel.transform.Find("CRP_DetermineBtn").GetComponent<Button>();
            m_CRP_CreateNameInputField = m_CreateRolePanel.GetComponentInChildren<InputField>();
        }

        public override void OnEnter(object intent = null)
        {
            base.OnEnter();

            // CST 是否创建角色
            m_CST_CancelBtn.onClick.AddListener(OnCSTCancelBtnClick);
            m_CST_DetermineBtn.onClick.AddListener(OnCSTDetermineBtnClick);
            // CRP:创建角色
            m_CRP_DetermineBtn.onClick.AddListener(OnCRPDetermineBtnClick);
            m_EndButton.onClick.AddListener(OnEndBtnClick);
        }

        public override void OnExit()
        {
            base.OnExit();

            m_CST_CancelBtn.onClick.RemoveListener(OnCSTCancelBtnClick);
            m_CST_DetermineBtn.onClick.RemoveListener(OnCSTDetermineBtnClick);

            m_CRP_DetermineBtn.onClick.RemoveListener(OnCRPDetermineBtnClick);
            m_EndButton.onClick.RemoveListener(OnEndBtnClick);
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

        private void OnEndBtnClick()
        {
            m_CreateSaveTipPanel.SetActive(true);
        }

        private void OnCSTCancelBtnClick()
        {
            // Disappear
            m_CreateSaveTipPanel.SetActive(false);
        }

        private void OnCSTDetermineBtnClick()
        {
            m_CreateSaveTipPanel.SetActive(false);
            m_CreateRolePanel.SetActive(true);
        }

        private void OnCRPDetermineBtnClick()
        {
            string playerName = m_CRP_CreateNameInputField.text;
            if (playerName.Equals(""))
            {
                UIPanelManager.Instance.PushPanel(UIPanelType.Tip);
                EventCenter.Broadcast(EventType.UPDATE_TIP, "请您输入名字");
            }
            else
            {
                EventCenter.Broadcast(EventType.GAME_INIT, playerName, GlobalManager.Instance.player.CurWeek + 1, GlobalManager.Instance.saveId);
                EventCenter.Broadcast(EventType.UPDATE_ACTIONPANEL_EVENT);
                GameSaveManager.Instance.SaveGame();
                m_CreateRolePanel.SetActive(false);
                m_CRP_CreateNameInputField.text = "";
                PlotManager.Instance.ExecuteMainBlock("StartStory");
                // 进行一次存档操作
                UIPanelManager.Instance.PopPanel();
            }
        }
    }
}