using UnityEngine.Events;
using UnityEngine.UI;

namespace Panel
{
    public class TipPanel : BasePanel
    {
        private Button m_ToastOkBtn;
        private Button m_ToastCancelBtn;
        private Text m_ToastCaptionText;

        private Button m_TipOkBtn;
        private Text m_TipCaptionText;
        public override void OnEnter(object intent = null)
        {
            base.OnEnter(intent);
            m_ToastCancelBtn.onClick.AddListener(ClosePanel);
            m_TipOkBtn.onClick.AddListener(ClosePanel);
            // Toast 和 Tip都关掉
            m_ToastCaptionText.transform.parent.gameObject.SetActive(false);
            m_TipCaptionText.transform.parent.gameObject.SetActive(false);
        }
        public override void OnExit()
        {
            base.OnExit();
            m_ToastOkBtn.onClick.RemoveAllListeners();
            m_ToastCancelBtn.onClick.RemoveAllListeners();
            m_TipOkBtn.onClick.RemoveAllListeners();
            EventCenter.RemoveListener<string>(EventType.UPDATE_TIP, UpdateTipCaption);
            EventCenter.RemoveListener<string, UnityAction>(EventType.UPDATE_TOAST, UpdateToastCaption);
        }
        public override void Init()
        {
            base.Init();
            m_ToastOkBtn = transform.Find("Toast/OK").GetComponent<Button>();
            m_ToastCancelBtn = transform.Find("Toast/Cancel").GetComponent<Button>();
            m_ToastCaptionText = transform.Find("Toast/Caption").GetComponent<Text>();
            m_TipOkBtn = transform.Find("Tip/OK").GetComponent<Button>();
            m_TipCaptionText = transform.Find("Tip/Caption").GetComponent<Text>();
            EventCenter.AddListener<string, UnityAction>(EventType.UPDATE_TOAST, UpdateToastCaption);
            EventCenter.AddListener<string>(EventType.UPDATE_TIP, UpdateTipCaption);
        }
        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="func"></param>
        private void UpdateToastCaption(string caption, UnityAction func)
        {
            m_ToastCancelBtn.transform.parent.gameObject.SetActive(true);
            m_ToastOkBtn.onClick.RemoveAllListeners();
            m_ToastOkBtn.onClick.AddListener(func);
        
            m_ToastCaptionText.text = caption;
        }
        private void UpdateTipCaption(string caption)
        {
            m_TipCaptionText.transform.parent.gameObject.SetActive(true);
            m_TipCaptionText.text = caption;
        }
        private void ClosePanel()
        {
            UIPanelManager.Instance.PopPanel();
        }
    }
}
