using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Panel
{
    public class RoommatePanel : BasePanel
    {
        [SerializeField]
        private GameObject m_PropertyPanel;

        public Sprite[] sprites;
        [FormerlySerializedAs("m_PropertyTxts")] [SerializeField]
        // 属性文本
        // 0:Name
        // 1:Logic
        // 2:Talk
        // 3:Athletics
        // 4:Creativity
        // 5:Money
        // 6:RelationShip
        // 7:SelfControl
        private Text[] propertyTxts = new Text[8];

        private Toggle m_State_Toggle;
        private Toggle m_Record_Toggle;
        private Button m_Invite_Btn;
        private Image m_ProfileImg;
        private GameObject m_StatePage;
        private GameObject m_RecordPage;

        private int m_Id;


        public override void Init()
        {
            base.Init();
            m_ProfileImg = transform.Find("ProfilePanel").GetComponent<Image>();
            m_PropertyPanel = transform.Find("PropertyPanel").gameObject;
            propertyTxts = m_PropertyPanel.GetComponentsInChildren<Text>();
            // 状态,记录单选框
            m_State_Toggle = transform.Find("ToggleBtns/StateToggle").GetComponent<Toggle>();
            m_Record_Toggle = transform.Find("ToggleBtns/RecordToggle").GetComponent<Toggle>();
            // 邀请按钮
            m_Invite_Btn = transform.Find("ToggleBtns/InviteBtn").GetComponent<Button>();
            // 状态，记录页面
            m_StatePage = transform.Find("Content/StatePage").gameObject;
            m_RecordPage = transform.Find("Content/RecordPage").gameObject;
        }

        public override void OnEnter(object intent = null)
        {
            base.OnEnter();
            m_State_Toggle.onValueChanged.AddListener(OnStateToggleClick);
            m_Record_Toggle.onValueChanged.AddListener(OnRecordToggleClick);
            m_Invite_Btn.onClick.AddListener(OnInviteBtnClick);
            EventCenter.AddListener<int>(EventType.REFRESH_ROOMMATE, SetId);
            EventCenter.AddListener<int>(EventType.REFRESH_ROOMMATE, RefreshInvitationBtn);
            EventCenter.AddListener<int>(EventType.REFRESH_ROOMMATE, InitProperty);
            EventCenter.AddListener<int>(EventType.REFRESH_ROOMMATE, RefreshRecordPanel);


        }

        public override void OnExit()
        {
            base.OnExit();
            m_State_Toggle.onValueChanged.RemoveListener(OnStateToggleClick);
            m_Record_Toggle.onValueChanged.RemoveListener(OnRecordToggleClick);
            m_Invite_Btn.onClick.RemoveListener(OnInviteBtnClick);
            EventCenter.RemoveListener<int>(EventType.REFRESH_ROOMMATE, SetId);
            EventCenter.RemoveListener<int>(EventType.REFRESH_ROOMMATE, InitProperty);
            EventCenter.RemoveListener<int>(EventType.REFRESH_ROOMMATE, RefreshRecordPanel);
            EventCenter.RemoveListener<int>(EventType.REFRESH_ROOMMATE, RefreshInvitationBtn);
        }
        private void SetId(int ID)
        {
            m_Id = ID;
        }
        private void InitProperty(int id)
        {
            Roommate roommate = GlobalManager.Instance.roommates[id];
            m_ProfileImg.sprite = sprites[id];
            propertyTxts[0].text = roommate.Name;
            propertyTxts[1].text = "逻辑: " + roommate.propertyStruct.Logic + " <color=" + "#f2c98a" + ">+" + roommate.bonus.LogicBonus + "</color>";
            propertyTxts[2].text = "言语: " + roommate.propertyStruct.Talk + " <color=" + "#f2c98a" + ">+" + roommate.bonus.TalkBonus + "</color>";
            propertyTxts[3].text = "体能: " + roommate.propertyStruct.Athletics + " <color=" + "#f2c98a" + ">+" + roommate.bonus.AthleticsBonus + "</color>";
            propertyTxts[4].text = "灵感: " + roommate.propertyStruct.Creativity + " <color=" + "#f2c98a" + ">+" + roommate.bonus.CreativityBonus + "</color>";
            propertyTxts[5].text = "零花钱: " + roommate.Money;
            propertyTxts[6].text = "亲密度: " + roommate.RelationShip;
            propertyTxts[7].text = "自制力: " + roommate.SelfControl;
        }

        private void OnStateToggleClick(bool isEnable)
        {
            if (isEnable)
            {
                m_StatePage.SetActive(true);
                m_RecordPage.SetActive(false);
            }
        }

        private void OnRecordToggleClick(bool isEnable)
        {
            if (isEnable)
            {
                m_StatePage.SetActive(false);
                m_RecordPage.SetActive(true);
            }
        }

        private void OnInviteBtnClick()
        {
            // TODO:邀请
            for (int i = 1; i < 4; i++)
            {
                if (GlobalManager.Instance.player.stateDic.ContainsKey(StateName.Help + i))
                {
                    GlobalManager.Instance.player.bonus-=(GlobalManager.Instance.player.stateDic[StateName.Help + i].Bonus);
                    GlobalManager.Instance.player.stateDic.Remove(StateName.Help + i);
                }
            }
            GlobalManager.Instance.player.AddState(StateName.Help + (m_Id + 1));
            GlobalManager.Instance.Invitation = (m_Id + 1);
            if (GlobalManager.Instance.Invitation == (m_Id + 1))
                m_Invite_Btn.interactable = false;
            else
                m_Invite_Btn.interactable = true;
        }

        private void RefreshRecordPanel(int id)
        {
            Roommate roommate = GlobalManager.Instance.roommates[id];
            // 刷新roommate的records
            for (int i = 0; i < m_RecordPage.transform.Find("Viewport/Content").childCount; i++)
            {
                Destroy(m_RecordPage.transform.Find("Viewport/Content").GetChild(i).gameObject);
            }

            // 更新record
            for (int i = 0; i < GlobalManager.Instance.player.CurRound - 1; i++)
            {
                GameObject recordPanel = DSD.KernalTool.LoadPrefabs.GetInstance().GetLoadPrefab("RecordRoundPanel");
                recordPanel.transform.Find("RoundTitle").GetComponent<Text>().text =
                    "第" + (i + 1) + "回合";
                Text[] texts = recordPanel.transform.Find("Actions").GetComponentsInChildren<Text>();
                for (var j = 0; j < 5; j++)
                {
                    texts[j].text = roommate.records[i, j];
                }
                recordPanel.transform.parent = m_RecordPage.transform.Find("Viewport/Content");
                recordPanel.transform.localScale = Vector3.one;
            }
        }
        private void RefreshInvitationBtn(int id)
        {
            // 已邀请则没法再邀请
            if (GlobalManager.Instance.Invitation == (id + 1))
                m_Invite_Btn.interactable = false;
            else
                m_Invite_Btn.interactable = true;
        }
    }
}