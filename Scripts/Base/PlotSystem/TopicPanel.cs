using UnityEngine;
using UnityEngine.UI;

public class TopicPanel : MonoBehaviour
{
    // 主题名称
    private Text TopicNameTxt;

    private Button goBtn;
    private Image unlockImg;

    // 底部panel，实在想不出词儿了，负责存放解锁事件及未解锁事件的图标
    private GameObject bottomPanel;

    private Image[] plotImgs;

    // 事件/情节 列表，存放当前panel里的情节，当情节解锁时未解锁改为解锁
    private Topic topic;

    private int plotIndex = 0;

    private void Awake()
    {
        TopicNameTxt = transform.Find("TopicName").GetComponent<Text>();
        bottomPanel = transform.Find("BottomPanel").gameObject;
        plotImgs = bottomPanel.GetComponentsInChildren<Image>();
        goBtn = transform.Find("GoBtn").GetComponent<Button>();
        unlockImg = transform.Find("Unlock").GetComponent<Image>();
    }

    public void SetTopic(Topic tp)
    {
        topic = tp;
        TopicNameTxt.text = topic.TopicName;
    }

    public void Refresh()
    {
        // 进入主题，即刷新页面，判断是否有情节解锁，有情节解锁则该主题解锁
        if (RefreshPlotState())
        {
            goBtn.gameObject.SetActive(true);
            unlockImg.gameObject.SetActive(false);
            goBtn.onClick.RemoveAllListeners();
            goBtn.onClick.AddListener(() => { OnGoBtnClick(topic.plots[plotIndex].place); });
            gameObject.SetActive(true);
        }
        else
        {
            goBtn.gameObject.SetActive(false);
            unlockImg.gameObject.SetActive(true);
        }
    }

    private bool RefreshPlotState()
    {
        for (int i = 0; i < 6; i++)
        {
            if (topic.plots[i].isFinish)
            {
                plotImgs[i].color = new Color(0.9254903f, 0.6862745f, 0.4627451f);
            }
            if (topic.plots[i].isLock == true && topic.plots[i].CheckUnlockCondition())
            {
                topic.plots[i].isLock = false;

                plotIndex = i;
                return true;
            }
        }
        return false;
    }

    private void OnGoBtnClick(string pt)
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.Place);
        ((PlacePanel)UIPanelManager.Instance.GetPanel(UIPanelType.Place)).SetPlaceType(pt);
        PlotManager.instance.SetTopic(topic);
        PlotManager.instance.ShowBranch();
    }
}