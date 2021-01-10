using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Base.ActionSystem;
using Base.PlotSystem;
using Base.Struct;
using UnityEngine;
/// <summary>
/// 加载ActionXML，生成actionList，供外部panel使用
/// </summary>
public class XMLManager : MonoBehaviour
{
    public static XMLManager Instance { get; private set; }

    private Dictionary<string, XmlNodeList> m_NodeListDic;

    public List<CharacterAction> actionList;
    public Dictionary<string, State> stateDic;
    public List<Topic> topicList;

    public List<CharacterAction>[] characterActionArray = new List<CharacterAction>[3];
    private List<CharacterAction> m_StudyActions = new List<CharacterAction>();
    private List<CharacterAction> m_AmusementActions = new List<CharacterAction>();
    private List<CharacterAction> m_LaborActions = new List<CharacterAction>();

    private string m_Type;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_NodeListDic = new Dictionary<string, XmlNodeList>();
        stateDic = new Dictionary<string, State>();
        actionList = new List<CharacterAction>();
        topicList = new List<Topic>();
        StartCoroutine(Init());
    }

    /// <summary>
    /// 待全部xml加载完进行初始化
    /// </summary>
    private IEnumerator Init()
    {
        // 加载行动
        yield return StartCoroutine(LoadXml("Actions", "/Xml/Action.xml"));
        // 加载状态
        yield return StartCoroutine(LoadXml("States", "/Xml/State.xml"));
        // 加载主题
        yield return StartCoroutine(LoadXml("Topics", "/Xml/Topic.xml"));

        foreach (XmlElement action in m_NodeListDic.GetValue("Actions"))
        {
            LoadAction(action, actionList);
        }
        foreach (XmlElement state in m_NodeListDic.GetValue("States"))
        {
            LoadState(state, stateDic);
        }
        foreach (XmlElement topic in m_NodeListDic.GetValue("Topics"))
        {
            LoadTopic(topic, topicList);
        }
    }

    /// <summary>
    /// 加载XML的类
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadXml(string nodeName, string path)
    {
#pragma warning disable 618
        WWW www = new WWW(Application.streamingAssetsPath + path);
#pragma warning restore 618
        yield return www;

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(www.text);
        XmlNodeList xmlNodeList = xml.SelectSingleNode(nodeName)?.ChildNodes;
        m_NodeListDic.Add(nodeName, xmlNodeList);
    }

    /// <summary>
    /// 从action对应的xml文件中加载action，并添加到actions中
    /// </summary>
    /// <param name="action"></param>
    /// <param name="actions"></param>
    private void LoadAction(XmlElement action, List<CharacterAction> actions)
    {
        CharacterAction ac = new CharacterAction();

        m_Type = action.GetAttribute("id").Split('_')[1];
        switch (m_Type)
        {
            case "Study":
                ac.Type = ActionType.Study;
                m_StudyActions.Add(ac);
                ac.SelfControlIncrement = 1;
                ac.ConsumeBonus = 0;
                break;

            case "Amusement":
                ac.Type = ActionType.Amusement;
                m_AmusementActions.Add(ac);
                ac.SelfControlIncrement = -2;
                ac.ConsumeBonus = 0;
                break;

            case "Labor":
                ac.Type = ActionType.Labor;
                m_LaborActions.Add(ac);
                ac.SelfControlIncrement = 2;
                ac.ConsumeBonus = -1;
                break;
        }
        ac.Name = action.ChildNodes[0].InnerText;
        ac.Consume = int.Parse(action.ChildNodes[1].InnerText);
        PropertyStruct PS1 = new PropertyStruct(Parse(action.ChildNodes[2].InnerText),
            Parse(action.ChildNodes[4].InnerText), 
            Parse(action.ChildNodes[3].InnerText),
            Parse(action.ChildNodes[5].InnerText));
        ac.Property = PS1;
        ac.Money = int.Parse(action.ChildNodes[6].InnerText);
        ac.SelfControl = int.Parse(action.ChildNodes[8].InnerText);
        ac.SuccessRate = float.Parse(action.ChildNodes[9].InnerText);
        PropertyStruct PS2 = new PropertyStruct(Parse(action.ChildNodes[10].InnerText),
            Parse(action.ChildNodes[12].InnerText), 
            Parse(action.ChildNodes[11].InnerText),
            Parse(action.ChildNodes[13].InnerText));
        ac.NeedProperty = PS2;

        ac.Captions = new string[3];
        ac.Captions[0] = action.ChildNodes[14].InnerText;
        ac.Captions[1] = action.ChildNodes[15].InnerText;
        ac.Captions[2] = action.ChildNodes[16].InnerText;
        ac.EventCaption = action.ChildNodes[17].InnerText;
        ac.Option = action.ChildNodes[18].InnerText;
        ac.End = action.ChildNodes[19].InnerText;
        ac.NeedMaxRound = Parse(action.ChildNodes[20].InnerText);
        ac.NeedMinRound = Parse(action.ChildNodes[21].InnerText);
        // TODO:不知是否会与存档冲突
        ac.Count = new[] { 0, 0, 0, 0 };

        actions.Add(ac);
        characterActionArray[0] = m_StudyActions;
        characterActionArray[1] = m_AmusementActions;
        characterActionArray[2] = m_LaborActions;
    }

    /// <summary>
    /// 从state对应的xml文件中加载state，并添加到states中
    /// </summary>
    /// <param name="state"></param>
    /// <param name="states"></param>
    /// <param name="dic"></param>
    private void LoadState(XmlElement state, IDictionary<string, State> dic)
    {
        State st = new State();
        m_Type = state.GetAttribute("id").Split('_')[1];
        switch (m_Type)
        {
            case "Invitation":
                st.StType = StateType.Invitation;
                st.IsTemp = false;
                break;

            case "Temporarily":
                st.StType = StateType.Temporarily;
                st.IsTemp = true;
                break;

            case "Identity":
                st.StType = StateType.Identity;
                st.IsTemp = false;
                break;
        }
        st.Name = state.ChildNodes[0].InnerText;
        st.Logic = Parse(state.ChildNodes[1].InnerText);
        st.Talk = Parse(state.ChildNodes[2].InnerText);
        st.Athletics = Parse(state.ChildNodes[3].InnerText);
        st.Creativity = Parse(state.ChildNodes[4].InnerText);
        st.Duration = Parse(state.ChildNodes[5].InnerText);
        st.OtherEffect = state.ChildNodes[6].InnerText;
        dic.Add(st.Name, st);
    }

    private void LoadTopic(XmlElement topic, List<Topic> topics)
    {
        Topic tp = new Topic
        {
            TopicName = topic.ChildNodes[0].InnerText
        };
        XmlNodeList xmlNodeList = topic.SelectNodes("Plot");
        if (xmlNodeList != null)
            for (var i = 0; i < xmlNodeList.Count; i++)
            {
                var plot = new Plot
                {
                    NeedLogic = Parse(xmlNodeList[i].ChildNodes[0].InnerText),
                    NeedTalk = Parse(xmlNodeList[i].ChildNodes[1].InnerText),
                    NeedAthletics = Parse(xmlNodeList[i].ChildNodes[2].InnerText),
                    NeedCreativity = Parse(xmlNodeList[i].ChildNodes[3].InnerText),
                    NeedMaxRound = Parse(xmlNodeList[i].ChildNodes[6].InnerText),
                    NeedMinRound = Parse(xmlNodeList[i].ChildNodes[7].InnerText),
                    Place = xmlNodeList[i].ChildNodes[8].InnerText,
                    NeedRelationShip = Parse(xmlNodeList[i].ChildNodes[9].InnerText)
                };
                plot.AddState(xmlNodeList[i].ChildNodes[4].InnerText);
                plot.AddSlot(xmlNodeList[i].ChildNodes[5].InnerText);
                tp.Plots.Add(plot);
            }

        topics.Add(tp);
    }

    private int Parse(string str)
    {
        return str.Equals("") ? 0 : int.Parse(str);
    }
}