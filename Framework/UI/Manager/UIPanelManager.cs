using LitJson;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelManager : MonoBehaviour
{
    public static UIPanelManager Instance;
    private Transform m_CanvasTransform;

    private Transform CanvasTransform
    {
        get
        {
            if (m_CanvasTransform == null)
            {
                m_CanvasTransform = GameObject.Find("Canvas").transform;
            }
            return m_CanvasTransform;
        }
    }
    
    private Dictionary<string, string> m_PanelPathDict;
    private Dictionary<string, BasePanel> m_PanelDict;
    private Stack<BasePanel> m_PanelStack;

    private void Awake()
    {
        ParseUiPanelTypeJson();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// 将UIPanel推入栈
    /// </summary>
    /// <param name="panelType"></param>
    /// <param name="intent"></param>
    public void PushPanel(string panelType,object intent = null)
    {
        if (m_PanelStack == null)
        {
            m_PanelStack = new Stack<BasePanel>();
        }
        // 停止上一个界面
        if (m_PanelStack.Count > 0)
        {
            BasePanel topPanel = m_PanelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        m_PanelStack.Push(panel);
        panel.OnEnter(intent);
    }
    /// <summary>
    ///  弹出栈顶的UIPanel，并恢复弹出后栈顶的面板
    /// </summary>
    public void PopPanel()
    {
        if (m_PanelStack == null)
        {
            m_PanelStack = new Stack<BasePanel>();
        }
        // 停止上一个界面
        if (m_PanelStack.Count <= 0)
        {
            return;
        }
        // 退出栈顶面板
        BasePanel topPanel = m_PanelStack.Pop();
        topPanel.OnExit();
        // 恢复上一个面板
        if (m_PanelStack.Count > 0)
        {
            BasePanel panel = m_PanelStack.Peek();
            panel.OnResume();
        }
    }

    public BasePanel GetPanel(string panelType)
    {
        if (m_PanelDict == null)
        {
            m_PanelDict = new Dictionary<string, BasePanel>();
        }
        BasePanel panel = m_PanelDict.GetValue(panelType);
        // 如果没有实例化面板，寻找路径进行实例化，并且存储到已经实例化好的字典面板中
        if (panel == null)
        {
            string path = m_PanelPathDict.GetValue(panelType);
            
            GameObject panelGo = Instantiate(Resources.Load<GameObject>(path), CanvasTransform, false);
            panel = panelGo.GetComponent<BasePanel>();
            m_PanelDict.Add(panelType, panel);
            panel.Init();
        }
        return panel;
    }

    // 解析Json文件
    private void ParseUiPanelTypeJson()
    {
        m_PanelPathDict = new Dictionary<string, string>();
        TextAsset textUiPanelType = Resources.Load<TextAsset>("UIPanelTypeJson");
        UIPanelInfoList panelInfoList = JsonMapper.ToObject<UIPanelInfoList>(textUiPanelType.text);

        foreach (UIPanelInfo panelInfo in panelInfoList.panelInfoList)
        {
            m_PanelPathDict.Add(panelInfo.panelType, panelInfo.path);
        }
    }
}