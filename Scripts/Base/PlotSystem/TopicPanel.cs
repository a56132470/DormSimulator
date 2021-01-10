using UnityEngine;
using UnityEngine.UI;

namespace Base.PlotSystem
{
    public class TopicPanel : MonoBehaviour
    {
        // 主题名称
        private Text m_TopicNameTxt;

        private Button m_GoBtn;
        private Image m_UnlockImg;

        // 底部panel，实在想不出词儿了，负责存放解锁事件及未解锁事件的图标
        private GameObject m_BottomPanel;

        private Image[] m_PlotImgs;

        // 事件/情节 列表，存放当前panel里的情节，当情节解锁时未解锁改为解锁
        private Topic m_Topic;

        private int m_PlotIndex = 0;

        private void Awake()
        {
            m_TopicNameTxt = transform.Find("TopicName").GetComponent<Text>();
            m_BottomPanel = transform.Find("BottomPanel").gameObject;
            m_PlotImgs = m_BottomPanel.GetComponentsInChildren<Image>();
            m_GoBtn = transform.Find("GoBtn").GetComponent<Button>();
            m_UnlockImg = transform.Find("Unlock").GetComponent<Image>();
        }

        public void SetTopic(Topic tp)
        {
            m_Topic = tp;
            if (m_TopicNameTxt != null)
                m_TopicNameTxt.text = m_Topic.TopicName;
            else
                transform.Find("TopicName").GetComponent<Text>().text = m_Topic.TopicName;
        }

        public void Refresh()
        {
            // 进入主题，即刷新页面，判断是否有情节解锁，有情节解锁则该主题解锁
            if (RefreshPlotState())
            {
                m_GoBtn.gameObject.SetActive(true);
                m_UnlockImg.gameObject.SetActive(false);
                m_GoBtn.onClick.RemoveAllListeners();
                m_GoBtn.onClick.AddListener(() => { OnGoBtnClick(m_Topic.Plots[m_PlotIndex].Place); });
                gameObject.SetActive(true);
            }
            else
            {
                m_GoBtn.gameObject.SetActive(false);
                m_UnlockImg.gameObject.SetActive(true);
            }
        }

        private bool RefreshPlotState()
        {
            for (var i = 0; i < 6; i++)
            {
                if (m_Topic.Plots[i].IsFinish)
                {
                    m_PlotImgs[i].color = new Color(0.9254903f, 0.6862745f, 0.4627451f);
                }
                if (m_Topic.Plots[i].IsLock && m_Topic.Plots[i].CheckUnlockCondition())
                {
                    m_Topic.Plots[i].IsLock = false;

                    m_PlotIndex = i;
                    return true;
                }
            }
            return false;
        }

        private void OnGoBtnClick(string pt)
        {
            EventCenter.Broadcast(EventType.CHANGE_SKIN, pt);
            PlotManager.Instance.SetTopic(m_Topic);
            PlotManager.Instance.ShowBranch((m_PlotIndex + 1));
        }
    }
}
