using DSD.KernalTool;
using UnityEngine;
using UnityEngine.UI;

public class SwitchRound : MonoBehaviour
{
    // 行动名字数组
    public string[] actionNames;

    public Text actionNameText;
    private Animator animator;
    private SelectedActionButton[] ActionListBtns;
    private Color[] colors;
    private int i;

    private void Start()
    {
        InitColors();
    }

    private void InitColors()
    {
        colors = new Color[3];
        Color Yellow = new Color(255 / 255f, 255 / 255f, 147 / 255f);
        Color Blue = new Color(146 / 255f, 247 / 255f, 251 / 255f);
        Color Pink = new Color(255 / 255f, 147 / 255f, 152 / 255f);
        colors[0] = Yellow;
        colors[1] = Blue;
        colors[2] = Pink;
    }

    public void OnSwitchAction()
    {
        // 当动画播放完毕，若actionNames的索引尚未到底，则继续动画，改变颜色

        if ((++i) < actionNames.Length && actionNames[i] != null)
        {
            SwitchColor();
            actionNameText.text = "[" + actionNames[i] + "]中";
        }
        else
        {
            animator.SetBool("SwitchRound", false);
            StartAction();
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置行动名字
    /// </summary>
    /// <param name="names"></param>
    public void SetActionNames(SelectedActionButton[] selectedActions)
    {
        int index = 0;
        // 初始化为null
        actionNames = new string[5];
        if (selectedActions.Length > 0)
        {
            while (index < selectedActions.Length && selectedActions[index] != null)
            {
                if (selectedActions[index].action != null)
                    actionNames[index] = selectedActions[index].action.Name;
                else
                    actionNames[index] = null;
                index++;
            }
        }
        ActionListBtns = selectedActions;
        i = 0;
        animator = GetComponent<Animator>();
        InitColors();
        SwitchColor();
        animator.SetBool("SwitchRound", true);
        actionNameText.text = "[" + actionNames[i] + "]中";
    }

    private void SwitchColor()
    {
        int[] randomSequence = Widget.GetRandomSequence(3, 3);
        transform.Find("Core").gameObject.GetComponent<Image>().color = colors[randomSequence[0]];
        transform.Find("Inner").gameObject.GetComponent<Image>().color = colors[randomSequence[1]];
        transform.Find("Outer").gameObject.GetComponent<Image>().color = colors[randomSequence[2]];
        actionNameText.color = colors[randomSequence[0]];
    }

    private void StartAction()
    {
        // 根据Action增加相应属性
        foreach (SelectedActionButton s in ActionListBtns)
        {
            if (s.action != null)
                s.AddProperty();
        }
    }
}