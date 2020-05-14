using UnityEngine;
using UnityEngine.UI;

public class SavePanel : BasePanel
{
    private Button ReturnBtn;
    private Button SettingBtn;

    private Button[] saveBtns;
    private Player[] players;
    private Roommate[][] Roommates;

    private GameObject CreateSaveTipPanel;
    private Button CST_CancelBtn;
    private Button CST_DetermineBtn;

    private GameObject CreateRolePanel;
    private Button CRP_DetermineBtn;
    private InputField CRP_CreateNameInputField;

    // 当前点击的按钮的ID
    private int Count;

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);

        ReturnBtn.onClick.AddListener(OnReturnButtonClick);
        SettingBtn.onClick.AddListener(OnSettingButtonClick);
        saveBtns[0].onClick.AddListener(() => { OnSaveBtnClick(0); });
        saveBtns[1].onClick.AddListener(() => { OnSaveBtnClick(1); });
        saveBtns[2].onClick.AddListener(() => { OnSaveBtnClick(2); });
        // CST:是否创建角色
        CST_CancelBtn.onClick.AddListener(OnCSTCancelBtnClick);
        CST_DetermineBtn.onClick.AddListener(OnCSTDetermineBtnClick);
        // CRP:创建角色
        CRP_DetermineBtn.onClick.AddListener(OnCRPDetermineBtnClick);
    }

    public override void OnExit()
    {
        base.OnExit();

        ReturnBtn.onClick.RemoveListener(OnReturnButtonClick);
        SettingBtn.onClick.RemoveListener(OnSettingButtonClick);
        saveBtns[0].onClick.RemoveListener(() => { OnSaveBtnClick(0); });
        saveBtns[1].onClick.RemoveListener(() => { OnSaveBtnClick(1); });
        saveBtns[2].onClick.RemoveListener(() => { OnSaveBtnClick(2); });

        // CST:是否创建角色
        CST_CancelBtn.onClick.RemoveListener(OnCSTCancelBtnClick);
        CST_DetermineBtn.onClick.RemoveListener(OnCSTDetermineBtnClick);
        // CRP:创建角色
        CRP_DetermineBtn.onClick.RemoveListener(OnCRPDetermineBtnClick);
    }

    public override void OnPause()
    {
        base.OnPause();
        gameObject.SetActive(false);
    }

    public override void OnResume()
    {
        base.OnResume();
        Init();
        gameObject.SetActive(true);
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
        canvasGroup = transform.GetComponent<CanvasGroup>();
        SettingBtn = transform.Find("SettingBtn").GetComponent<Button>();
        ReturnBtn = transform.Find("ReturnBtn").GetComponent<Button>();
        saveBtns = transform.Find("Saves").GetComponentsInChildren<Button>();
        CreateSaveTipPanel = transform.Find("CreateSaveTipPanel").gameObject;
        CST_CancelBtn = CreateSaveTipPanel.transform.Find("CST_CancelBtn").GetComponent<Button>();
        CST_DetermineBtn = CreateSaveTipPanel.transform.Find("CST_DetermineBtn").GetComponent<Button>();

        CreateRolePanel = transform.Find("CreateRolePanel").gameObject;
        CRP_DetermineBtn = CreateRolePanel.transform.Find("CRP_DetermineBtn").GetComponent<Button>();
        CRP_CreateNameInputField = CreateRolePanel.GetComponentInChildren<InputField>();


        Roommates = new Roommate[3][];
        Roommates[0] = new Roommate[3];
        Roommates[1] = new Roommate[3];
        Roommates[2] = new Roommate[3];
        players = new Player[3];

        for (int i = 0; i < 3; i++)
        {
            players[i] = new Player();
            for (int j = 0; j < 3; j++)
            {
                Roommates[i][j] = new Roommate();
            }
            GameSaveManager.instance.LoadGame(ref players[i], ref Roommates[i], i);
            if (players[i] != null && !players[i].Name.Equals(""))
            {
                // 读取成功
                saveBtns[i].transform.Find("Yes").gameObject.SetActive(true);
                saveBtns[i].transform.Find("Yes/Week").GetComponent<Text>().text = "第" + players[i].CurWeek.ToString() + "周目";
                saveBtns[i].transform.Find("Yes/Round").GetComponent<Text>().text = "第" + players[i].CurRound.ToString() + "回合";
                saveBtns[i].transform.Find("Yes/Name").GetComponent<Text>().text = players[i].Name;

                saveBtns[i].transform.Find("No").gameObject.SetActive(false);
            }
            else
            {
                saveBtns[i].transform.Find("No").gameObject.SetActive(true);
                saveBtns[i].transform.Find("Yes").gameObject.SetActive(false);
            }
        }
    
    }

    private void OnSaveBtnClick(int count)
    {
        Count = count;
        if (saveBtns[Count].transform.Find("Yes").gameObject.activeSelf)
        {
            GlobalVariable.instance.player = players[Count];
            GlobalVariable.instance.roommates = Roommates[Count];
            GlobalVariable.instance.SaveID = Count;
            GameSaveManager.instance.LoadInventoryOnStart();
            if(GlobalVariable.instance.player.CurRound>=24)
            {
                UIPanelManager.Instance.PushPanel(UIPanelType.End);
            }
            else
            {
                UIPanelManager.Instance.PushPanel(UIPanelType.MainMenu);
            }
            
        }
        else if (saveBtns[Count].transform.Find("No").gameObject.activeSelf)
        {
            // 创建角色
            CreateSaveTipPanelShow();
        }
    }

    private void CreateSaveTipPanelShow()
    {
        // Show
        if (!CreateSaveTipPanel.activeSelf)
        {
            saveBtns[0].transform.parent.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            CreateSaveTipPanel.SetActive(true);
        }
    }

    private void OnCSTCancelBtnClick()
    {
        // Disappear
        saveBtns[0].transform.parent.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

        CreateSaveTipPanel.SetActive(false);
    }

    private void OnCSTDetermineBtnClick()
    {
        CreateSaveTipPanel.SetActive(false);
        CreateRolePanel.SetActive(true);
    }

    private void OnCRPDetermineBtnClick()
    {
        string PlayerName = CRP_CreateNameInputField.text;
        if (PlayerName.Equals(""))
        {
            // TODO: 提示模块
            Debug.Log("请您输入名字");
        }
        else
        {
            GameInit.Instance.Init(PlayerName, 1, Count);

            GameSaveManager.instance.SaveGame();
            CreateRolePanel.SetActive(false);
            CRP_CreateNameInputField.text = "";
            saveBtns[0].transform.parent.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            // 进行一次存档操作
            UIPanelManager.Instance.PushPanel(UIPanelType.MainMenu);
            PlotManager.instance.ShowMain();
        }
    }
}