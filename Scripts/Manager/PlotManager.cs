using Fungus;
using System.Collections.Generic;
using Base.ActionSystem;
using Base.PlotSystem;
using Base.Struct;
using JetBrains.Annotations;
using UnityEngine;
public class PlotManager : MonoBehaviour
{
    public static PlotManager Instance;
    public GameObject mainFlowchart;
    public GameObject branchFlowchart;
    private Topic m_CurTopic;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        EventCenter.AddListener(EventType.NEXT_ROUND, UpdateVariable);
    }

    public void SetTopic(Topic tc)
    {
        m_CurTopic = tc;
    }

    public void ShowMain()
    {
        mainFlowchart.SetActive(true);
        GameObject.Find("Fungus/Characters/P").GetComponent<Character>().NameText = GlobalManager.Instance.player.Name;
        mainFlowchart.GetComponent<Flowchart>().ExecuteBlock("StartStory");
    }

    public void ShowBranch(int branch)
    {
        branchFlowchart.SetActive(true);
        Debug.Log("Branch:" + branch);
        branchFlowchart.GetComponent<Flowchart>().SetIntegerVariable("branch", branch);
        branchFlowchart.GetComponent<Flowchart>().ExecuteIfHasBlock(m_CurTopic.TopicName);
    }
    /// <summary>
    /// 调用指定名称的block
    /// </summary>
    /// <param name="BlockName"></param>
    public void ExecuteMainBlock(string BlockName)
    {
        mainFlowchart.GetComponent<Flowchart>().ExecuteBlock(BlockName);
    }
    public void AddStateToPlayer(string StateName, bool isHide = false)
    {

        if (GlobalManager.Instance.player.stateDic.ContainsKey(StateName))
        {
            GlobalManager.Instance.player.stateDic.GetValue(StateName).Duration +=
             GlobalManager.Instance.player.stateDic.GetValue(StateName).RemainTime;
            GlobalManager.Instance.player.stateDic.GetValue(StateName).IsHide = isHide;
        }
        else
        {
            GlobalManager.Instance.player.AddState(StateName);
            GlobalManager.Instance.player.stateDic.GetValue(StateName).IsHide = isHide;
            Debug.Log("玩家获得状态：" + StateName);
        }

    }
    public void AddStateToRoommate(int roommateIndex, string StateName, bool isHide = false)
    {
        if (GlobalManager.Instance.roommates[roommateIndex].stateDic.ContainsKey(StateName))
        {
            GlobalManager.Instance.roommates[roommateIndex].stateDic.GetValue(StateName).Duration +=
             GlobalManager.Instance.roommates[roommateIndex].stateDic.GetValue(StateName).RemainTime;
            GlobalManager.Instance.roommates[roommateIndex].stateDic.GetValue(StateName).IsHide = isHide;
        }
        else
        {
            GlobalManager.Instance.roommates[roommateIndex].AddState(StateName);
            GlobalManager.Instance.roommates[roommateIndex].stateDic.GetValue(StateName).IsHide = isHide;
            Debug.Log(GlobalManager.Instance.roommates[roommateIndex].Name + "获得状态：" + StateName);
        }
    }
    public void AddSlotToPlayer(params string[] SlotName)
    {

    }

    public void AddPropertyToPlayer(int logic, int talk, int athletics, int creativity, int money)
    {
    }


    [UsedImplicitly]
    public void ExitPlace(int index)
    {
        m_CurTopic.Plots[(index - 1)].IsFinish = true;
        EventCenter.Broadcast(EventType.UPDATE_ACTIONPANEL_EVENT);
    }
    [UsedImplicitly]
    public string GetVariable(string variableName)
    {
        switch (variableName)
        {
            case VariableName.PlayerName:
                return GlobalManager.Instance.player.Name;
            default:
                return null;
        }
    }
    private void UpdateVariable()
    {
        mainFlowchart.GetComponent<Flowchart>().SetIntegerVariable("Round", GlobalManager.Instance.player.CurRound + 1);
        mainFlowchart.GetComponent<Flowchart>().ExecuteBlock("Main");
    }
    public void ControllUI(bool flag)
    {
        EventCenter.Broadcast<bool>(EventType.CONTROLL_UI_ON_OFF, flag);
    }
    public void SetSkin(string skin)
    {
        EventCenter.Broadcast<string>(EventType.CHANGE_SKIN, skin);
    }
    #region  Check  
    /// <summary>
    /// 检查舍友关系
    /// </summary>
    /// <param name="roommateIndex">舍友编号</param>
    /// <returns>返回指定舍友关系</returns>
    public int CheckRelationShip(int roommateIndex)
    {
        return GlobalManager.Instance.roommates[roommateIndex].RelationShip;
    }
    /// <summary>
    /// 返回支线是否开启的标识
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool CheckPlotIsLock(int index)
    {
        if (m_CurTopic.Plots[index].IsLock)
            return false;
        else
            return true;
    }
    public bool CheckState(string stateName)
    {
        if (GlobalManager.Instance.player.stateDic.ContainsKey(stateName))
            return true;
        else
            return false;
    }
    public bool CheckRoommateState(string stateName, int id)
    {
        if (GlobalManager.Instance.roommates[id].stateDic.ContainsKey(stateName))
            return true;
        else
            return false;
    }
    /// <summary>
    /// 检测指定ID舍友是否满足属性标准
    /// </summary>
    /// <param name="id"></param>
    /// <param name="logic"></param>
    /// <param name="talk"></param>
    /// <param name="athletics"></param>
    /// <param name="creativity"></param>
    /// <returns></returns>
    public bool CheckRoommateProperty(int id, int logic, int talk, int athletics, int creativity)
    {
        Roommate roommate = GlobalManager.Instance.roommates[id];
        PropertyStruct PS = new PropertyStruct(logic,athletics,talk,creativity);
        if (roommate.propertyStruct>PS)
        {
            return true;
        }
        return false;
    }
    public bool CheckPlayerProperty(int logic, int talk, int athletics, int creativity)
    {
        // TODO:参数得改
        PropertyStruct PS = new PropertyStruct(logic,athletics,talk,creativity);
        if (GlobalManager.Instance.player.propertyStruct>PS)
        {
            return true;
        }
        return false;
    }


    private CharacterAction CheckActionOrder(int order, bool isPlayer = false, int id = 0)
    {
        int index = 1;
        Dictionary<CharacterAction, int> counts = new Dictionary<CharacterAction, int>();
        if (isPlayer)
        {
            foreach (CharacterAction i in XMLManager.Instance.actionList)
            {
                if (!i.EventCaption.Equals(""))
                {
                    counts.Add(i, i.Count[0]);
                }
            }
        }
        else
        {
            foreach (CharacterAction i in XMLManager.Instance.actionList)
            {
                if (!i.EventCaption.Equals(""))
                {
                    counts.Add(i, i.Count[id + 1]);
                }
            }
        }
        DSD.KernalTool.Widget.DictionarySort(counts, ref counts);
        foreach (KeyValuePair<CharacterAction, int> kvp in counts)
        {
            if (index == order)
                return kvp.Key;
            else
                index++;
        }
        return XMLManager.Instance.actionList[10];
    }
    /// <summary>
    /// 统计事件描述指定位次的行动
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public string CheckEventCaptionActionOrder(int order, bool isPlayer = false, int id = 0)
    {

        return CheckActionOrder(order, isPlayer, id).EventCaption;
    }
    public string CheckOptionActionOrder(int order, bool isPlayer = false, int id = 0)
    {
        return CheckActionOrder(order, isPlayer, id).Option;
    }
    public string Check_End_Caption_Action_Order(int order, bool isPlayer = false, int id = 0)
    {
        return CheckActionOrder(order, isPlayer, id).End;
}
    /// <summary>
    /// 检查是否考研失败或未成功
    /// </summary>
    /// <returns>true:考研失败或未考研 </returns>
    public bool CheckKaoYan()
    {
        if ((!CheckPlayerProperty(400, 400, 0, 400) &&
                        CheckState("考研")) || !CheckState("考研"))
        {
            return true;
        }
        else
            return false;
    }
    #region 结局

    // 不会lua以及没有提前看插件就是得这么惨
    public bool End_Shui(int id)
    {
        switch (id)
        {
            // 结局一 （玩家未考研或考研失败）&（与林青水的亲密度达到100）&（林青水持有隐藏状态（公司）)
            case 1:
                {
                    if (!CheckState("考研") ||
                        !CheckPlayerProperty(400, 400, 0, 400)
                        && (CheckRelationShip(0) == 100)
                        && CheckRoommateState("公司", 0))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            // 结局二 （玩家考研成功）or（亲密度未达到100）&（林青水持有隐藏状态（公司））
            case 2:
                {
                    if (CheckState("考研") &&
                        CheckPlayerProperty(400, 400, 0, 400)
                        || (CheckRelationShip(0) <= 100)
                        && CheckRoommateState("公司", 0))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            // 结局三 （志愿者）
            case 3:
                {
                    if (CheckRoommateState("志愿者", 0))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            // 结局四 无（公司）&（志愿者）
            case 4:
                {
                    if (!CheckRoommateState("志愿者", 0) && !CheckRoommateState("公司", 0))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            default:
                return false;
        }
    }
    public bool End_Cheng(int id)
    {
        switch (id)
        {
            // 结局一 （玩家未考研或考研失败）&（与方橙的亲密度达到100）&（方橙持有隐藏状态（游戏））&（逻辑、体能、灵感未全部到达600）
            case 1:
                {
                    if (CheckKaoYan()==true
                        && (CheckRelationShip(1) == 100)
                        && CheckRoommateState("游戏", 1)
                        && !CheckPlayerProperty(600, 0, 600, 600))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            // 结局二 （玩家考研成功）or（亲密度未达到100）&（方橙持有隐藏状态（游戏））&（逻辑、体能、灵感未全部到达600）时显示
            case 2:
                {
                    if ((CheckState("考研") && CheckPlayerProperty(400, 400, 0, 400)
                        || (CheckRelationShip(1) <= 100))
                        && CheckRoommateState("游戏", 1)
                        && !CheckPlayerProperty(600, 0, 600, 600))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            // 结局三 （游戏）&（逻辑、体能、灵感全部达到600）
            case 3:
                {
                    if (CheckRoommateState("游戏", 1) &&
                        CheckPlayerProperty(600, 0, 600, 600))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            // 结尾四：（羽毛球）
            case 4:
                {
                    if (CheckRoommateState("羽毛球", 1))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            // 结局五 无（游戏）&（羽毛球）
            case 5:
                {
                    if (!CheckRoommateState("游戏", 1) && !CheckRoommateState("羽毛球", 1))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            default:
                return false;
        }
    }
    public bool End_Jie(int id)
    {
        switch (id)
        {
            // 结局一 （（杜时节保研或者考研成功）时显示
            case 1:
                {
                    if ((CheckRoommateProperty(2, 400, 400, 0, 400) &&
                        CheckRoommateState("考研", 2)) || (CheckRoommateState("保研", 2))
                        )
                    {
                        return true;
                    }
                    else
                        return false;
                }
            // 结局二 （玩家考研成功）&（杜时节保研或者考研成功）时显示
            case 2:
                {
                    if (((CheckRoommateProperty(2, 400, 400, 0, 400) &&     // 杜时节考研成功或保研
                        CheckRoommateState("考研", 2)) || (CheckRoommateState("保研", 2)))
                        && ((CheckState("考研")) && CheckPlayerProperty(400, 400, 0, 400)))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            // 结局三 （杜时节考研失败）
            case 3:
                {
                    if ((!CheckRoommateProperty(2, 400, 400, 0, 400) &&
                        CheckRoommateState("考研", 2)))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            default:
                return false;
        }
    }

    public bool End_Player(int id)
    {

        // 考研失败或未考研
        if ((CheckKaoYan() == false) || !CheckState("考研"))
        {
            switch (id)
            {
                case 4:
                    if (End_Shui(1))
                        return true;
                    else
                        return false;
                case 5:
                    if (End_Cheng(1))
                        return true;
                    else
                        return false;
                default:
                    return false;
            }
        }
        else
            return false;
    }
    #endregion
    #endregion
    /// <summary>
    /// 增加指定舍友好感度
    /// </summary>
    /// <param name="roommateIndex">舍友编号</param>
    /// <param name="Add_Relation_Value">增加数值</param>
    public void AddRelationShip(int roommateIndex, int Add_Relation_Value)
    {
        GlobalManager.Instance.roommates[roommateIndex].RelationShip += Add_Relation_Value;
    }
    /// <summary>
    /// 增加全部舍友好感度
    /// </summary>
    /// <param name="Add_Relation_Value">增加数值</param>
    public void AddAllRelationShip(int Add_Relation_Value)
    {
        GlobalManager.Instance.roommates[0].RelationShip += Add_Relation_Value;
        GlobalManager.Instance.roommates[1].RelationShip += Add_Relation_Value;
        GlobalManager.Instance.roommates[2].RelationShip += Add_Relation_Value;
    }

}