using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Base.ActionSystem
{
    /// <summary>
    /// 已选择行动的按钮：当在ActionPanel选择行动，则行动出现在此面板中
    /// 功能：选择按钮则初始化按钮，并恢复一点体力
    /// </summary>
    public class SelectedActionButton : MonoBehaviour, IPointerClickHandler
    {
        private GameObject m_ActionName;
        public CharacterAction action;


        private void Start()
        {
            m_ActionName = transform.Find("ActionName").gameObject;
            m_ActionName.GetComponent<Text>().text = "";
            action = null;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            // 恢复体力
            if (action != null)
            {
                if (GlobalManager.Instance.player.stateDic.ContainsKey(StateName.Help1) && action.Consume > 1)
                {
                    GlobalManager.Instance.player.Strength += (action.Consume + action.ConsumeBonus);
                }
                else
                    GlobalManager.Instance.player.Strength += action.Consume;
            }

            // 设置左上角头像的体力条
            // 将ActionName设为空
            SetActionName("");
            // 将Action设为空
            action = null;
            EventCenter.Broadcast(EventType.UPDATE_ACTIONCAPTION, "", "");
        }

        public void SetActionName(string Name)
        {
            m_ActionName.GetComponent<Text>().text = Name;
        }
    }
}