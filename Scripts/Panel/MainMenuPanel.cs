using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    private Button ProfileBtn;
    private Button ActionBtn;
    private Button NextBtn;
    private Button SettingBtn;
    private Button MapBtn;
    private Button[] roommateBtns = new Button[3];

    public override void Init()
    {
        base.Init();
        ProfileBtn = transform.Find("Profile").GetComponent<Button>();
        ActionBtn = transform.Find("IconPanel/ActionBtn").GetComponent<Button>();
        NextBtn = transform.Find("IconPanel/NextBtn").GetComponent<Button>();
        SettingBtn = transform.Find("IconPanel/SettingBtn").GetComponent<Button>();
        MapBtn = transform.Find("IconPanel/MapBtn").GetComponent<Button>();

        roommateBtns[0] = transform.Find("Roommates/Roommate1").GetComponent<Button>();
        roommateBtns[1] = transform.Find("Roommates/Roommate2").GetComponent<Button>();
        roommateBtns[2] = transform.Find("Roommates/Roommate3").GetComponent<Button>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        ProfileBtn.onClick.AddListener(OnProfileBtnClick);
        ActionBtn.onClick.AddListener(OnActionBtnClick);
        NextBtn.onClick.AddListener(OnNextBtnClick);
        SettingBtn.onClick.AddListener(OnSettingBtnClick);
        MapBtn.onClick.AddListener(OnMapBtnClick);
        roommateBtns[0].onClick.AddListener(() => { OnRoommateBtnClick(0); });
        roommateBtns[1].onClick.AddListener(() => { OnRoommateBtnClick(1); });
        roommateBtns[2].onClick.AddListener(() => { OnRoommateBtnClick(2); });
    }

    public override void OnExit()
    {
        base.OnExit();
        ProfileBtn.onClick.RemoveAllListeners();
        ActionBtn.onClick.RemoveAllListeners();
        NextBtn.onClick.RemoveAllListeners();
        SettingBtn.onClick.RemoveAllListeners();
        MapBtn.onClick.RemoveAllListeners();
        roommateBtns[0].onClick.RemoveAllListeners();
        roommateBtns[1].onClick.RemoveAllListeners();
        roommateBtns[2].onClick.RemoveAllListeners();
    }

    private void OnProfileBtnClick()
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.Player);
    }

    private void OnNextBtnClick()
    {
        // 这里执行Roommate的下一回合行动
        for (int i = 0; i < 3; i++)
        {
            GlobalVariable.instance.roommates[i].AddRecordAction();
        }
        // 当前回合加1
        GlobalVariable.instance.player.CurRound += 1;
        GlobalVariable.instance.player.Strength = 5;
        foreach (System.Collections.Generic.KeyValuePair<string, State> k in GlobalVariable.instance.player.stateDic)
        {
            k.Value.RemainTime -= 1;
        }
        if (GlobalVariable.instance.player.CurRound >= 24)
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.End);
        }
    }

    private void OnActionBtnClick()
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.Action);
    }

    private void OnRoommateBtnClick(int id)
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.Roommate);
        ((RoommatePanel)UIPanelManager.Instance.GetPanel(UIPanelType.Roommate)).roommate = GlobalVariable.instance.roommates[id];
        ((RoommatePanel)UIPanelManager.Instance.GetPanel(UIPanelType.Roommate)).InitProperty();
        ((RoommatePanel)UIPanelManager.Instance.GetPanel(UIPanelType.Roommate)).RefreshRecordPanel();
    }

    private void OnSettingBtnClick()
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.Setting);
    }

    private void OnMapBtnClick()
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.Map);
    }
}