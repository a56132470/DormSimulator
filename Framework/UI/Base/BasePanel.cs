using UnityEngine;

public class BasePanel : MonoBehaviour, IBasePanel
{
    protected CanvasGroup CanvasGroup;
    private GameObject _block;

    public virtual void Init()
    {
        if (GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }
        CanvasGroup = GetComponent<CanvasGroup>();
        // 触碰区域
        if (transform.Find("Block") != null)
            _block = transform.Find("Block").gameObject;
        else
            _block = null;
    }

    public virtual void OnEnter(object intent = null)
    {
        gameObject.SetActive(true);
        if (_block != null)
            _block.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { UIPanelManager.Instance.PopPanel(); });
        Init();
    }

    public virtual void OnPause()
    {
        CanvasGroup.blocksRaycasts = false;
    }

    public virtual void OnResume()
    {
        CanvasGroup.blocksRaycasts = true;
    }

    public virtual void OnExit()
    {
        gameObject.SetActive(false);
        if (_block != null)
            _block.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
    }
}