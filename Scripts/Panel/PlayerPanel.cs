using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : BasePanel
{
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
    /// </summary>
    private Text[] propertyTxts = new Text[6];

    private Toggle State_Toggle;
    private Toggle Record_Toggle;
    private Toggle Goods_Toggle;

    private GameObject StatePage;
    private GameObject RecordPage;
    private GameObject GoodsPage;
    private System.Collections.Generic.List<GameObject> recordGams;

    private void OnStateToggleClick(bool isEnable)
    {
        if (isEnable)
        {
            StatePage.SetActive(true);
            RecordPage.SetActive(false);
            GoodsPage.SetActive(false);
        }
    }

    private void OnRecordToggleClick(bool isEnable)
    {
        if (isEnable)
        {
            StatePage.SetActive(false);
            RecordPage.SetActive(true);
            GoodsPage.SetActive(false);
        }
    }

    private void OnGoodsToggleClick(bool isEnable)
    {
        if (isEnable)
        {
            StatePage.SetActive(false);
            RecordPage.SetActive(false);
            GoodsPage.SetActive(true);
        }
    }

    private void InitProperty()
    {
        propertyTxts[0].text = GlobalVariable.instance.player.Name;
        propertyTxts[1].text = "逻辑: " + GlobalVariable.instance.player.Logic.ToString() + " <color=" + "#f2c98a" + ">+" + GlobalVariable.instance.player.LogicBonus + "</color>";
        propertyTxts[2].text = "言语: " + GlobalVariable.instance.player.Talk.ToString() + " <color=" + "#f2c98a" + ">+" + GlobalVariable.instance.player.TalkBonus + "</color>";
        propertyTxts[3].text = "体能: " + GlobalVariable.instance.player.Athletics.ToString() + " <color=" + "#f2c98a" + ">+" + GlobalVariable.instance.player.AthleticsBonus + "</color>";
        propertyTxts[4].text = "灵感: " + GlobalVariable.instance.player.Creativity.ToString() + " <color=" + "#f2c98a" + ">+" + GlobalVariable.instance.player.CreativityBonus + "</color>";
        propertyTxts[5].text = "零花钱: " + GlobalVariable.instance.player.Money.ToString();
    }

    public override void Init()
    {
        base.Init();

        // 状态,记录,物品单选框
        State_Toggle = transform.Find("ToggleBtns/StateToggle").GetComponent<Toggle>();
        Record_Toggle = transform.Find("ToggleBtns/RecordToggle").GetComponent<Toggle>();
        Goods_Toggle = transform.Find("ToggleBtns/GoodsToggle").GetComponent<Toggle>();
        // 状态，记录，物品页面
        StatePage = transform.Find("Content/StatePage").gameObject;
        RecordPage = transform.Find("Content/RecordPage").gameObject;
        GoodsPage = transform.Find("Content/GoodsPage").gameObject;

        recordGams = new System.Collections.Generic.List<GameObject>();

        propertyPanel = transform.Find("PropertyPanel").gameObject;
        propertyTxts = propertyPanel.GetComponentsInChildren<Text>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        InitProperty();
        State_Toggle.onValueChanged.AddListener(OnStateToggleClick);
        Record_Toggle.onValueChanged.AddListener(OnRecordToggleClick);
        Goods_Toggle.onValueChanged.AddListener(OnGoodsToggleClick);
        // RefreshRecordPanel();
    }

    public override void OnExit()
    {
        base.OnExit();
        State_Toggle.onValueChanged.RemoveListener(OnStateToggleClick);
        Record_Toggle.onValueChanged.RemoveListener(OnRecordToggleClick);
        Goods_Toggle.onValueChanged.RemoveListener(OnGoodsToggleClick);
    }

    public void RefreshRecordPanel()
    {
        // player 这里 如果只做了一件事情，那么判断体力是否清空，
        // 如若清空，则在其余空的地方添加"你没精力做其他事情了"
        // 如若未清空，则在其余空的
        // 如果当前的记录里未更新新的record，则更新record
        for (int i = 0; i < GlobalVariable.instance.player.CurRound - 1; i++)
        {
            GameObject recordPanel = DSD.KernalTool.LoadPrefabs.GetInstance().GetLoadPrefab("RecordRoundPanel");
            recordPanel.transform.Find("RoundTitle").GetComponent<Text>().text =
                "第" + (i + 1).ToString() + "回合";
            Text[] texts = recordPanel.transform.Find("Actions").GetComponentsInChildren<Text>();
            for (int j = 0; j < 5; j++)
            {
                if (GlobalVariable.instance.player.records[i, j] != null)
                    texts[j].text = GlobalVariable.instance.player.records[i, j];
                else
                    texts[j].text = "躺了一天，啥也没干";
            }
            recordPanel.transform.parent = RecordPage.transform.Find("Viewport/Content");
            recordPanel.transform.localScale = Vector3.one;
            recordGams.Add(recordPanel);
            Debug.Log("已添加记录");
        }
    }
}