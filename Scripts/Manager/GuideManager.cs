using UnityEngine;

public class GuideManager : MonoBehaviour
{
    // 指向箭头，指向各个位置，先做指向箭头，至于要不要限制玩家行动，个人认为不需要
    public void ArrowTo(GuideType guide)
    {
        switch (guide)
        {
            case GuideType.Portrait:
                break;

            case GuideType.Strength:
                break;

            case GuideType.Roommate:
                break;

            case GuideType.ActionPanel:
                UIPanelManager.Instance.PushPanel(UIPanelType.Action);
                break;

            case GuideType.ActionButton:
                break;

            case GuideType.ActionDetermineBtn:
                break;

            case GuideType.Block:

                break;

            case GuideType.NextRound:
                break;
        }
    }
}