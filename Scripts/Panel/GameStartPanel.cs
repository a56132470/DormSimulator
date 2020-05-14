using UnityEngine;
using UnityEngine.UI;

public class GameStartPanel : BasePanel
{
    private Button StartButton;
    private Button SettingButton;

    private void Update()
    {
        // 点击esc退出
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public override void Init()
    {
        base.Init();
        StartButton = transform.Find("StartBtn").GetComponent<Button>();
        SettingButton = transform.Find("SettingBtn").GetComponent<Button>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        StartButton.onClick.AddListener(OnStartButtonClick);
        SettingButton.onClick.AddListener(OnSettingButtonClick);
    }

    public override void OnExit()
    {
        base.OnExit();
        StartButton.onClick.RemoveListener(OnStartButtonClick);
        SettingButton.onClick.RemoveListener(OnSettingButtonClick);
    }

    /// <summary>
    /// 开始游戏按钮
    /// </summary>
    private void OnStartButtonClick()
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.Save);
    }

    /// <summary>
    /// 设置按钮
    /// </summary>
    private void OnSettingButtonClick()
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.Setting);
    }
}