public class State
{
    private StateType type;     // 类型
    private string name;        // 名称
    private int logic;          // 逻辑
    private int talk;           // 言语
    private int athletics;      // 体能
    private int creativity;     // 灵感
    private int duration;       // 持续时间
    private int remainTime;     // 剩余时间
    private string otherEffect; // 其他效果
    private bool isTemp;        // 是否为临时状态

    public State()
    {
    }

    public State(StateType type, string name, int logic, int talk, int athletics, int creativity, int duration, string otherEffect, bool isTemp)
    {
        this.type = type;
        this.name = name;
        this.logic = logic;
        this.talk = talk;
        this.athletics = athletics;
        this.creativity = creativity;
        this.duration = duration;
        this.otherEffect = otherEffect;
        this.isTemp = isTemp;
    }

    public StateType Type { get => type; set => type = value; }
    public string Name { get => name; set => name = value; }
    public int Logic { get => logic; set => logic = value; }
    public int Talk { get => talk; set => talk = value; }
    public int Athletics { get => athletics; set => athletics = value; }
    public int Creativity { get => creativity; set => creativity = value; }
    public int Duration { get => duration; set => duration = value; }
    public string OtherEffect { get => otherEffect; set => otherEffect = value; }
    public bool IsTemp { get => isTemp; set => isTemp = value; }

    public int RemainTime
    {
        get => remainTime;
        set
        {
            remainTime = value;
            if (remainTime < 1)
                remainTime = 0;
        }
    }
}