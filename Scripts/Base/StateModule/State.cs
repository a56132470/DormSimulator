using Base.Struct;

[System.Serializable]
public class State
{
    private StateType m_StType;     // 类型
    private string m_Name;        // 名称
    private int m_Logic;          // 逻辑
    private int m_Talk;           // 言语
    private int m_Athletics;      // 体能
    private int m_Creativity;     // 灵感
    private BonusStruct m_Bonus;  // 属性加成

    public BonusStruct Bonus
    {
        get => m_Bonus;
        set => m_Bonus = value;
    }

    private int m_Duration;       // 持续时间
    private int m_RemainTime;     // 剩余时间
    private string m_OtherEffect; // 其他效果
    private bool m_IsTemp;        // 是否为临时状态
    private bool m_IsHide;        // 是否隐藏

    public State()
    {
        IsHide = false;
    }

    public State(StateType type, string name, int logic, int talk, int athletics, int creativity, int duration, string otherEffect, bool isTemp)
    {
        this.m_StType = type;
        this.m_Name = name;
        this.m_Logic = logic;
        this.m_Talk = talk;
        this.m_Athletics = athletics;
        this.m_Creativity = creativity;
        this.m_Duration = duration;
        this.m_OtherEffect = otherEffect;
        this.m_IsTemp = isTemp;
        IsHide = false;
    }

    public StateType StType { get => m_StType; set => m_StType = value; }
    public string Name { get => m_Name; set => m_Name = value; }
    public int Logic { get => m_Logic; set => m_Logic = value; }
    public int Talk { get => m_Talk; set => m_Talk = value; }
    public int Athletics { get => m_Athletics; set => m_Athletics = value; }
    public int Creativity { get => m_Creativity; set => m_Creativity = value; }
    public int Duration { get => m_Duration; set => m_Duration = value; }
    public string OtherEffect { get => m_OtherEffect; set => m_OtherEffect = value; }
    public bool IsTemp { get => m_IsTemp; set => m_IsTemp = value; }

    public int RemainTime
    {
        get => m_RemainTime;
        set
        {
            m_RemainTime = value;
            if (m_RemainTime < 1)
                m_RemainTime = 0;
        }
    }

    public bool IsHide { get => m_IsHide; set => m_IsHide = value; }
    public int RemainTime1 { get => m_RemainTime; set => m_RemainTime = value; }
}