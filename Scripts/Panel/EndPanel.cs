using UnityEngine;
using UnityEngine.UI;

public class EndPanel : BasePanel
{
    private Text endText;
    private Button endButton;
    private Sprite[] sprites;
    private string[] strs;

    private GameObject CreateSaveTipPanel;
    private Button CST_CancelBtn;
    private Button CST_DetermineBtn;

    private GameObject CreateRolePanel;
    private Button CRP_DetermineBtn;
    private InputField CRP_CreateNameInputField;

    public override void Init()
    {
        base.Init();
        endText = transform.Find("Text").GetComponent<Text>();
        endButton = transform.Find("ImageBtn").GetComponent<Button>();

        CreateSaveTipPanel = transform.Find("CreateSaveTipPanel").gameObject;
        CST_CancelBtn = CreateSaveTipPanel.transform.Find("CST_CancelBtn").GetComponent<Button>();
        CST_DetermineBtn = CreateSaveTipPanel.transform.Find("CST_DetermineBtn").GetComponent<Button>();

        CreateRolePanel = transform.Find("CreateRolePanel").gameObject;
        CRP_DetermineBtn = CreateRolePanel.transform.Find("CRP_DetermineBtn").GetComponent<Button>();
        CRP_CreateNameInputField = CreateRolePanel.GetComponentInChildren<InputField>();

        sprites = new Sprite[4];
        strs = new string[4];
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // CST 是否创建角色
        CST_CancelBtn.onClick.AddListener(OnCSTCancelBtnClick);
        CST_DetermineBtn.onClick.AddListener(OnCSTDetermineBtnClick);
        // CRP:创建角色
        CRP_DetermineBtn.onClick.AddListener(OnCRPDetermineBtnClick);
        endButton.onClick.AddListener(OnEndBtnClick);
    }

    public override void OnExit()
    {
        base.OnExit();

        CST_CancelBtn.onClick.RemoveListener(OnCSTCancelBtnClick);
        CST_DetermineBtn.onClick.RemoveListener(OnCSTDetermineBtnClick);

        CRP_DetermineBtn.onClick.RemoveListener(OnCRPDetermineBtnClick);
        endButton.onClick.RemoveListener(OnEndBtnClick);
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
        //// 切换图片
        //if(sprites!=null&&sprites[3]!=null)
        //{
        //}
        //else
        {
            CreateSaveTipPanel.SetActive(true);
        }
    }

    private void OnCSTCancelBtnClick()
    {
        // Disappear
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
            GameInit.Instance.Init(PlayerName, GlobalVariable.instance.player.CurWeek + 1, GlobalVariable.instance.SaveID);

            GameSaveManager.instance.SaveGame();
            CreateRolePanel.SetActive(false);
            CRP_CreateNameInputField.text = "";
            // 进行一次存档操作
            UIPanelManager.Instance.PopPanel();
        }
    }
}