using Base.ActionSystem;
using Base.Struct;
using UnityEngine;

[System.Serializable]
public class Player : BasePerson
{
    // 体力
    [SerializeField]
    private int m_Strength;



    // 周目，暂时找不到周目的单词
    private int m_CurWeek;

    public Player()
    {
        Name = "";
        records = new string[24, 5];
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="name"></param>
    /// <param name="logic"></param>
    /// <param name="talk"></param>
    /// <param name="athletics"></param>
    /// <param name="creativity"></param>
    /// <param name="money"></param>
    public Player(string name, int logic, int talk, int athletics, int creativity, int money)
    {
        Name = name;
        propertyStruct = new PropertyStruct(logic,athletics,talk,creativity);
        Money = money;
        // 成长值均为0
        bonus = new BonusStruct(0,0,0,0);

        m_Strength = 5;
        CurWeek = 1;
        m_CurRound = 1;
        stateDic = new System.Collections.Generic.Dictionary<string, State>();
        records = new string[24, 5];
    }

    /// <summary>
    /// 体力
    /// </summary>
    public int Strength
    {
        get => m_Strength;
        set
        {
            m_Strength = value;

            // 体力限制在0--5之间
            if (m_Strength < 0)
                m_Strength = 0;
            if (m_Strength > 5)
                m_Strength = 5;
            if (StrengthManager.Instance != null)
            {
                StrengthManager.Instance.ChangeStrength(m_Strength);
            }
        }
    }

    /// <summary>
    /// 当前周目
    /// </summary>
    public int CurWeek
    {
        get
        {
            return m_CurWeek;
        }
        set
        {
            m_CurWeek = value;
            if (m_CurWeek <= 0)
            {
                m_CurWeek = 1;
            }
        }
    }
    public override void AddRecordAction(string content)
    {
        base.AddRecordAction(content);
        {
            int j = 0;
            for (int i = 0; i < 5; i++)
            {
                if (records[CurRound - 1, i] == null)
                {
                    j = i;
                    break;
                }
            }
            records[CurRound - 1, j] = content;
        }
    }
}