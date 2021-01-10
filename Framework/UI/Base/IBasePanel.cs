using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
public interface IBasePanel
{
    /// <summary>
    /// 面板生成时调用，负责初始化
    /// </summary>
    [UsedImplicitly]
    void Init();

    /// <summary>
    /// 面板进入时调用
    /// </summary>
    [UsedImplicitly]
    void OnEnter(object intent = null);

    /// <summary>
    /// 面板停止时调用（鼠标与面板的交互停止）
    /// </summary>
    [UsedImplicitly]
    void OnPause();

    /// <summary>
    /// 面板恢复使用时调用(鼠标与面板的交互恢复)
    /// </summary>
    [UsedImplicitly]
    void OnResume();

    /// <summary>
    /// 面板退出时调用
    /// </summary>
    [UsedImplicitly]
    void OnExit();
}