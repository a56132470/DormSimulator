using UnityEngine;
using UnityEngine.UI;

namespace Panel
{
    public class SavePanel : BasePanel
    {
        private Button m_ReturnBtn;
        private Button m_SettingBtn;
        private Button[] m_DeleteSaveBtns;

        private Button[] m_SaveBtns;
        private Player[] m_Players;
        private Roommate[][] m_Roommates;

        private GameObject m_CreateSaveTipPanel;
        private Button m_Cst_CancelBtn;
        private Button m_Cst_DetermineBtn;

        private GameObject m_CreateRolePanel;
        private Button m_Crp_DetermineBtn;
        private InputField m_Crp_CreateNameInputField;

        // 当前点击的按钮的ID
        private int m_Count;

        public override void OnEnter(object intent = null)
        {
            base.OnEnter();
            gameObject.SetActive(true);

            m_ReturnBtn.onClick.AddListener(OnReturnButtonClick);
            m_SettingBtn.onClick.AddListener(OnSettingButtonClick);

            m_SaveBtns[0].onClick.AddListener(() => { OnSaveBtnClick(0); });
            m_SaveBtns[1].onClick.AddListener(() => { OnSaveBtnClick(1); });
            m_SaveBtns[2].onClick.AddListener(() => { OnSaveBtnClick(2); });
            // CST:是否创建角色
            m_Cst_CancelBtn.onClick.AddListener(OnCstCancelBtnClick);
            m_Cst_DetermineBtn.onClick.AddListener(OnCstDetermineBtnClick);
            // CRP:创建角色
            m_Crp_DetermineBtn.onClick.AddListener(OnCrpDetermineBtnClick);
        }

        public override void OnExit()
        {
            base.OnExit();

            m_ReturnBtn.onClick.RemoveListener(OnReturnButtonClick);
            m_SettingBtn.onClick.RemoveListener(OnSettingButtonClick);
            for (int i = 0; i < 3; i++)
            {
                // m_DeleteSaveBtns[i].onClick.RemoveAllListeners();
                m_SaveBtns[i].onClick.RemoveAllListeners();
            }

            // CST:是否创建角色
            m_Cst_CancelBtn.onClick.RemoveListener(OnCstCancelBtnClick);
            m_Cst_DetermineBtn.onClick.RemoveListener(OnCstDetermineBtnClick);
            // CRP:创建角色
            m_Crp_DetermineBtn.onClick.RemoveListener(OnCrpDetermineBtnClick);
        }

        public override void OnResume()
        {
            base.OnResume();
            Init();
        }

        private void OnReturnButtonClick()
        {
            UIPanelManager.Instance.PopPanel();
        }

        private void OnSettingButtonClick()
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.Setting);
        }

        public override void Init()
        {
            base.Init();
            m_SettingBtn = transform.Find("SettingBtn").GetComponent<Button>();
            m_ReturnBtn = transform.Find("ReturnBtn").GetComponent<Button>();
            m_SaveBtns = transform.Find("Saves").GetComponentsInChildren<Button>();

            m_CreateSaveTipPanel = transform.Find("CreateSaveTipPanel").gameObject;
            m_Cst_CancelBtn = m_CreateSaveTipPanel.transform.Find("CST_CancelBtn").GetComponent<Button>();
            m_Cst_DetermineBtn = m_CreateSaveTipPanel.transform.Find("CST_DetermineBtn").GetComponent<Button>();

            m_CreateRolePanel = transform.Find("CreateRolePanel").gameObject;
            m_Crp_DetermineBtn = m_CreateRolePanel.transform.Find("CRP_DetermineBtn").GetComponent<Button>();
            m_Crp_CreateNameInputField = m_CreateRolePanel.GetComponentInChildren<InputField>();


            m_Roommates = new Roommate[3][];
            m_Roommates[0] = new Roommate[3];
            m_Roommates[1] = new Roommate[3];
            m_Roommates[2] = new Roommate[3];
            m_Players = new Player[3];

            LoadLocalSaves();

        }
        private void LoadLocalSaves()
        {
            for (var i = 0; i < 3; i++)
            {
                m_Players[i] = new Player();
                for (var j = 0; j < 3; j++)
                {
                    m_Roommates[i][j] = new Roommate();
                }
                GameSaveManager.Instance.LoadGame(ref m_Players[i], ref m_Roommates[i], i);
                if (m_Players[i] != null && !m_Players[i].Name.Equals(""))
                {
                    // 读取成功
                    m_SaveBtns[i].transform.Find("Yes").gameObject.SetActive(true);
                    m_SaveBtns[i].transform.Find("Yes/Week").GetComponent<Text>().text = "第" + m_Players[i].CurWeek.ToString() + "周目";
                    m_SaveBtns[i].transform.Find("Yes/Round").GetComponent<Text>().text = "第" + m_Players[i].CurRound.ToString() + "回合";
                    m_SaveBtns[i].transform.Find("Yes/Name").GetComponent<Text>().text = m_Players[i].Name;

                    m_SaveBtns[i].transform.Find("No").gameObject.SetActive(false);
                }
                else
                {
                    m_SaveBtns[i].transform.Find("No").gameObject.SetActive(true);
                    m_SaveBtns[i].transform.Find("Yes").gameObject.SetActive(false);
                }
            }
        }
        private void OnSaveBtnClick(int count)
        {
            m_Count = count;
            if (m_SaveBtns[m_Count].transform.Find("Yes").gameObject.activeSelf)
            {
                GlobalManager.Instance.player = m_Players[m_Count];
                GlobalManager.Instance.roommates = m_Roommates[m_Count];
                GlobalManager.Instance.saveId = m_Count;
                GameSaveManager.Instance.LoadInventoryOnStart();
                GameObject.Find("Fungus/Characters/P").GetComponent<Fungus.Character>().NameText = GlobalManager.Instance.player.Name;
                if (GlobalManager.Instance.player.CurRound >= 24)
                {
                    UIPanelManager.Instance.PushPanel(UIPanelType.End);
                    PlotManager.Instance.ExecuteMainBlock("Game_End");
                }
                else
                {
                    UIPanelManager.Instance.PushPanel(UIPanelType.MainMenu);
                }

            }
            else if (m_SaveBtns[m_Count].transform.Find("No").gameObject.activeSelf)
            {
                // 创建角色
                CreateSaveTipPanelShow();
            }
        }

        private void CreateSaveTipPanelShow()
        {
            // Show
            if (!m_CreateSaveTipPanel.activeSelf)
            {
                m_SaveBtns[0].transform.parent.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                m_CreateSaveTipPanel.SetActive(true);
            }
        }

        private void OnCstCancelBtnClick()
        {
            // Disappear
            m_SaveBtns[0].transform.parent.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

            m_CreateSaveTipPanel.SetActive(false);
        }

        private void OnCstDetermineBtnClick()
        {
            m_CreateSaveTipPanel.SetActive(false);
            m_CreateRolePanel.SetActive(true);
        }

        private void OnCrpDetermineBtnClick()
        {
            string playerName = m_Crp_CreateNameInputField.text;
            if (playerName.Equals(""))
            {
                UIPanelManager.Instance.PushPanel(UIPanelType.Tip);
                EventCenter.Broadcast(EventType.UPDATE_TIP, "请您输入名字");
            }
            else
            {
                EventCenter.Broadcast(EventType.GAME_INIT, playerName, 1, m_Count);

                GameSaveManager.Instance.SaveGame();
                m_CreateRolePanel.SetActive(false);
                m_Crp_CreateNameInputField.text = "";
                m_SaveBtns[0].transform.parent.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                // 进行一次存档操作
                UIPanelManager.Instance.PushPanel(UIPanelType.MainMenu);
                PlotManager.Instance.ShowMain();
            }
        }
    }
}