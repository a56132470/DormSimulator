using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler
{
    // 外边框，当鼠标移动上去时显示
    private GameObject Border;

    private GameObject ActionName;
    public CharacterAction action;
    public Text caption;
    public Text consume;
    public UnityEngine.Events.UnityAction determine;
    private StringBuilder CaptionStr;
    private StringBuilder ConsumeStr;
    private ActionPanel actionPanel;

    private void Start()
    {
        Border = transform.Find("Border").gameObject;
        ActionName = transform.Find("ActionName").gameObject;
        actionPanel = (ActionPanel)UIPanelManager.Instance.GetPanel(UIPanelType.Action);
        Init();
    }

    private void OnEnable()
    {
        if (ActionName != null)
            Init();
    }

    private void Init()
    {
        ActionName.GetComponent<Text>().text = action.Name;
        CaptionStr = new StringBuilder();
        ConsumeStr = new StringBuilder();
        if (action.Logic != 0)
            CaptionStr.Append("  逻辑+" + (action.Logic + GlobalVariable.instance.player.LogicBonus));
        if (action.Talk != 0)
            CaptionStr.Append("  言谈+" + (action.Talk + GlobalVariable.instance.player.TalkBonus));
        if (action.Athletics != 0)
            CaptionStr.Append("  体能+" + (action.Athletics + GlobalVariable.instance.player.AthleticsBonus));
        if (action.Creativity != 0)
            CaptionStr.Append("  灵感+" + (action.Creativity + GlobalVariable.instance.player.CreativityBonus));
        if (action.Money != 0)
            CaptionStr.Append("  零花钱+" + (action.Money));

        if (action.Consume != 0)
            ConsumeStr.Append("消耗行动点数：<color=" + "#f2a599" + ">" + action.Consume + "</color>");
    }

    // 鼠标移入开启边框
    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    // 鼠标移出关闭边框
    public void OnPointerUp(PointerEventData eventData)
    {
        if (Border != null)
            Border.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Border != null)
            Border.SetActive(true);
        caption.text = CaptionStr.ToString();
        actionPanel.SetAction(action);
        consume.text = ConsumeStr.ToString();
        determine();
    }
}