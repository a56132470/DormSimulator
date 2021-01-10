using System;
using System.Collections.Generic;
using Base.Struct;
using UnityEngine.Serialization;

[Serializable]
/// <summary>
/// 人类，供室友与玩家继承
/// </summary>
public class BasePerson
{
    public string Name { get; set; }
    
    /// <summary>
    /// 属性
    /// </summary>
    [FormerlySerializedAs("property")] public PropertyStruct propertyStruct;
    /// <summary>
    /// 加成
    /// </summary>
    public BonusStruct bonus;


    public int Money { get; set; }

    public Dictionary<string, State> stateDic;

    // 用于数据持久化
    public List<string> stateKeys;

    public List<State> stateValues;

    public string[,] records = new string[24, 5];


    // 当前回合
    protected int m_CurRound;
    /// <summary>
    /// 当前回合
    /// </summary>
    public int CurRound
    {
        get
        {
            return m_CurRound;
        }
        set
        {
            m_CurRound = value;
            if (m_CurRound > 24)
            {
                m_CurRound = 24;
            }
            if (m_CurRound <= 0)
            {
                m_CurRound = 1;
            }
        }
    }
    /// <summary>
    /// 添加从XML文件中获取的状态到当前字典
    /// </summary>
    /// <param name="Key"></param>
    public void AddState(string Key)
    {
        if (XMLManager.Instance.stateDic.ContainsKey(Key))
        {
            State state = XMLManager.Instance.stateDic.GetValue(Key);
            state.RemainTime = state.Duration;
            stateDic.Add(Key, XMLManager.Instance.stateDic.GetValue(Key));
            bonus += state.Bonus;
        }
    }
    
    public virtual void AddRecordAction(string content) { }
    
}