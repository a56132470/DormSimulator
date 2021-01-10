using Base.Struct;

namespace Base.ActionSystem
{
    /// <summary>
    /// 角色行动类，作为类的实例绑定在行动按钮上
    /// <para>可作为舍友的行动类，也可作为玩家的行动类</para>
    /// </summary>
    public class CharacterAction
    {
        public CharacterAction()
        {
            Property = new PropertyStruct(0,0,0,0);
            NeedProperty = new PropertyStruct(0,0,0,0);
        }

        public CharacterAction(ActionType type, string name, int consume, int logic, int talk, int athletics, int creativity, int money, string caption)
        {
            Type = type;
            Name = name;
            Consume = consume;
            Property = new PropertyStruct(logic,athletics,talk,creativity);
            Money = money;
            Captions = new string[3];
            switch (type)
            {
                case ActionType.Amusement:
                    SelfControlIncrement = -2;
                    ConsumeBonus = 0;
                    break;

                case ActionType.Labor:
                    SelfControlIncrement = 2;
                    ConsumeBonus = -1;
                    break;

                case ActionType.Study:
                    SelfControlIncrement = 1;
                    ConsumeBonus = 0;
                    break;
            }
        }

        public ActionType Type { get; set; }

        public string Name { get; set; }

        public int Consume { get; set; }

        public int Money { get; set; }
        public PropertyStruct Property { get; set; }
        public PropertyStruct NeedProperty { get; set; }

        public string[] Captions { get; set; }

        public float SuccessRate { get; set; }

        public int SelfControl { get; set; }

        public int SelfControlIncrement { get; set; }

        public int ConsumeBonus { get; set; }

        public string EventCaption { get; set; }
        public string Option { get; set; }
        public string End { get; set; }
        public int NeedMaxRound { get; set; }
        public int NeedMinRound { get; set; }
        public int[] Count { get; set; }
    }
}