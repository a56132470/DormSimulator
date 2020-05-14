using DSD.KernalTool;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // 标明保存的状态是谁的状态，是舍友123的还是玩家的
    public BasePerson person;

    private List<GameObject> stateGams;

    // 如果person为舍友则需要设定id
    private int RoommateID = 0;

    private GameObject stateGrid;

    private void Awake()
    {
        stateGrid = transform.Find("Viewport/Content").gameObject;

        stateGams = new List<GameObject>();
    }

    private void OnEnable()
    {
        RefreshStatePage();
    }

    /// <summary>
    /// 根据当前person存储的状态，更新statepage
    /// </summary>
    private void RefreshStatePage()
    {
        if (transform.parent.parent.GetComponent<PlayerPanel>() != null)
        {
            person = GlobalVariable.instance.player;
        }
        for (int i = 0; i < stateGrid.transform.childCount; i++)
        {
            Destroy(stateGrid.transform.GetChild(i).gameObject);
        }
        List<string> removeKeys = new List<string>();
        if (person.stateDic != null && person.stateDic.Count > 0)
        {
            foreach (KeyValuePair<string, State> k in person.stateDic)
            {
                if (k.Value.RemainTime > 0)
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
            GlobalVariable.instance.player.SubBonus(GlobalVariable.instance.player.stateDic[removeKeys[i]]);
            GlobalVariable.instance.player.stateDic.Remove(removeKeys[i]);
        }
    }

    private void CreateNewStatePanel(KeyValuePair<string, State> k)
    {
        GameObject statepanel = LoadPrefabs.GetInstance().GetLoadPrefab("StatePanel");
        StatePanel panel = statepanel.GetComponent<StatePanel>();
        panel.setState(k.Value, Translate(k.Value));
        statepanel.transform.parent = stateGrid.transform;
        statepanel.GetComponent<RectTransform>().localScale = Vector3.one;
        stateGams.Add(statepanel);
    }

    public void SetRoommateID(int id)
    {
        RoommateID = id;
        person = GlobalVariable.instance.roommates[id];
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