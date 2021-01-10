using UnityEngine.UI;

namespace Panel
{
    public class MapPanel : BasePanel
    {
        private Button m_StoreBtn;
        private Button m_ClassBtn;
        private Button m_DormBtn;
        private Button m_PlaygroundBtn;

        public override void Init()
        {
            base.Init();
            m_StoreBtn = transform.Find("StoreBtn").GetComponent<Button>();
            m_ClassBtn = transform.Find("ClassBtn").GetComponent<Button>();
            m_DormBtn = transform.Find("DormBtn").GetComponent<Button>();
            m_PlaygroundBtn = transform.Find("PlaygroundBtn").GetComponent<Button>();
        }

        public override void OnEnter(object intent = null)
        {
            base.OnEnter();
            m_StoreBtn.onClick.AddListener(OnStoreBtnClick);
            m_ClassBtn.onClick.AddListener(OnClassBtnClick);
            m_DormBtn.onClick.AddListener(OnDormBtnClick);
            m_PlaygroundBtn.onClick.AddListener(OnPlaygroundBtnClick);
        }

        public override void OnExit()
        {
            base.OnExit();
            m_StoreBtn.onClick.RemoveAllListeners();
            m_ClassBtn.onClick.RemoveAllListeners();
            m_DormBtn.onClick.RemoveAllListeners();
            m_PlaygroundBtn.onClick.RemoveAllListeners();
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
        private void OnStoreBtnClick()
        {
            UIPanelManager.Instance.PushPanel(UIPanelType.Store);
        }
        private void OnClassBtnClick()
        {
            EventCenter.Broadcast<string>(EventType.CHANGE_SKIN, PlaceType.Classroom);
            UIPanelManager.Instance.PopPanel();
        }
        private void OnDormBtnClick()
        {
            EventCenter.Broadcast<string>(EventType.CHANGE_SKIN, PlaceType.Dorm);
            UIPanelManager.Instance.PopPanel();
        }
        private void OnPlaygroundBtnClick()
        {
            EventCenter.Broadcast<string>(EventType.CHANGE_SKIN, PlaceType.Playground);
            UIPanelManager.Instance.PopPanel();
        }
    }
}