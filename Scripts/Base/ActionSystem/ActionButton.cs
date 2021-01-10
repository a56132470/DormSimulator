using System.Text;
using Panel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Base.ActionSystem
{
    public class ActionButton : MonoBehaviour,  IPointerUpHandler, IPointerDownHandler
    {
        // 外边框，当鼠标移动上去时显示
        private GameObject m_Border;

        private GameObject m_ActionName;
        [FormerlySerializedAs("actionID")] public int actionId;
        public UnityEngine.Events.UnityAction determine;
        private StringBuilder m_CaptionStr;
        private StringBuilder m_ConsumeStr;
        private ActionPanel m_ActionPanel;
        [FormerlySerializedAs("m_ConsumeBonus")] public int consumeBonus;

        private void Start()
        {
            m_Border = transform.Find("Border").gameObject;
            m_ActionName = transform.Find("ActionName").gameObject;
            m_ActionPanel = (ActionPanel)UIPanelManager.Instance.GetPanel(UIPanelType.Action);
            Init();
        }

        private void OnEnable()
        {
            if (m_ActionName != null)
                Init();
        }

        private void Init()
        {
            m_ActionName.GetComponent<Text>().text = XMLManager.Instance.actionList[actionId].Name;
            m_CaptionStr = new StringBuilder();
            m_ConsumeStr = new StringBuilder();
            // TODO: 该改
            if (XMLManager.Instance.actionList[actionId].Property.Logic != 0)
                m_CaptionStr.Append("  逻辑+" + (XMLManager.Instance.actionList[actionId].Property.Logic + GlobalManager.Instance.player.bonus.LogicBonus));
            if (XMLManager.Instance.actionList[actionId].Property.Talk != 0)
                m_CaptionStr.Append("  言谈+" + (XMLManager.Instance.actionList[actionId].Property.Talk + GlobalManager.Instance.player.bonus.TalkBonus));
            if (XMLManager.Instance.actionList[actionId].Property.Athletics != 0)
                m_CaptionStr.Append("  体能+" + (XMLManager.Instance.actionList[actionId].Property.Athletics + GlobalManager.Instance.player.bonus.AthleticsBonus));
            if (XMLManager.Instance.actionList[actionId].Property.Creativity != 0)
                m_CaptionStr.Append("  灵感+" + (XMLManager.Instance.actionList[actionId].Property.Creativity + GlobalManager.Instance.player.bonus.CreativityBonus));
            if (XMLManager.Instance.actionList[actionId].Money != 0)
                m_CaptionStr.Append("  零花钱+" + (XMLManager.Instance.actionList[actionId].Money));

            if (XMLManager.Instance.actionList[actionId].Consume != 0)
            {
                int consume;
                if (GlobalManager.Instance.player.stateDic.ContainsKey(StateName.Help1) && XMLManager.Instance.actionList[actionId].Type == ActionType.Labor)
                {
                    consume = XMLManager.Instance.actionList[actionId].Consume + XMLManager.Instance.actionList[actionId].ConsumeBonus;
                    if (consume >= 1)
                        m_ConsumeStr.Append("消耗行动点数：<color=" + "#f2a599" + ">" + consume + "</color>");
                    else
                        m_ConsumeStr.Append("消耗行动点数：<color=" + "#f2a599" + ">" + (XMLManager.Instance.actionList[actionId].Consume) +
                                            "</color>");
                }
                else
                    m_ConsumeStr.Append("消耗行动点数：<color=" + "#f2a599" + ">" + (XMLManager.Instance.actionList[actionId].Consume) +
                                        "</color>");


            }
        }

        // 鼠标移出关闭边框
        public void OnPointerUp(PointerEventData eventData)
        {
            if (m_Border != null)
                m_Border.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (m_Border != null)
                m_Border.SetActive(true);
            m_ActionPanel.SetAction(XMLManager.Instance.actionList[actionId]);
            EventCenter.Broadcast(EventType.UPDATE_ACTIONCAPTION, m_ConsumeStr.ToString(), m_CaptionStr.ToString());
            determine();
        }
    }
}

