using System.Collections.Generic;
using System.Text;
using DSD.KernalTool;
using Panel;
using UnityEngine;
using UnityEngine.Serialization;

namespace Base.StateModule
{
    public class StateManager : MonoBehaviour
    {
        // 标明保存的状态是谁的状态，是舍友123的还是玩家的
        public BasePerson person;

        [FormerlySerializedAs("m_StateGams")] 
        [SerializeField] 
        private List<GameObject> stateGams;

        private GameObject m_StateGrid;

        private void Awake()
        {
            m_StateGrid = transform.Find("Viewport/Content").gameObject;

            stateGams = new List<GameObject>();
        }

        private void OnEnable()
        {
            RefreshStatePage();
        }

        /// <summary>
        /// 根据当前person存储的状态，更新statePage
        /// </summary>
        private void RefreshStatePage()
        {
            if (transform.parent.parent.GetComponent<PlayerPanel>() != null)
            {
                person = GlobalManager.Instance.player;
            }
            for (int i = 0; i < m_StateGrid.transform.childCount; i++)
            {
                Destroy(m_StateGrid.transform.GetChild(i).gameObject);
            }
            List<string> removeKeys = new List<string>();
            if (person.stateDic != null && person.stateDic.Count > 0)
            {
                foreach (KeyValuePair<string, State> k in person.stateDic)
                {
                    if (k.Value.RemainTime > 0&&!k.Value.IsHide)
                    {
                        CreateNewStatePanel(k);
                    }
                    else
                    {
                        removeKeys.Add(k.Key);
                    }
                }
            }
            for (int i = 0; i < removeKeys.Count; i++)
            {
                GlobalManager.Instance.player.bonus+=(GlobalManager.Instance.player.stateDic[removeKeys[i]].Bonus);
                GlobalManager.Instance.player.stateDic.Remove(removeKeys[i]);
            }
        }

        private void CreateNewStatePanel(KeyValuePair<string, State> k)
        {
            GameObject statepanel = LoadPrefabs.GetInstance().GetLoadPrefab("StatePanel");
            StatePanel panel = statepanel.GetComponent<StatePanel>();
            panel.SetState(k.Value, Translate(k.Value));
            statepanel.transform.parent = m_StateGrid.transform;
            statepanel.GetComponent<RectTransform>().localScale = Vector3.one;
            stateGams.Add(statepanel);
        }

        public void SetRoommateID(int id)
        {
            person = GlobalManager.Instance.roommates[id];
        }

        public string Translate(State state)
        {
            StringBuilder effectCaption = new StringBuilder();
            if (state.OtherEffect.Equals(""))
            {
                if (state.Logic != 0)
                {
                    effectCaption.Append("逻辑+" + state.Logic.ToString() + " ");
                }
                if (state.Talk != 0)
                {
                    effectCaption.Append("言语+" + state.Talk.ToString() + " ");
                }
                if (state.Athletics != 0)
                {
                    effectCaption.Append("体能+" + state.Athletics.ToString() + " ");
                }
                if (state.Creativity != 0)
                {
                    effectCaption.Append("灵感+" + state.Creativity.ToString() + " ");
                }
            }
            else
            {
                effectCaption.Append(state.OtherEffect);
            }
            return effectCaption.ToString();
        }
    }
}