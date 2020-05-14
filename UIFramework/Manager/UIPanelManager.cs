using LitJson;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelManager : MonoBehaviour
{
    public static UIPanelManager Instance;
    private Transform canvasTransform;

    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }

    private Dictionary<string, string> panelPathDict;
    private Dictionary<string, BasePanel> panelDict;
    private Stack<BasePanel> panelStack;

    private void Awake()
    {
        ParseUIPanelTypeJson();
        if (Instance == null)
        {
            Instance = this;
        }
        else
            return;
    }

    public void PushPanel(string panelType)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        // 停止上一个界面
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panelStack.Push(panel);
        panel.OnEnter();
    }

    public void PopPanel()
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        // 停止上一个界面
        if (panelStack.Count <= 0)
        {
            return;
        }
        // 退出栈顶面板
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();
        // 恢复上一个面板
        if (panelStack.Count > 0)
        {
            BasePanel panel = panelStack.Peek();
            panel.OnResume();
        }
    }

    public BasePanel GetPanel(string panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<string, BasePanel>();
        }
        BasePanel panel = panelDict.GetValue(panelType);
        // 如果没有实例化面板，寻找路径进行实例化，并且存储到已经实例化好的字典面板中
        if (panel == null)
        {
            string path = panelPathDict.GetValue(panelType);
            GameObject panelGo = Instantiate(Resources.Load<GameObject>(path), CanvasTransform, false);
            panel = panelGo.GetComponent<BasePanel>();
            panelDict.Add(panelType, panel);
            panel.Init();
        }
        return panel;
    }

    // 解析Json文件
    private void ParseUIPanelTypeJson()
    {
        panelPathDict = new Dictionary<string, string>();
        TextAsset textUIPanelType = Resources.Load<TextAsset>("UIPanelTypeJson");
        UIPanelInfoList panelInfoList = JsonMapper.ToObject<UIPanelInfoList>(textUIPanelType.text);

        foreach (UIPanelInfo panelInfo in panelInfoList.panelInfoList)
        {
            panelPathDict.Add(panelInfo.panelType, panelInfo.path);
        }
    }
}