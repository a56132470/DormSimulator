using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : BasePanel
{
    private GameObject switchRoundPanel_Gam;
    private SwitchRound switchRound_Component;

    // 行动页面
    private GameObject ActionPage;

    // 三个按钮框，都遵循网格布局，在Start里直接生成按钮在其中
    // 学习按钮框
    private GameObject StudyBtn_Gam;

    // 娱乐按钮框
    private GameObject AmusementBtn_Gam;

    // 劳动按钮框
    private GameObject LaborBtn_Gam;

    private List<GameObject> actionBtns_ListGams;

    // 说明文本
    private GameObject Caption_Gam;

    // 消耗体力文本
    private GameObject Consume_Gam;

    // 事件页面
    private GameObject PlotPage;

    private TopicPanel[] topicPanels;

    // 两个单选按钮

    // 行动单选框
    private Toggle Action_Toggle;

    // 事件单选框
    private Toggle Plot_Toggle;

    // 已选择的行动列表框
    private GameObject ActionListPanel;

    // 行动列表框按钮们的transform，通过transform获取gameobject
    public static SelectedActionButton[] ActionListBtns;

    // 确定执行的下一回合按钮
    private GameObject NextRoundBtn;

    private CharacterAction CurrentSelect_Action;

    private void Start()
    {
        InitAction();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        // 绑定事件
        Action_Toggle.onValueChanged.AddListener(OnActionToggleClick);
        Plot_Toggle.onValueChanged.AddListener(OnPlotToggleClick);
        NextRoundBtn.GetComponent<Button>().onClick.AddListener(OnNextRoundBtnClick);
    }

    public override void OnExit()
    {
        base.OnExit();
        // 解绑事件
        Action_Toggle.onValueChanged.RemoveListener(OnActionToggleClick);
        Plot_Toggle.onValueChanged.RemoveListener(OnPlotToggleClick);
        NextRoundBtn.GetComponent<Button>().onClick.RemoveListener(OnNextRoundBtnClick);
    }

    public override void OnResume()
    {
        base.OnResume();
        Caption_Gam.GetComponent<Text>().text = "";
        RefreshPlotPanel();
    }

    /// <summary>
    /// 初始化，绑定物体绑定事件
    /// </summary>
    public override void Init()
    {
        base.Init();

        // 行动和事件单选框
        Action_Toggle = transform.Find("Toggles/ActionToggle").GetComponent<Toggle>();
        Plot_Toggle = transform.Find("Toggles/PlotToggle").GetComponent<Toggle>();

        // 行动和事件页面
        ActionPage = transform.Find("Content/ActionPage").gameObject;
        PlotPage = transform.Find("Content/PlotPage").gameObject;

        // 情节面板
        topicPanels = PlotPage.GetComponentsInChildren<TopicPanel>();

        // 学习，娱乐，劳动按钮列表
        StudyBtn_Gam = ActionPage.transform.Find("ActionScrollView/Viewport/Content/Study").gameObject;
        AmusementBtn_Gam = ActionPage.transform.Find("ActionScrollView/Viewport/Content/Amusement").gameObject;
        LaborBtn_Gam = ActionPage.transform.Find("ActionScrollView/Viewport/Content/Labor").gameObject;

        // 描述，消耗体力，确定选择按钮
        Caption_Gam = ActionPage.transform.Find("Caption").gameObject;
        Consume_Gam = ActionPage.transform.Find("Consume").gameObject;

        // 行动列表框
        ActionListPanel = ActionPage.transform.Find("ActionList").gameObject;
        ActionListBtns = ActionListPanel.transform.Find("Action").GetComponentsInChildren<SelectedActionButton>();
        NextRoundBtn = ActionListPanel.transform.Find("NextRoundBtn").gameObject;

        // 切换回合
        switchRoundPanel_Gam = transform.Find("SwitchRoundPanel").gameObject;
        switchRound_Component = switchRoundPanel_Gam.GetComponent<SwitchRound>();

        actionBtns_ListGams = new List<GameObject>();

        if (ActionPage.activeSelf)
        {
            ToggleFade(Plot_Toggle, Action_Toggle);
        }
        else if (PlotPage.activeSelf)
        {
            ToggleFade(Action_Toggle, Plot_Toggle);
        }
        for (int i = 0; i < ActionListBtns.Length; i++)
        {
            ActionListBtns[i].consume = Consume_Gam.GetComponent<Text>();
            ActionListBtns[i].caption = Caption_Gam.GetComponent<Text>();
        }
        for (int i = 0; i < topicPanels.Length; i++)
        {
            // TODO:当前只有1个topic
            topicPanels[i].SetTopic(XMLManager.Instance.topicList[0]);
        }
    }

    private void OnActionToggleClick(bool isEnable)
    {
        if (isEnable)
        {
            ToggleFade(Plot_Toggle, Action_Toggle);
            ActionPage.SetActive(true);
            PlotPage.SetActive(false);
        }
    }

    private void OnPlotToggleClick(bool isEnable)
    {
        if (isEnable)
        {
            ToggleFade(Action_Toggle, Plot_Toggle);
            ActionPage.SetActive(false);
            PlotPage.SetActive(true);
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
        for (int i = 0; i < topicPanels.Length; i++)
        {
            topicPanels[i].Refresh();
        }
    }

    private ActionType type;

    /// <summary>
    /// 根据ActionManager加载的xml数据生成按钮并放置于面板中
    /// </summary>
    private void InitAction()
    {
        int index = 0;

        while (index < XMLManager.Instance.actionList.Count)
        {
            type = XMLManager.Instance.actionList[index].Type;
            GameObject btn;
            switch (type)
            {
                case ActionType.Study:
                    btn = DSD.KernalTool.LoadPrefabs.GetInstance().GetLoadPrefab("StudyBtn");
                    btn.transform.parent = StudyBtn_Gam.transform;
                    break;

                case ActionType.Amusement:
                    btn = DSD.KernalTool.LoadPrefabs.GetInstance().GetLoadPrefab("AmusementBtn");
                    btn.transform.parent = AmusementBtn_Gam.transform;
                    break;

                case ActionType.Labor:
                    btn = DSD.KernalTool.LoadPrefabs.GetInstance().GetLoadPrefab("LaborBtn");
                    btn.transform.parent = LaborBtn_Gam.transform;
                    break;

                default:
                    btn = new GameObject();
                    break;
            }
            btn.GetComponent<ActionButton>().action = XMLManager.Instance.actionList[index];
            btn.GetComponent<RectTransform>().localScale = Vector3.one;
            btn.GetComponent<ActionButton>().consume = Consume_Gam.GetComponent<Text>();
            btn.GetComponent<ActionButton>().caption = Caption_Gam.GetComponent<Text>();
            btn.GetComponent<ActionButton>().determine = OnDetermineClick;
            actionBtns_ListGams.Add(btn);
            index++;
        }
    }

    public void SetAction(CharacterAction action)
    {
        for (int i = 0; i < XMLManager.Instance.actionList.Count; i++)
        {
            if (action.Name.Equals(XMLManager.Instance.actionList[i].Name))
            {
                CurrentSelect_Action = action;
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
                s.action = CurrentSelect_Action;
                if (s.action.Consume <= GlobalVariable.instance.player.Strength)
                {
                    s.SetActionName();
                    GlobalVariable.instance.player.Strength -= s.action.Consume;
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
        if (isNullSelectActionArray(ActionListBtns))
        {
            switchRoundPanel_Gam.SetActive(true);
            switchRound_Component.SetActionNames(ActionListBtns);
        }
    }

    /// <summary>
    /// 判断当前选择的列表是否有非空值，
    /// 第二版将弃用，因为点击一个选择的列表将自动往上滑一格
    /// </summary>
    /// <param name="Buttons"></param>
    /// <returns></returns>
    private bool isNullSelectActionArray(SelectedActionButton[] Buttons)
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
}