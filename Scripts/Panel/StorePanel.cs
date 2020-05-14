using UnityEngine;
using UnityEngine.UI;

public class StorePanel : BasePanel
{
    private Button ReturnBtn;
    private Button SettingBtn;

    public override void Init()
    {
        base.Init();
        canvasGroup = transform.GetComponent<CanvasGroup>();
        SettingBtn = transform.Find("SettingBtn").GetComponent<Button>();
        ReturnBtn = transform.Find("ReturnBtn").GetComponent<Button>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        ReturnBtn.onClick.AddListener(OnReturnButtonClick);
        SettingBtn.onClick.AddListener(OnSettingButtonClick);
    }

    public override void OnExit()
    {
        base.OnExit();
        ReturnBtn.onClick.RemoveListener(OnReturnButtonClick);
        SettingBtn.onClick.RemoveListener(OnSettingButtonClick);
    }

    private void OnReturnButtonClick()
    {
        UIPanelManager.Instance.PopPanel();
    }

    private void OnSettingButtonClick()
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.Setting);
    }
}