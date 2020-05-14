using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/// <summary>
/// 加载ActionXML，生成actionList，供外部panel使用
/// </summary>
public class XMLManager : MonoBehaviour
{
    public static XMLManager Instance { get; private set; }

    private Dictionary<string, XmlNodeList> NodeListDic;

    public List<CharacterAction> actionList;
    public Dictionary<string, State> stateDic;
    public List<Topic> topicList;

    public List<CharacterAction>[] characterActionArray = new List<CharacterAction>[3];
    private List<CharacterAction> studyActions = new List<CharacterAction>();
    private List<CharacterAction> amusementActions = new List<CharacterAction>();
    private List<CharacterAction> laborActions = new List<CharacterAction>();

    private string type;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        NodeListDic = new Dictionary<string, XmlNodeList>();
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

        foreach (XmlElement action in NodeListDic.GetValue("Actions"))
        {
            LoadAction(action, actionList);
        }
        foreach (XmlElement state in NodeListDic.GetValue("States"))
        {
            LoadState(state, stateDic);
        }
        foreach (XmlElement topic in NodeListDic.GetValue("Topics"))
        {
            LoadTopic(topic, topicList);
        }
    }

    /// <summary>
    /// 加载XML的类
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadXml(string NodeName, string path)
    {
        WWW www = new WWW(Application.streamingAssetsPath + path);
        yield return www;

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(www.text);
        XmlNodeList xmlNodeList = xml.SelectSingleNode(NodeName).ChildNodes;
        NodeListDic.Add(NodeName, xmlNodeList);
    }

    /// <summary>
    /// 从action对应的xml文件中加载action，并添加到actions中
    /// </summary>
    /// <param name="action"></param>
    /// <param name="actions"></param>
    private List<CharacterAction>[] LoadAction(XmlElement action, List<CharacterAction> actions)
    {
        CharacterAction ac = new CharacterAction();

        type = action.GetAttribute("id").Split('_')[1];
        switch (type)
        {
            case "Study":
                ac.Type = ActionType.Study;
                studyActions.Add(ac);
                break;

            case "Amusement":
                ac.Type = ActionType.Amusement;
                amusementActions.Add(ac);
                break;

            case "Labor":
                ac.Type = ActionType.Labor;
                laborActions.Add(ac);
                break;
        }
        ac.Name = action.ChildNodes[0].InnerText;
        ac.Consume = int.Parse(action.ChildNodes[1].InnerText);
        ac.Logic = int.Parse(action.ChildNodes[2].InnerText);
        ac.Talk = int.Parse(action.ChildNodes[3].InnerText);
        ac.Athletics = int.Parse(action.ChildNodes[4].InnerText);
        ac.Creativity = int.Parse(action.ChildNodes[5].InnerText);
        ac.Money = int.Parse(action.ChildNodes[6].InnerText);
        ac.Caption = action.ChildNodes[7].InnerText;
        ac.SelfControl = int.Parse(action.ChildNodes[8].InnerText);
        ac.SuccessRate = float.Parse(action.ChildNodes[9].InnerText);
        ac.NeedLogic = parse(action.ChildNodes[10].InnerText);
        ac.NeedTalk = parse(action.ChildNodes[11].InnerText);
        ac.NeedAthletics = parse(action.ChildNodes[12].InnerText);
        ac.NeedCreativity = parse(action.ChildNodes[13].InnerText);

        ac.Captions = new string[3];
        ac.Captions[0] = action.ChildNodes[14].InnerText;
        ac.Captions[1] = action.ChildNodes[15].InnerText;
        ac.Captions[2] = action.ChildNodes[16].InnerText;
        actions.Add(ac);
        characterActionArray[0] = studyActions;
        characterActionArray[1] = amusementActions;
        characterActionArray[2] = laborActions;
        return characterActionArray;
    }

    /// <summary>
    /// 从state对应的xml文件中加载state，并添加到states中
    /// </summary>
    /// <param name="state"></param>
    /// <param name="states"></param>
    private void LoadState(XmlElement state, Dictionary<string, State> Dic)
    {
        State st = new State();
        type = state.GetAttribute("id").Split('_')[1];
        switch (type)
        {
            case "Invitation":
                st.Type = StateType.Invitation;
                st.IsTemp = false;
                break;

            case "Temporarily":
                st.Type = StateType.Temporarily;
                st.IsTemp = true;
                break;

            case "Identity":
                st.Type = StateType.Identity;
                st.IsTemp = false;
                break;
        }
        st.Name = state.ChildNodes[0].InnerText;
        st.Logic = parse(state.ChildNodes[1].InnerText);
        st.Talk = parse(state.ChildNodes[2].InnerText);
        st.Athletics = parse(state.ChildNodes[3].InnerText);
        st.Creativity = parse(state.ChildNodes[4].InnerText);
        st.Duration = parse(state.ChildNodes[5].InnerText);
        st.OtherEffect = state.ChildNodes[6].InnerText;
        Dic.Add(st.Name, st);
    }

    private void LoadTopic(XmlElement topic, List<Topic> topics)
    {
        Topic tp = new Topic();
        tp.TopicName = topic.ChildNodes[0].InnerText;
        XmlNodeList xmlNodeList = topic.SelectNodes("Plot");
        for (int i = 0; i < xmlNodeList.Count; i++)
        {
            Plot plot = new Plot();
            plot.needLogic = parse(xmlNodeList[i].ChildNodes[0].InnerText);
            plot.needTalk = parse(xmlNodeList[i].ChildNodes[1].InnerText);
            plot.needAthletics = parse(xmlNodeList[i].ChildNodes[2].InnerText);
            plot.needCreativity = parse(xmlNodeList[i].ChildNodes[3].InnerText);
            plot.place = xmlNodeList[i].ChildNodes[6].InnerText;
            plot.AddState(xmlNodeList[i].ChildNodes[4].InnerText);
            plot.AddSlot(xmlNodeList[i].ChildNodes[5].InnerText);
            tp.plots.Add(plot);
        }
        topics.Add(tp);
    }

    private int parse(string str)
    {
        if (str.Equals(""))
        {
            return 0;
        }
        else
        {
            return int.Parse(str);
        }
    }
}