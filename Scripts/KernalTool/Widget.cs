using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Base.ActionSystem;
using Base.Struct;

namespace DSD.KernalTool
{
    public static partial class Widget
    {
        /// <summary>
        /// 在total个连续数字里取n个不重复数字，并返回不重复数字数组
        /// </summary>
        /// <param name="total">在几个连续数字里面取</param>
        /// <param name="n">取到的不重复数字的数组长度</param>
        /// <returns></returns>
        public static int[] GetRandomSequence(int total, int n)
        {
            if (total < n) return null;
            // 随机总数组
            int[] sequence = new int[total];
            // 取到的不重复数字的数组
            int[] output = new int[n];
            for (int i = 0; i < total; i++)
            {
                sequence[i] = i;
            }
            int end = total - 1;
            for (int i = 0; i < n; i++)
            {
                // 随机一个数，每随机一次，随机区间-1
                int num = Random.Range(0, end + 1);
                output[i] = sequence[num];
                // 将区间最后一个数复制到取到值上
                sequence[num] = sequence[end];
                end--;
                // 执行一次效果如:1,2,3,4,5，取到2
                // 则下次随机区间变为1,5,3,4
            }
            return output;
        }

        /// <summary>
        /// 中国式四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static double ChinaRound(float value, int decimals)
        {
            if (value < 0)
                return System.Math.Round(value + 5 / System.Math.Pow(10, decimals + 1), decimals, System.MidpointRounding.AwayFromZero);
            else
                return System.Math.Round(value, decimals, System.MidpointRounding.AwayFromZero);
        }

        public static bool JudgingFirstSuccess(CharacterAction action, BasePerson person)
        {
            float successRate = 0;
            int index = 0;
            // TODO:因为要交毕设而先让其勉强能运行，之后还需要改
            if (action.NeedProperty.Athletics != 0)
            {
                index++;
                successRate = successRate + (action.SuccessRate +
                            ((person.propertyStruct.Athletics - action.NeedProperty.Athletics) /
                             (person.propertyStruct.Athletics + action.NeedProperty.Athletics)));
            }
            if (action.NeedProperty.Creativity != 0)
            {
                index++;
                successRate += action.SuccessRate +
                     ((person.propertyStruct.Creativity - action.NeedProperty.Creativity) /
                     (person.propertyStruct.Creativity + action.NeedProperty.Creativity));
            }
            if (action.NeedProperty.Logic != 0)
            {
                index++;
                successRate += action.SuccessRate +
                     ((person.propertyStruct.Logic - action.NeedProperty.Logic) /
                     (person.propertyStruct.Logic + action.NeedProperty.Logic));
            }
            if (action.NeedProperty.Talk != 0)
            {
                index++;
                successRate += action.SuccessRate +
                     ((person.propertyStruct.Talk - action.NeedProperty.Talk) /
                     (person.propertyStruct.Talk + action.NeedProperty.Talk));
            }
            if (index != 0)
                successRate /= index;
            else
                successRate = 1;
            if (successRate > 0)
            {
                // 成功
                return true;
            }
            else
            {
                // 失败
                return false;
            }
        }

        public static bool JudgingSecondSuccess(CharacterAction action, BasePerson person)
        {
            float successRate = 0;
            int index = 0;
            // TODO:因为要交光盘所以先临时让其可以运行，之后还需再改，改得通用一些
            if (action.NeedProperty.Athletics != 0)
            {
                index++;
                successRate += person.propertyStruct.Athletics /
                    (person.propertyStruct.Athletics + action.NeedProperty.Athletics);
            }
            if (action.NeedProperty.Creativity != 0)
            {
                index++;
                successRate += person.propertyStruct.Creativity /
                    (person.propertyStruct.Creativity + action.NeedProperty.Creativity);
            }
            if (action.NeedProperty.Logic != 0)
            {
                index++;
                successRate += person.propertyStruct.Logic /
                    (person.propertyStruct.Logic + action.NeedProperty.Logic);
            }
            if (action.NeedProperty.Talk != 0)
            {
                index++;
                successRate += person.propertyStruct.Talk /
                    (person.propertyStruct.Talk + action.NeedProperty.Talk);
            }
            if (index != 0)
                successRate /= index;
            else
                successRate = 1;
            if (successRate > 0)
            {
                // 大成功
                return true;
            }
            else
            {
                // 成功
                return false;
            }
        }
        /// <summary>
        /// 根据当前选择的action为player加属性
        /// 在舍友不会自己获取状态的时候，就全部用玩家的属性加成
        /// </summary>
        public static void AddProperty(BasePerson person,CharacterAction action)
        {
            // 是玩家
            if(person.GetType()==GlobalManager.Instance.player.GetType())
            {
                if (!action.EventCaption.Equals(""))
                    action.Count[0]++;
            }
            else
            {
                if (person.Name == GlobalManager.Instance.roommates[0].Name)
                {
                    if (!action.EventCaption.Equals(""))
                        action.Count[1]++;
                }
                else if (person.Name == GlobalManager.Instance.roommates[1].Name)
                {
                    if (!action.EventCaption.Equals(""))
                        action.Count[2]++;
                }
                else if (person.Name == GlobalManager.Instance.roommates[2].Name)
                {
                    if (!action.EventCaption.Equals(""))
                        action.Count[3]++;
                }
            }

            float multiple;
            int promote_Money;
            PropertyStruct promoteProperty = new PropertyStruct(0,0,0,0);
            promoteProperty += action.Property;
            promoteProperty += GlobalManager.Instance.player.bonus;

            promote_Money = action.Money;
            if (JudgingFirstSuccess(action, person))
            {
                multiple = 1;
                person.AddRecordAction(action.Captions[1]);
                if (JudgingSecondSuccess(action, person))
                {
                    multiple = 2;
                    person.AddRecordAction(action.Captions[2]);
                }
            }
            else
            {
                multiple = 0.5f;
                person.AddRecordAction(action.Captions[0]);
            }

            promoteProperty *= multiple;
            person.propertyStruct += promoteProperty;
            person.Money += (int)ChinaRound(promote_Money * multiple, 0);

        }
        public static void DictionarySort(Dictionary<CharacterAction,int>dic,ref Dictionary<CharacterAction,int> refDic)
        {
            refDic = dic.OrderByDescending(o => o.Value).ToDictionary(o => o.Key, p => p.Value);
        }
        public static void RemoveInivitationState()
        {

            for (int i = 1; i < 4; i++)
            {
                if (GlobalManager.Instance.player.stateDic.ContainsKey(StateName.Help + i))
                {
                    GlobalManager.Instance.player.bonus-=(GlobalManager.Instance.player.stateDic[StateName.Help + i].Bonus);
                    GlobalManager.Instance.player.stateDic.Remove(StateName.Help + i);
                }
            }
            GlobalManager.Instance.Invitation = 0;
        }
    }
}