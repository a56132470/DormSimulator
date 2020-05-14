using UnityEngine;

public class BasePanel : MonoBehaviour, IBasePanel
{
    protected CanvasGroup canvasGroup;
    protected GameObject Block;

    public virtual void Init()
    {
        if (GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup = GetComponent<CanvasGroup>();
        // 触碰区域
        if (transform.Find("Block") != null)
            Block = transform.Find("Block").gameObject;
        else
            Block = null;
    }

    public virtual void OnEnter()
    {
        gameObject.SetActive(true);
        if (Block != null)
            Block.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { UIPanelManager.Instance.PopPanel(); });
    }

    public virtual void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }

    public virtual void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void OnExit()
    {
        gameObject.SetActive(false);
        if (Block != null)
            Block.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
    }
}