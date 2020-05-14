using UnityEngine.UI;

public class MapPanel : BasePanel
{
    private Button storeBtn;

    public override void Init()
    {
        base.Init();
        storeBtn = transform.Find("StoreBtn").GetComponent<Button>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        storeBtn.onClick.AddListener(OnStoreBtnClick);
    }

    public override void OnExit()
    {
        base.OnExit();
        storeBtn.onClick.RemoveListener(OnStoreBtnClick);
    }

    private void OnStoreBtnClick()
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.Store);
    }
}