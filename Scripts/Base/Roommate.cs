using DSD.KernalTool;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// 舍友类
/// </summary>
public class Roommate : BasePerson
{
    [SerializeField]
    private int relationShip;

    [SerializeField]
    private int selfControl;

    [SerializeField]
    private int id;                  // 标识是哪个舍友

    public string[,] records;

    public Roommate()
    {
        Name = "";
        records = new string[24, 5];
    }

    /// <summary>
    /// 初始化，配置表初始化之后弄
    /// </summary>
    /// <param name="name"></param>
    public Roommate(string name, int learn, int eloquence, int athletics, int creativity, int money, int selfControl, int relationShip, int id)
    {
        Name = name;
        Logic = learn;
        Talk = eloquence;
        Athletics = athletics;
        Creativity = creativity;
        Money = money;
        SelfControl = selfControl;
        RelationShip = relationShip;
        ID = id;

        // 成长值均为0
        LogicBonus = 0;
        TalkBonus = 0;
        AthleticsBonus = 0;
        CreativityBonus = 0;
        stateDic = new Dictionary<string, State>();
        records = new string[24, 5];
    }

    public void AddRecordAction()
    {
        List<CharacterAction> characterActions = new List<CharacterAction>();
        float multiple = 1;
        int promote_Athletics, promote_Creativity, promote_Logic, promote_Money, promoty_talk;
        int randomNum;
        int increment;
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
            randomNum = UnityEngine.Random.Range(0, characterActions.Count);

            promote_Athletics = characterActions[randomNum].Athletics + AthleticsBonus;
            promote_Creativity = characterActions[randomNum].Creativity + CreativityBonus; ;
            promote_Logic = characterActions[randomNum].Logic + LogicBonus;
            promote_Money = characterActions[randomNum].Money;
            promoty_talk = characterActions[randomNum].Talk + TalkBonus;
            increment = characterActions[randomNum].SelfControlIncrement;
            // 成功
            if (Widget.JudgingFirstSuccess(characterActions[randomNum], this))
            {
                multiple = 1;

                records[GlobalVariable.instance.player.CurRound - 1, i] = characterActions[randomNum].Captions[1];

                if (Widget.JudgingSecondSuccess(characterActions[randomNum], this))
                {
                    // 大成功
                    records[GlobalVariable.instance.player.CurRound - 1, i] = characterActions[randomNum].Captions[2];
                    multiple = 2;
                    if (increment > 0) increment++;
                    else increment--;
                    break;
                }
            }
            else
            {
                // 失败
                records[GlobalVariable.instance.player.CurRound - 1, i] = characterActions[randomNum].Captions[0];
                multiple = 0.5f;
                increment = -increment;
            }
            SelfControl += increment;
            Athletics += (int)Widget.ChinaRound(promote_Athletics * multiple, 0);
            Creativity += (int)Widget.ChinaRound(promote_Creativity * multiple, 0);
            Logic += (int)Widget.ChinaRound(promote_Logic * multiple, 0);
            Money += (int)Widget.ChinaRound(promote_Money * multiple, 0);
            Talk += (int)Widget.ChinaRound(promoty_talk * multiple, 0);
        }
    }

    public int RelationShip { get => relationShip; set => relationShip = value; }
    public int SelfControl { get => selfControl; set => selfControl = value; }
    public int ID { get => id; set => id = value; }
}