/// <summary>
/// 角色行动类，作为类的实例绑定在行动按钮上
/// <para>可作为舍友的行动类，也可作为玩家的行动类</para>
/// </summary>
public class CharacterAction
{
    private ActionType type;    // 类型
    private string name;        // 名称
    private int consume;        // 消耗
    private int logic;          // 逻辑
    private int talk;           // 言语
    private int athletics;      // 体能
    private int creativity;     // 灵感
    private int money;          // 零花
    private float successRate;  // 基础成功率
    private int selfControl;    // 需求自控力
    private string caption;     // 描述
    private int needLogic;      // 需求逻辑
    private int needTalk;       // 需求言语
    private int needAthletics;  // 需求体能
    private int needCreativity; // 需求创造
    private string[] captions;  // 三个描述
    private Roommate partner;   // 当前邀请的角色
    private int selfControlIncrement;   // 自制力增量

    public CharacterAction()
    {
    }

    public CharacterAction(ActionType type, string name, int consume, int logic, int talk, int athletics, int creativity, int money, string caption)
    {
        Type = type;
        Name = name;
        Consume = consume;
        Logic = logic;
        Talk = talk;
        Athletics = athletics;
        Creativity = creativity;
        Money = money;
        Caption = caption;
        captions = new string[3];
        switch (type)
        {
            case ActionType.Amusement:
                selfControlIncrement = -2;
                break;

            case ActionType.Labor:
                selfControlIncrement = 2;
                break;

            case ActionType.Study:
                selfControlIncrement = 1;
                break;
        }
    }

    public ActionType Type { get => type; set => type = value; }
    public string Name { get => name; set => name = value; }
    public int Consume { get => consume; set => consume = value; }
    public int Logic { get => logic; set => logic = value; }
    public int Talk { get => talk; set => talk = value; }
    public int Athletics { get => athletics; set => athletics = value; }
    public int Creativity { get => creativity; set => creativity = value; }
    public int Money { get => money; set => money = value; }
    public string Caption { get => caption; set => caption = value; }
    public int NeedLogic { get => needLogic; set => needLogic = value; }
    public int NeedTalk { get => needTalk; set => needTalk = value; }
    public int NeedAthletics { get => needAthletics; set => needAthletics = value; }
    public int NeedCreativity { get => needCreativity; set => needCreativity = value; }
    public string[] Captions { get => captions; set => captions = value; }
    public float SuccessRate { get => successRate; set => successRate = value; }
    public int SelfControl { get => selfControl; set => selfControl = value; }
    public int SelfControlIncrement { get => selfControlIncrement; set => selfControlIncrement = value; }
}