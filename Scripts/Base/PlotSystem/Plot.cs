using System.Text;
using Base.Struct;

namespace Base.PlotSystem
{
    public class Plot
    {
        // 需要包含解锁条件，剧情内容
        // 当前情节是否解锁
        public bool IsLock = true;

        public bool IsFinish = false;

        // 解锁所需属性
        public int NeedLogic, NeedAthletics, NeedTalk, NeedCreativity,
            NeedMaxRound,NeedMinRound;

        public PropertyStruct needProperty;
        public int NeedRelationShip;

        // 所需状态，所需物品
        public StringBuilder needState, needSlot;

        public string Place;

        public Plot()
        {
            IsLock = true;
            needSlot = new StringBuilder();
            needState = new StringBuilder();
        }

        public Plot(int needlogic, int needathletics, int needtalk, int needcreativity,int needRound, string type)
        {
            IsLock = true;
            needSlot = new StringBuilder();
            needState = new StringBuilder();
            NeedLogic = needlogic;
            NeedAthletics = needathletics;
            NeedTalk = needtalk;
            NeedCreativity = needcreativity;
            Place = type;
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
            PropertyStruct PS1 = new PropertyStruct(NeedLogic,NeedAthletics,NeedTalk,NeedCreativity);
            bool flag = GlobalManager.Instance.player.propertyStruct>=PS1 &&
                        GlobalManager.Instance.player.CurRound>=NeedMinRound &&
                        GlobalManager.Instance.player.CurRound<=NeedMaxRound;
            // TODO:这里关联舍友写死了，之后如果还要用，需要写活
            // 如果跟林青水关系小于所需值
            if(PlotManager.Instance.CheckRelationShip(0)<NeedRelationShip)
            {
                flag = false;
            }
            //// 检测状态，物品 状态物品目前支线用不到
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
}