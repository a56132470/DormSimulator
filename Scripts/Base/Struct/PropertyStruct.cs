using DSD.KernalTool;

namespace Base.Struct
{
    /// <summary>
    /// 通用的属性结构体：逻辑、体能、言语、灵感
    /// </summary>
    public struct PropertyStruct
    {
        private int m_Logic;

        public int Logic
        {
            get => m_Logic;
            set => m_Logic = value;
        }

        public int Athletics
        {
            get => m_Athletics;
            set => m_Athletics = value;
        }

        public int Talk
        {
            get => m_Talk;
            set => m_Talk = value;
        }

        public int Creativity
        {
            get => m_Creativity;
            set => m_Creativity = value;
        }

        private int m_Athletics;
        private int m_Talk;
        private int m_Creativity;

        public PropertyStruct(int logic, int athletics, int talk, int creativity)
        {
            m_Logic = logic;
            m_Athletics = athletics;
            m_Talk = talk;
            m_Creativity = creativity;
        }

        public static PropertyStruct operator +(PropertyStruct PS1, BonusStruct BS2)
        {
            return new PropertyStruct(PS1.Logic + BS2.LogicBonus,
                PS1.Athletics + BS2.AthleticsBonus,
                PS1.Talk + BS2.TalkBonus,
                PS1.Creativity + BS2.CreativityBonus);
        }

        public static PropertyStruct operator -(PropertyStruct PS1, BonusStruct BS2)
        {
            return new PropertyStruct(PS1.Logic - BS2.LogicBonus,
                PS1.Athletics - BS2.AthleticsBonus,
                PS1.Talk - BS2.TalkBonus,
                PS1.Creativity - BS2.CreativityBonus);
        }
        public static PropertyStruct operator +(PropertyStruct PS1, PropertyStruct PS2)
        {
            return new PropertyStruct(PS1.Logic + PS2.Logic,
                PS1.Athletics + PS2.Athletics,
                PS1.Talk + PS2.Talk,
                PS1.Creativity + PS2.Creativity);
        }

        public static PropertyStruct operator -(PropertyStruct PS1, PropertyStruct PS2)
        {
            return new PropertyStruct(PS1.Logic + PS2.Logic,
                PS1.Athletics + PS2.Athletics,
                PS1.Talk + PS2.Talk,
                PS1.Creativity + PS2.Creativity);
        }

        public static bool operator >(PropertyStruct PS1, PropertyStruct PS2)
        {
            if (PS1.Logic > PS2.Logic &&
                PS1.Athletics > PS2.Athletics &&
                PS1.Creativity > PS2.Creativity &&
                PS1.Talk > PS2.Talk)
            {
                return true;
            }
            return false;
        }

        public static bool operator >=(PropertyStruct PS1, PropertyStruct PS2)
        {
            if (PS1.Logic >= PS2.Logic &&
                PS1.Athletics >= PS2.Athletics &&
                PS1.Creativity >= PS2.Creativity &&
                PS1.Talk >= PS2.Talk)
            {
                return true;
            }
            return false;
        }
        public static bool operator <(PropertyStruct PS1, PropertyStruct PS2)
        {
            if (PS1.Logic < PS2.Logic &&
                PS1.Athletics < PS2.Athletics &&
                PS1.Creativity < PS2.Creativity &&
                PS1.Talk < PS2.Talk)
            {
                return true;
            }
            return false;
        }

        public static bool operator <=(PropertyStruct PS1, PropertyStruct PS2)
        {
            if (PS1.Logic <= PS2.Logic &&
                PS1.Athletics <= PS2.Athletics &&
                PS1.Creativity <= PS2.Creativity &&
                PS1.Talk <= PS2.Talk)
            {
                return true;
            }
            return false;
        }
        public static PropertyStruct operator *(PropertyStruct PS1, int i)
        {
            return new PropertyStruct(PS1.Logic*i,PS1.Athletics*i,PS1.Talk*i,PS1.Creativity*i);
        }
        public static PropertyStruct operator *(PropertyStruct PS1, float f)
        {
            return new PropertyStruct(
                (int)Widget.ChinaRound(PS1.Logic*f,0) ,
                (int)Widget.ChinaRound(PS1.Athletics*f,0),
                (int)Widget.ChinaRound(PS1.Talk*f,0),
                (int)Widget.ChinaRound(PS1.Creativity*f,0));
        }
        
    }
}