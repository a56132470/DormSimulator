using System.Collections.Generic;
using Base.ActionSystem;
using Base.PlotSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Panel
{
    public class ActionPanel : BasePanel
    {
        private GameObject m_SwitchRoundPanel_Gam;
        private SwitchRound m_SwitchRound_Component;

        // 行动页面
        private GameObject m_ActionPage;

        // 三个按钮框，都遵循网格布局，在Start里直接生成按钮在其中
        // 学习按钮框
        private GameObject m_StudyBtn_Gam;

        // 娱乐按钮框
        private GameObject m_AmusementBtn_Gam;

        // 劳动按钮框
        private GameObject m_LaborBtn_Gam;

        private List<GameObject> m_ActionBtns_ListGams;

        // 说明文本
        private GameObject m_Caption_Gam;

        // 消耗体力文本
        private GameObject m_Consume_Gam;

        // 事件页面
        private GameObject m_PlotPage;

        private TopicPanel[] m_TopicPanels;

        // 两个单选按钮

        // 行动单选框
        private Toggle m_Action_Toggle;

        // 事件单选框
        private Toggle m_Plot_Toggle;

        // 已选择的行动列表框
        private GameObject m_ActionListPanel;

        // 行动列表框按钮们的transform，通过transform获取gameobject
        private static SelectedActionButton[] ActionListBtns;

        // 确定执行的下一回合按钮
        private GameObject m_NextRoundBtn;

        private CharacterAction m_CurrentSelect_Action;

        public override void OnEnter()
        {
            base.OnEnter();
            // 绑定事件
            m_Action_Toggle.onValueChanged.AddListener(OnActionToggleClick);
            m_Plot_Toggle.onValueChanged.AddListener(OnPlotToggleClick);
            m_NextRoundBtn.GetComponent<Button>().onClick.AddListener(OnNextRoundBtnClick);
            EventCenter.AddListener<string, string>(EventType.UPDATE_ACTIONCAPTION, UpdateConsumeAndCaption);
            EventCenter.AddListener(EventType.UPDATE_ACTIONPANEL_EVENT, RefreshPlotPanel);
        }

        public override void OnExit()
        {
            base.OnExit();
            // 解绑事件
            m_Action_Toggle.onValueChanged.RemoveListener(OnActionToggleClick);
            m_Plot_Toggle.onValueChanged.RemoveListener(OnPlotToggleClick);
            m_NextRoundBtn.GetComponent<Button>().onClick.RemoveListener(OnNextRoundBtnClick);
            EventCenter.RemoveListener<string, string>(EventType.UPDATE_ACTIONCAPTION, UpdateConsumeAndCaption);
            EventCenter.RemoveListener(EventType.UPDATE_ACTIONPANEL_EVENT, RefreshPlotPanel);
        }

        public override void OnResume()
        {
            base.OnResume();
            m_Caption_Gam.GetComponent<Text>().text = "";
            EventCenter.Broadcast(EventType.UPDATE_ACTIONPANEL_EVENT);
        }

        /// <summary>
        /// 初始化，绑定物体绑定事件
        /// </summary>
        public override void Init()
        {
            base.Init();

            // 行动和事件单选框
            m_Action_Toggle = transform.Find("Toggles/ActionToggle").GetComponent<Toggle>();
            m_Plot_Toggle = transform.Find("Toggles/PlotToggle").GetComponent<Toggle>();

            // 行动和事件页面
            m_ActionPage = transform.Find("Content/ActionPage").gameObject;
            m_PlotPage = transform.Find("Content/PlotPage").gameObject;

            // 情节面板
            m_TopicPanels = m_PlotPage.GetComponentsInChildren<TopicPanel>();

            // 学习，娱乐，劳动按钮列表
            m_StudyBtn_Gam = m_ActionPage.transform.Find("ActionScrollView/Viewport/Content/Study").gameObject;
            m_AmusementBtn_Gam = m_ActionPage.transform.Find("ActionScrollView/Viewport/Content/Amusement").gameObject;
            m_LaborBtn_Gam = m_ActionPage.transform.Find("ActionScrollView/Viewport/Content/Labor").gameObject;

            // 描述，消耗体力，确定选择按钮
            m_Caption_Gam = m_ActionPage.transform.Find("Caption").gameObject;
            m_Consume_Gam = m_ActionPage.transform.Find("Consume").gameObject;

            // 行动列表框
            m_ActionListPanel = m_ActionPage.transform.Find("ActionList").gameObject;
            ActionListBtns = m_ActionListPanel.transform.Find("Action").GetComponentsInChildren<SelectedActionButton>();
            m_NextRoundBtn = m_ActionListPanel.transform.Find("NextRoundBtn").gameObject;

            // 切换回合
            m_SwitchRoundPanel_Gam = transform.Find("SwitchRoundPanel").gameObject;
            m_SwitchRound_Component = m_SwitchRoundPanel_Gam.GetComponent<SwitchRound>();

            m_ActionBtns_ListGams = new List<GameObject>();

            if (m_ActionPage.activeSelf)
            {
                ToggleFade(m_Plot_Toggle, m_Action_Toggle);
            }
            else if (m_PlotPage.activeSelf)
            {
                ToggleFade(m_Action_Toggle, m_Plot_Toggle);
            }
            for (int i = 0; i < m_TopicPanels.Length; i++)
            {
                // TODO:当前只有1个topic
                m_TopicPanels[i].SetTopic(XMLManager.Instance.topicList[0]);
            }
            InitAction();
        }

        private void OnActionToggleClick(bool isEnable)
        {
            if (isEnable)
            {
                ToggleFade(m_Plot_Toggle, m_Action_Toggle);
                m_ActionPage.SetActive(true);
                m_PlotPage.SetActive(false);
            }
        }

        private void OnPlotToggleClick(bool isEnable)
        {
            if (isEnable)
            {
                ToggleFade(m_Action_Toggle, m_Plot_Toggle);
                m_ActionPage.SetActive(false);
                m_PlotPage.SetActive(true);
                RefreshPlotPanel();
            }
        }

        /// <summary>
        /// Toggle变暗的函数
        /// </summary>
        /// <param name="toggleA">要变暗的Toggle</param>
        /// <param name="toggleB">被点击而恢复的Toggle</param>
        private void ToggleFade(Toggle toggleA, Toggle toggleB)
        {
            ColorBlock cb = toggleB.colors;
            cb.normalColor = toggleB.colors.pressedColor;
            toggleA.colors = cb;
            cb.normalColor = Color.white;
            toggleB.colors = cb;
        }

        public void RefreshPlotPanel()
        {
            for (int i = 0; i < m_TopicPanels.Length; i++)
            {
                m_TopicPanels[i].Refresh();
            }
        }

        private ActionType m_Type;

        /// <summary>
        /// 根据ActionManager加载的xml数据生成按钮并放置于面板中
        /// </summary>
        private void InitAction()
        {
            int index = 0;

            for (int i = 0; i < m_StudyBtn_Gam.transform.childCount; i++)
            {
                Destroy(m_StudyBtn_Gam.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < m_AmusementBtn_Gam.transform.childCount; i++)
            {
                Destroy(m_AmusementBtn_Gam.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < m_LaborBtn_Gam.transform.childCount; i++)
            {
                Destroy(m_LaborBtn_Gam.transform.GetChild(i).gameObject);
            }

            while (index < XMLManager.Instance.actionList.Count)
            {
                if (GlobalManager.Instance.player.CurRound >= XMLManager.Instance.actionList[index].NeedMinRound &&
                    GlobalManager.Instance.player.CurRound <= XMLManager.Instance.actionList[index].NeedMaxRound)
                {
                    m_Type = XMLManager.Instance.actionList[index].Type;
                    GameObject btn;

                    switch (m_Type)
                    {
                        case ActionType.Study:
                            btn = DSD.KernalTool.LoadPrefabs.GetInstance().GetLoadPrefab("StudyBtn");
                            // worldPositionStays设置为false，保留局部方向和比例，防止常见的UI缩放问题
                            btn.transform.SetParent(m_StudyBtn_Gam.transform, false);
                            break;

                        case ActionType.Amusement:
                            btn = DSD.KernalTool.LoadPrefabs.GetInstance().GetLoadPrefab("AmusementBtn");
                            btn.transform.SetParent(m_AmusementBtn_Gam.transform, false);
                            break;

                        case ActionType.Labor:
                            btn = DSD.KernalTool.LoadPrefabs.GetInstance().GetLoadPrefab("LaborBtn");
                            btn.transform.SetParent(m_LaborBtn_Gam.transform, false);
                            break;

                        default:
                            btn = new GameObject();
                            break;
                    }
                    btn.GetComponent<ActionButton>().actionId = index;
                    btn.GetComponent<RectTransform>().localScale = Vector3.one;
                    btn.GetComponent<ActionButton>().determine = OnDetermineClick;
                    m_ActionBtns_ListGams.Add(btn);
                }
                index++;
            }
        }

        public void SetAction(CharacterAction action)
        {
            for (int i = 0; i < XMLManager.Instance.actionList.Count; i++)
            {
                if (action.Name.Equals(XMLManager.Instance.actionList[i].Name))
                {
                    m_CurrentSelect_Action = action;
                }
            }
        }

        /// <summary>
        /// 行动按钮的点击事件,根据体力判断是否可选择
        /// </summary>
        private void OnDetermineClick()
        {
            foreach (SelectedActionButton s in ActionListBtns)
            {
                if (s.action == null)
                {
                    s.action = m_CurrentSelect_Action;
                    if (s.action.Consume <= GlobalManager.Instance.player.Strength ||
                        (GlobalManager.Instance.player.stateDic.ContainsKey(StateName.Help1) &&
                         (s.action.Consume + s.action.ConsumeBonus) <= GlobalManager.Instance.player.Strength))
                    {
                        s.SetActionName(s.action.Name);
                        if (GlobalManager.Instance.player.stateDic.ContainsKey(StateName.Help1) && (s.action.Consume > 1))
                            GlobalManager.Instance.player.Strength -= (s.action.Consume + s.action.ConsumeBonus);
                        else
                            GlobalManager.Instance.player.Strength -= s.action.Consume;
                        break;
                    }
                    else
                    {
                        s.action = null;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 下一回合按钮事件绑定
        /// </summary>
        private void OnNextRoundBtnClick()
        {
            if (IsNullSelectActionArray(ActionListBtns))
            {
                m_SwitchRoundPanel_Gam.SetActive(true);
                m_SwitchRound_Component.SetActionNames(ActionListBtns);
            }
        }

        /// <summary>
        /// 判断当前选择的列表是否有非空值，
        /// 第二版将弃用，因为点击一个选择的列表将自动往上滑一格
        /// </summary>
        /// <param name="Buttons"></param>
        /// <returns></returns>
        private bool IsNullSelectActionArray(SelectedActionButton[] Buttons)
        {
            if (Buttons.Length > 0)
            {
                for (int i = 0; i < Buttons.Length; i++)
                {
                    if (Buttons[i].action != null)
                        return true;
                }
            }
            return false;
        }
        private void UpdateConsumeAndCaption(string consume, string caption)
        {
            m_Consume_Gam.GetComponent<Text>().text = consume;
            m_Caption_Gam.GetComponent<Text>().text = caption;
        }
    }
}