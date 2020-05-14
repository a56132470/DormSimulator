public interface IBasePanel
{
    /// <summary>
    /// 面板生成时调用，负责初始化
    /// </summary>
    void Init();

    /// <summary>
    /// 面板进入时调用
    /// </summary>
    void OnEnter();

    /// <summary>
    /// 面板停止时调用（鼠标与面板的交互停止）
    /// </summary>
    void OnPause();

    /// <summary>
    /// 面板恢复使用时调用(鼠标与面板的交互恢复)
    /// </summary>
    void OnResume();

    /// <summary>
    /// 面板退出时调用
    /// </summary>
    void OnExit();
}