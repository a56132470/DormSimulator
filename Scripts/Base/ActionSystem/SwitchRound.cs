using DSD.KernalTool;
using UnityEngine;
using UnityEngine.UI;

namespace Base.ActionSystem
{
    public class SwitchRound : MonoBehaviour
    {
        // 行动名字数组
        public string[] actionNames;

        public Text actionNameText;
        private Animator m_Animator;
        private SelectedActionButton[] m_ActionListBtns;
        private Color[] m_Colors;
        private int m_I;

        private void Start()
        {
            InitColors();
        }

        private void InitColors()
        {
            m_Colors = new Color[3];
            Color yellow = new Color(255 / 255f, 255 / 255f, 147 / 255f);
            Color blue = new Color(146 / 255f, 247 / 255f, 251 / 255f);
            Color pink = new Color(255 / 255f, 147 / 255f, 152 / 255f);
            m_Colors[0] = yellow;
            m_Colors[1] = blue;
            m_Colors[2] = pink;
        }

        public void OnSwitchAction()
        {
            // 当动画播放完毕，若actionNames的索引尚未到底，则继续动画，改变颜色

            if ((++m_I) < actionNames.Length && actionNames[m_I] != null)
            {
                SwitchColor();
                actionNameText.text = "[" + actionNames[m_I] + "]中";
            }
            else
            {
                m_Animator.SetBool("SwitchRound", false);
                StartAction();
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 设置行动名字
        /// </summary>
        /// <param name="names"></param>
        public void SetActionNames(SelectedActionButton[] selectedActions)
        {
            int index = 0;
            // 初始化为null
            actionNames = new string[5];
            if (selectedActions.Length > 0)
            {
                while (index < selectedActions.Length && selectedActions[index] != null)
                {
                    if (selectedActions[index].action != null)
                        actionNames[index] = selectedActions[index].action.Name;
                    else
                        actionNames[index] = null;
                    index++;
                }
            }
            m_ActionListBtns = selectedActions;
            m_I = 0;
            m_Animator = GetComponent<Animator>();
            InitColors();
            SwitchColor();
            m_Animator.SetBool("SwitchRound", true);
            actionNameText.text = "[" + actionNames[m_I] + "]中";
        }

        private void SwitchColor()
        {
            int[] randomSequence = Widget.GetRandomSequence(3, 3);
            transform.Find("Core").gameObject.GetComponent<Image>().color = m_Colors[randomSequence[0]];
            transform.Find("Inner").gameObject.GetComponent<Image>().color = m_Colors[randomSequence[1]];
            transform.Find("Outer").gameObject.GetComponent<Image>().color = m_Colors[randomSequence[2]];
            actionNameText.color = m_Colors[randomSequence[0]];
        }

        private void StartAction()
        {
            // 根据Action增加相应属性
            foreach (SelectedActionButton s in m_ActionListBtns)
            {
                if (s.action != null)
                {
                    Widget.AddProperty(GlobalManager.Instance.player, s.action);
                    // 舍友邀请后一同行动
                    if (GlobalManager.Instance.Invitation != 0)
                    {
                        Widget.AddProperty(GlobalManager.Instance.roommates[GlobalManager.Instance.Invitation - 1], s.action);
                        if(GlobalManager.Instance.player.stateDic.ContainsKey(StateName.Help1))
                        {
                            // 劳动+大于1体力的 增加亲密度值为 (行动的消耗体力值+减免的值) *2
                            if(s.action.Type==ActionType.Labor&&s.action.Consume>1)
                            {
                                GlobalManager.Instance.roommates[GlobalManager.Instance.Invitation - 1].RelationShip += 
                                    ((s.action.Consume+s.action.ConsumeBonus) * 2);
                            }
                            else
                            {
                                // 否则增加 行动的消耗体力值 *2
                                GlobalManager.Instance.roommates[GlobalManager.Instance.Invitation - 1].RelationShip += (s.action.Consume * 2);
                            }
                        }
                    }
                    
                    s.SetActionName("");
                    s.action = null;
                    EventCenter.Broadcast(EventType.UPDATE_ACTIONCAPTION, "", "");
                }
                
            }
        }
    }
}