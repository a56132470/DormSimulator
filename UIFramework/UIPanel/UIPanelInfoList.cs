using System;
using System.Collections.Generic;

/// <summary>
/// 面板信息集合类
/// <para>用于和json文件进行映射</para>
/// </summary>
[Serializable]
public class UIPanelInfoList
{
    public List<UIPanelInfo> panelInfoList;

    public UIPanelInfoList()
    {
    }
}