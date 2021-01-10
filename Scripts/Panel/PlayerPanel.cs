using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Panel
{
    public class PlayerPanel : BasePanel
    {
        private GameObject m_PropertyPanel;

        [FormerlySerializedAs("m_PropertyTxts")] [SerializeField]
        /// <summary>
        /// 属性文本
        /// <para>0:Name</para>
        /// <para>1:Logic</para>
        /// <para>2:Talk</para>
        /// <para>3:Athletics</para>
        /// <para>4:Creativity</para>
        /// <para>5:Money</para>
        /// </summary>
        private Text[] propertyTxts = new Text[6];

        private Toggle m_State_Toggle;
        private Toggle m_Record_Toggle;
        private Toggle m_Goods_Toggle;

        private GameObject m_StatePage;
        private GameObject m_RecordPage;
        private GameObject m_GoodsPage;

        private void OnStateToggleClick(bool isEnable)
        {
            if (isEnable)
            {
                m_StatePage.SetActive(true);
                m_RecordPage.SetActive(false);
                m_GoodsPage.SetActive(false);
            }
        }

        private void OnRecordToggleClick(bool isEnable)
        {
            if (isEnable)
            {
                m_StatePage.SetActive(false);
                m_RecordPage.SetActive(true);
                m_GoodsPage.SetActive(false);
            }
        }

        private void OnGoodsToggleClick(bool isEnable)
        {
            if (isEnable)
            {
                m_StatePage.SetActive(false);
                m_RecordPage.SetActive(false);
                m_GoodsPage.SetActive(true);
            }
        }

        private void InitProperty()
        {
            propertyTxts[0].text = GlobalManager.Instance.player.Name;
            propertyTxts[1].text = "逻辑: " + GlobalManager.Instance.player.propertyStruct.Logic + " <color=" + "#f2c98a" + ">+" +
                                   GlobalManager.Instance.player.bonus.LogicBonus + "</color>";
            propertyTxts[2].text = "言语: " + GlobalManager.Instance.player.propertyStruct.Talk + " <color=" + "#f2c98a" + ">+" + 
                                   GlobalManager.Instance.player.bonus.TalkBonus +  "</color>";
            propertyTxts[3].text = "体能: " + GlobalManager.Instance.player.propertyStruct.Athletics + " <color=" + "#f2c98a" + ">+" +
                                   GlobalManager.Instance.player.bonus.AthleticsBonus + "</color>";
            propertyTxts[4].text = "灵感: " + GlobalManager.Instance.player.propertyStruct.Creativity + " <color=" + "#f2c98a" + ">+" +
                                   GlobalManager.Instance.player.bonus.CreativityBonus + "</color>";
            propertyTxts[5].text = "零花钱: " + GlobalManager.Instance.player.Money;
        }

        public override void Init()
        {
            base.Init();

            // 状态,记录,物品单选框
            m_State_Toggle = transform.Find("ToggleBtns/StateToggle").GetComponent<Toggle>();
            m_Record_Toggle = transform.Find("ToggleBtns/RecordToggle").GetComponent<Toggle>();
            m_Goods_Toggle = transform.Find("ToggleBtns/GoodsToggle").GetComponent<Toggle>();
            // 状态，记录，物品页面
            m_StatePage = transform.Find("Content/StatePage").gameObject;
            m_RecordPage = transform.Find("Content/RecordPage").gameObject;
            m_GoodsPage = transform.Find("Content/GoodsPage").gameObject;

            m_PropertyPanel = transform.Find("PropertyPanel").gameObject;
            propertyTxts = m_PropertyPanel.GetComponentsInChildren<Text>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            InitProperty();
            RefreshRecordPanel();
            m_State_Toggle.onValueChanged.AddListener(OnStateToggleClick);
            m_Record_Toggle.onValueChanged.AddListener(OnRecordToggleClick);
            m_Goods_Toggle.onValueChanged.AddListener(OnGoodsToggleClick);
        
        }

        public override void OnExit()
        {
            base.OnExit();
            m_State_Toggle.onValueChanged.RemoveListener(OnStateToggleClick);
            m_Record_Toggle.onValueChanged.RemoveListener(OnRecordToggleClick);
            m_Goods_Toggle.onValueChanged.RemoveListener(OnGoodsToggleClick);
        }

        public void RefreshRecordPanel()
        {

            // 更新record

            for (int i = 0; i < m_RecordPage.transform.Find("Viewport/Content").childCount; i++)
            {
                Destroy(m_RecordPage.transform.Find("Viewport/Content").GetChild(i).gameObject);
            }

            for (int i = 0; i < GlobalManager.Instance.player.CurRound - 1; i++)
            {
                GameObject recordPanel = DSD.KernalTool.LoadPrefabs.GetInstance().GetLoadPrefab("RecordRoundPanel");
                recordPanel.transform.Find("RoundTitle").GetComponent<Text>().text =
                    "第" + (i + 1).ToString() + "回合";
                Text[] texts = recordPanel.transform.Find("Actions").GetComponentsInChildren<Text>();
                for (int j = 0; j < 5; j++)
                {
                    if (GlobalManager.Instance.player.records[i, j] != null)
                        texts[j].text = GlobalManager.Instance.player.records[i, j];
                    else
                        texts[j].text = "发呆了一天，啥也没干";
                }
                recordPanel.transform.parent = m_RecordPage.transform.Find("Viewport/Content");
                recordPanel.transform.localScale = Vector3.one;
            }
        }
    }
}