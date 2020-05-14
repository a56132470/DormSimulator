using System.Text;

public class Plot
{
    // 需要包含解锁条件，剧情内容
    // 当前情节是否解锁
    public bool isLock = true;

    public bool isFinish = false;

    // 解锁所需属性
    public int needLogic, needAthletics, needTalk, needCreativity;

    // 所需状态，所需物品
    public StringBuilder needState, needSlot;

    public string place;

    public Plot()
    {
        isLock = true;
        needSlot = new StringBuilder();
        needState = new StringBuilder();
    }

    public Plot(int needlogic, int needathletics, int needtalk, int needcreativity, string type)
    {
        isLock = true;
        needSlot = new StringBuilder();
        needState = new StringBuilder();
        needLogic = needlogic;
        needAthletics = needathletics;
        needTalk = needtalk;
        needCreativity = needcreativity;
        place = type;
    }

    public void AddState(params string[] stateNames)
    {
        for (int i = 0; i < stateNames.Length; i++)
        {
            needState.Append(stateNames[i]);
            if (i != (stateNames.Length - 1))
            {
                needState.Append(",");
            }
        }
    }

    public void AddSlot(params string[] slotNames)
    {
        for (int i = 0; i < slotNames.Length; i++)
        {
            needSlot.Append(slotNames[i]);
            if (i != (slotNames.Length - 1))
            {
                needSlot.Append(",");
            }
        }
    }

    /// <summary>
    /// 检查解锁条件
    /// </summary>
    /// <returns></returns>
    public bool CheckUnlockCondition()
    {
        bool flag = false;
        if (GlobalVariable.instance.player.Logic >= needLogic &&
            GlobalVariable.instance.player.Athletics >= needAthletics &&
            GlobalVariable.instance.player.Talk >= needTalk &&
            GlobalVariable.instance.player.Creativity >= needCreativity)
        {
            flag = true;
        }
        //// 检测状态，物品
        //if (!needState.Equals(""))
        //{
        //    string[] strs = needState.ToString().Split(',');
        //    Debug.Log("需要状态为:" + needState);
        //    for (int i = 0; i < strs.Length; i++)
        //    {
        //        if (!GlobalVariable.instance.player.stateDic.ContainsKey(strs[i]))
        //        {
        //            Debug.Log("当前情节所需状态为：" + strs[i] + ",玩家未存在此状态，故不符合");
        //            flag = false;
        //            break;
        //        }
        //    }
        //}
        //if (!needSlot.Equals(""))
        //{
        //    string[] strs = needState.ToString().Split(',');
        //    int index = 0;
        //    for (int i = 0; i < strs.Length; i++)
        //    {
        //        for (int j = 0; j < GlobalVariable.instance.MyBag.itemList.Count; j++)
        //        {
        //            if (GlobalVariable.instance.MyBag.itemList[j].ItemName.Equals(strs[i]))
        //            {
        //                index++;
        //            }
        //        }
        //    }
        //    if(index<strs.Length)
        //    {
        //        flag = false;
        //    }
        //}
        return flag;
    }
}