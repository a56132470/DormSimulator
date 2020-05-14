[System.Serializable]
/// <summary>
/// 人类，供室友与玩家继承
/// </summary>
public class BasePerson
{
    public string Name { get; set; }

    /// <summary>
    /// 学习能力
    /// </summary>
    public int Logic { get; set; }

    /// <summary>
    /// 社交能力
    /// </summary>
    public int Talk { get; set; }

    /// <summary>
    /// 运动能力
    /// </summary>
    public int Athletics { get; set; }

    /// <summary>
    /// 创造力
    /// </summary>
    public int Creativity { get; set; }

    /// <summary>
    /// 学习能力成长值
    /// </summary>
    public int LogicBonus { get; set; }

    /// <summary>
    /// 社交能力成长值
    /// </summary>
    public int TalkBonus { get; set; }

    /// <summary>
    /// 运动能力成长值
    /// </summary>
    public int AthleticsBonus { get; set; }

    /// <summary>
    /// 创造力成长值
    /// </summary>
    public int CreativityBonus { get; set; }

    public int Money { get; set; }

    public System.Collections.Generic.Dictionary<string, State> stateDic;

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
            AddBonus(state);
        }
    }
    public void AddBonus(State state)
    {
        LogicBonus += state.Logic;
        TalkBonus += state.Talk;
        CreativityBonus += state.Creativity;
        AthleticsBonus += state.Athletics;
    }

    public void SubBonus(State state)
    {
        LogicBonus -= state.Logic;
        TalkBonus -= state.Talk;
        CreativityBonus -= state.Creativity;
        AthleticsBonus -= state.Athletics;
    }
}