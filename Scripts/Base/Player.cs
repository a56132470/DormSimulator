using UnityEngine;

[System.Serializable]
public class Player : BasePerson
{
    // 体力
    [SerializeField]
    private int strength;

    // 当前回合
    private int curRound;

    // 周目，暂时找不到周目的单词
    private int curWeek;

    public string[,] records;

    public Player()
    {
        Name = "";
        records = new string[24, 5];
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="name"></param>
    public Player(string name, int logic, int talk, int athletics, int creativity, int money)
    {
        Name = name;
        Logic = logic;
        Talk = talk;
        Athletics = athletics;
        Creativity = creativity;
        Money = money;
        // 成长值均为0
        LogicBonus = 0;
        TalkBonus = 0;
        AthleticsBonus = 0;
        CreativityBonus = 0;
        strength = 5;
        CurWeek = 1;
        curRound = 1;
        stateDic = new System.Collections.Generic.Dictionary<string, State>();
        records = new string[24, 5];
    }

    /// <summary>
    /// 体力
    /// </summary>
    public int Strength
    {
        get => strength;
        set
        {
            strength = value;

            // 体力限制在0--5之间
            if (strength < 0)
                strength = 0;
            if (strength > 5)
                strength = 5;
            if (StrengthManager.Instance != null)
            {
                StrengthManager.Instance.ChangeStrength(strength);
            }
        }
    }

    /// <summary>
    /// 当前回合
    /// </summary>
    public int CurRound
    {
        get
        {
            return curRound;
        }
        set
        {
            curRound = value;
            if (curRound > 24)
            {
                curRound = 24;
            }
            if (curRound <= 0)
            {
                curRound = 1;
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
            return curWeek;
        }
        set
        {
            curWeek = value;
            if (curWeek <= 0)
            {
                curWeek = 1;
            }
        }
    }


    public void AddRecordAction(string content)
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