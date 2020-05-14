using UnityEngine;
using UnityEngine.EventSystems;

public class UniversalButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject border;

    // 鼠标移入开启边框
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (border != null)
            border.SetActive(true);
    }

    // 鼠标移出关闭边框
    public void OnPointerExit(PointerEventData eventData)
    {
        if (border != null)
            border.SetActive(false);
    }
}