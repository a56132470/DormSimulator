using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoommatePanel : BasePanel
{
    [SerializeField]
    public Roommate roommate;

    [SerializeField]
    private GameObject propertyPanel;

    [SerializeField]
    /// <summary>
    /// 属性文本
    /// <para>0:Name</para>
    /// <para>1:Logic</para>
    /// <para>2:Talk</para>
    /// <para>3:Athletics</para>
    /// <para>4:Creativity</para>
    /// <para>5:Money</para>
    /// <para>6:RelationShip</para>
    /// <para>7:SelfControl</para>
    /// </summary>
    private Text[] propertyTxts = new Text[8];

    private Toggle State_Toggle;
    private Toggle Record_Toggle;
    private Button Invite_Btn;

    private GameObject StatePage;
    private GameObject RecordPage;
    private List<GameObject> recordGams;

    private void Awake()
    {
    }

    private void Start()
    {
    }

    public override void Init()
    {
        base.Init();
        propertyPanel = transform.Find("PropertyPanel").gameObject;
        propertyTxts = propertyPanel.GetComponentsInChildren<Text>();
        // 状态,记录单选框
        State_Toggle = transform.Find("ToggleBtns/StateToggle").GetComponent<Toggle>();
        Record_Toggle = transform.Find("ToggleBtns/RecordToggle").GetComponent<Toggle>();
        // 邀请按钮
        Invite_Btn = transform.Find("ToggleBtns/InviteBtn").GetComponent<Button>();
        // 状态，记录页面
        StatePage = transform.Find("Content/StatePage").gameObject;
        RecordPage = transform.Find("Content/RecordPage").gameObject;
        recordGams = new List<GameObject>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        State_Toggle.onValueChanged.AddListener(OnStateToggleClick);
        Record_Toggle.onValueChanged.AddListener(OnRecordToggleClick);
        Invite_Btn.onClick.AddListener(OnInviteBtnClick);
    }

    public override void OnExit()
    {
        base.OnExit();
        State_Toggle.onValueChanged.RemoveListener(OnStateToggleClick);
        Record_Toggle.onValueChanged.RemoveListener(OnRecordToggleClick);
        Invite_Btn.onClick.RemoveListener(OnInviteBtnClick);
    }

    public void InitProperty()
    {
        propertyTxts[0].text = roommate.Name;
        propertyTxts[1].text = "逻辑: " + roommate.Logic.ToString() + " <color=" + "#f2c98a" + ">+" + GlobalVariable.instance.player.LogicBonus + "</color>";
        propertyTxts[2].text = "言语: " + roommate.Talk.ToString() + " <color=" + "#f2c98a" + ">+" + GlobalVariable.instance.player.TalkBonus + "</color>";
        propertyTxts[3].text = "体能: " + roommate.Athletics.ToString() + " <color=" + "#f2c98a" + ">+" + GlobalVariable.instance.player.AthleticsBonus + "</color>";
        propertyTxts[4].text = "灵感: " + roommate.Creativity.ToString() + " <color=" + "#f2c98a" + ">+" + GlobalVariable.instance.player.CreativityBonus + "</color>";
        propertyTxts[5].text = "零花钱: " + roommate.Money.ToString();
        propertyTxts[6].text = "亲密度: " + roommate.RelationShip.ToString();
        propertyTxts[7].text = "自制力: " + roommate.SelfControl.ToString();
    }

    private void OnStateToggleClick(bool isEnable)
    {
        if (isEnable)
        {
            StatePage.SetActive(true);
            RecordPage.SetActive(false);
            StatePage.GetComponent<StateManager>().SetRoommateID(roommate.ID);
        }
    }

    private void OnRecordToggleClick(bool isEnable)
    {
        if (isEnable)
        {
            StatePage.SetActive(false);
            RecordPage.SetActive(true);
        }
    }

    private void OnInviteBtnClick()
    {
        // TODO:邀请
        Debug.Log(("已邀请"));
    }

    public void RefreshRecordPanel()
    {
        // 刷新roommate的records
        if (recordGams.Count < (GlobalVariable.instance.player.CurRound - 1))
        {
            // 如果当前的记录里未更新新的record，则更新record
            for (int i = recordGams.Count; i < GlobalVariable.instance.player.CurRound - 1; i++)
            {
                GameObject recordPanel = DSD.KernalTool.LoadPrefabs.GetInstance().GetLoadPrefab("RecordRoundPanel");
                recordPanel.transform.Find("RoundTitle").GetComponent<Text>().text =
                    "第" + (i + 1).ToString() + "回合";
                Text[] texts = recordPanel.transform.Find("Actions").GetComponentsInChildren<Text>();
                for (int j = 0; j < 5; j++)
                {
                    texts[j].text = roommate.records[i, j];
                }
                recordPanel.transform.parent = RecordPage.transform.Find("Viewport/Content");
                recordPanel.transform.localScale = Vector3.one;
                recordGams.Add(recordPanel);
            }
        }
    }
}