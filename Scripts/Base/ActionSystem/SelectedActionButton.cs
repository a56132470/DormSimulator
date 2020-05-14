using DSD.KernalTool;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 已选择行动的按钮：当在ActionPanel选择行动，则行动出现在此面板中
/// 功能：选择按钮则初始化按钮，并恢复一点体力
/// </summary>
public class SelectedActionButton : MonoBehaviour, IPointerClickHandler
{
    private GameObject ActionName;
    public CharacterAction action;
    public Text caption;
    public Text consume;

    private void Start()
    {
        ActionName = transform.Find("ActionName").gameObject;
        ActionName.GetComponent<Text>().text = "";
        action = null;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        // 恢复体力
        if (action != null)
            GlobalVariable.instance.player.Strength += action.Consume;
        // 设置左上角头像的体力条
        // 将ActionName设为空
        ActionName.GetComponent<Text>().text = "";
        // 将Action设为空
        action = null;
        consume.text = "";
        caption.text = "";
    }

    public void SetActionName()
    {
        ActionName.GetComponent<Text>().text = action.Name;
    }

    /// <summary>
    /// 根据当前选择的action为player加属性
    /// </summary>
    public void AddProperty()
    {
        float multiple;
        int promote_Athletics, promote_Creativity, promote_Logic, promote_Money, promoty_talk;
        promote_Athletics = action.Athletics + GlobalVariable.instance.player.AthleticsBonus;
        promote_Creativity = action.Creativity + GlobalVariable.instance.player.CreativityBonus; ;
        promote_Logic = action.Logic + GlobalVariable.instance.player.LogicBonus;
        promote_Money = action.Money;
        promoty_talk = action.Talk + GlobalVariable.instance.player.TalkBonus;
        if (Widget.JudgingFirstSuccess(action, GlobalVariable.instance.player))
        {
            multiple = 1;
            GlobalVariable.instance.player.AddRecordAction(action.Captions[1]);
            if (Widget.JudgingSecondSuccess(action, GlobalVariable.instance.player))
            {
                multiple = 2;
                GlobalVariable.instance.player.AddRecordAction(action.Captions[2]);
            }
        }
        else
        {
            multiple = 0.5f;
            GlobalVariable.instance.player.AddRecordAction(action.Captions[0]);
        }

        GlobalVariable.instance.player.Athletics += (int)Widget.ChinaRound(promote_Athletics * multiple, 0);
        GlobalVariable.instance.player.Creativity += (int)Widget.ChinaRound(promote_Creativity * multiple, 0);
        GlobalVariable.instance.player.Logic += (int)Widget.ChinaRound(promote_Logic * multiple, 0);
        GlobalVariable.instance.player.Money += (int)Widget.ChinaRound(promote_Money * multiple, 0);
        GlobalVariable.instance.player.Talk += (int)Widget.ChinaRound(promoty_talk * multiple, 0);

        action = null;
        ActionName.GetComponent<Text>().text = "";
        consume.text = "";
        caption.text = "";
    }
}