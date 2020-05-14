using System;

/// <summary>
/// 面板的信息类，包括面板的名称和面板Prefab的路径
/// <para>用于和json文件进行映射</para>
/// </summary>
[Serializable]
public class UIPanelInfo
{
    public string panelType;
    public string path;

    public UIPanelInfo()
    {
    }
}