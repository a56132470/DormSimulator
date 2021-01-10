using DSD.KernalTool;
using System.Collections.Generic;
using Base.ActionSystem;
using Base.Struct;
using UnityEngine;

[System.Serializable]
/// <summary>
/// 舍友类
/// </summary>
public class Roommate : BasePerson
{
    [SerializeField]
    private int m_RelationShip;

    [SerializeField]
    private int m_SelfControl;

    [SerializeField]
    private int m_Id;                  // 标识是哪个舍友

    public Roommate()
    {
        Name = "";
        records = new string[24, 5];
    }
    public new int CurRound
    {
        get
        {
            return GlobalManager.Instance.player.CurRound;
        }
    }

    /// <summary>
    /// 初始化，配置表初始化之后弄
    /// </summary>
    /// <param name="name"></param>
    /// <param name="learn"></param>
    /// <param name="eloquence"></param>
    /// <param name="athletics"></param>
    /// <param name="creativity"></param>
    /// <param name="money"></param>
    /// <param name="selfControl"></param>
    /// <param name="relationShip"></param>
    /// <param name="id"></param>
    public Roommate(string name, int logic, int talk, int athletics, int creativity, int money, int selfControl, int relationShip, int id)
    {
        Name = name;
        propertyStruct = new PropertyStruct(logic,athletics,talk,creativity);
        Money = money;
        SelfControl = selfControl;
        RelationShip = relationShip;
        ID = id;

        // 成长值均为0
        bonus = new BonusStruct(0,0,0,0);
        stateDic = new Dictionary<string, State>();
        records = new string[24, 5];
    }

    public void AddFiveRecordAction()
    {
        List<CharacterAction> characterActions = new List<CharacterAction>();
        // 筛选出自控力符合的action
        for (int i = 0; i < XMLManager.Instance.characterActionArray.Length; i++)
        {
            for (int j = 0; j < XMLManager.Instance.characterActionArray[i].Count; j++)
            {
                if (SelfControl >= XMLManager.Instance.characterActionArray[i][j].SelfControl)
                {
                    characterActions.Add(XMLManager.Instance.characterActionArray[i][j]);
                }
            }
        }
        for (int i = 0; i < 5; i++)
        {
            var randomNum = Random.Range(0, characterActions.Count);

            if(!characterActions[randomNum].EventCaption.Equals(""))
            {
                if(Name.Equals(GlobalManager.Instance.roommates[0]))
                {
                    characterActions[randomNum].Count[1]++;
                }
                if (Name.Equals(GlobalManager.Instance.roommates[1]))
                {
                    characterActions[randomNum].Count[2]++;
                }
                if (Name.Equals(GlobalManager.Instance.roommates[2]))
                {
                    characterActions[randomNum].Count[3]++;
                }
            }
            var promoteProperty = new PropertyStruct(0,0,0,0);
            promoteProperty += characterActions[randomNum].Property;
            promoteProperty += bonus;
            int promoteMoney = characterActions[randomNum].Money;
            int increment = characterActions[randomNum].SelfControlIncrement;
            float multiple;
            // 成功
            if (Widget.JudgingFirstSuccess(characterActions[randomNum], this))
            {
                multiple = 1;

                records[GlobalManager.Instance.player.CurRound - 1, i] = characterActions[randomNum].Captions[1];

                if (Widget.JudgingSecondSuccess(characterActions[randomNum], this))
                {
                    // 大成功
                    records[GlobalManager.Instance.player.CurRound - 1, i] = characterActions[randomNum].Captions[2];
                    if (increment > 0) increment++;
                    else increment--;
                }
            }
            else
            {
                // 失败
                records[GlobalManager.Instance.player.CurRound - 1, i] = characterActions[randomNum].Captions[0];
                multiple = 0.5f;
                increment = -increment;
            }
            SelfControl += increment;
            propertyStruct += promoteProperty * increment;
            Money += (int)Widget.ChinaRound(promoteMoney * multiple, 0);
        }
    }
    public override void AddRecordAction(string content)
    {
        base.AddRecordAction(content);
        {
            int j = 0;
            for (var i = 0; i < 5; i++)
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
    public int RelationShip
    {
        get => m_RelationShip;
        set
        {
            m_RelationShip = value;
            if (m_RelationShip >= 100)
                m_RelationShip = 100;
        }
    }
    public int SelfControl
    {
        get => m_SelfControl;
        set
        {
            m_SelfControl = value;
            if (m_SelfControl >= 100)
                m_SelfControl = 100;
            if (m_SelfControl <= 0)
                m_SelfControl = 0;
        }
    }
    public int ID { get => m_Id; set => m_Id = value; }
}