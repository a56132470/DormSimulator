namespace Base.Struct
{
    public struct BonusStruct
    {
        /// <summary>
        /// 逻辑加成
        /// </summary>
        public int LogicBonus { get; set; }
        /// <summary>
        /// 体能加成
        /// </summary>
        public int AthleticsBonus { get; set; }
        /// <summary>
        /// 口才加成
        /// </summary>
        public int TalkBonus { get; set; }
        /// <summary>
        /// 灵感加成
        /// </summary>
        public int CreativityBonus { get; set; }
        
        public BonusStruct(int logicBonus, int athleticsBonus, int talkBonus, int creativityBonus)
        {
            LogicBonus = logicBonus;
            AthleticsBonus = athleticsBonus;
            TalkBonus = talkBonus;
            CreativityBonus = creativityBonus;
        }

        public static BonusStruct operator +(BonusStruct BS1,BonusStruct BS2)
        {
            return new BonusStruct(BS1.LogicBonus+BS2.LogicBonus,
                                   BS1.AthleticsBonus+BS2.AthleticsBonus,
                                   BS1.TalkBonus+BS2.TalkBonus,
                                   BS1.CreativityBonus+BS2.CreativityBonus);
        }

        public static BonusStruct operator -(BonusStruct BS1, BonusStruct BS2)
        {
            return new BonusStruct(BS1.LogicBonus - BS2.LogicBonus,
                BS1.AthleticsBonus - BS2.AthleticsBonus,
                BS1.TalkBonus - BS2.TalkBonus,
                BS1.CreativityBonus - BS2.CreativityBonus);
        }
    }
}