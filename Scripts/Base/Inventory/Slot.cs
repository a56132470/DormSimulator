using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    public Item slotItem;
    public Image slotImage;
    public Sprite RunOutSprite;
    public Text heldNum;
    public GameObject detailedInfoGam;
    public Text Price;

    private Text detailedInfoTitle;
    private Text detailedInfoCaption;
    private Button BuyOrUseBtn;
    private string[] CarryingStateStr;
    public bool isSell = false;

    private void Start()
    {
        detailedInfoTitle = detailedInfoGam.transform.Find("Title").GetComponent<Text>();
        detailedInfoCaption = detailedInfoGam.transform.Find("Caption").GetComponent<Text>();
        BuyOrUseBtn = detailedInfoGam.GetComponentInChildren<Button>();

        detailedInfoTitle.text = slotItem.ItemName;
        detailedInfoCaption.text = slotItem.ItemEffect;
        CarryingStateStr = new string[2];
        heldNum.text = slotItem.ItemHeld.ToString();
        if (Price != null)
        {
            Price.text = slotItem.ItemPrice + "元";
        }
        detailedInfoGam.SetActive(false);

        BuyOrUseBtn.onClick.AddListener(OnBuyOrUseButtonClick);
        TranslateFromStrToState(slotItem.ItemEffect);
    }

    /// <summary>
    /// 点击时触发的事件，商店与背包共用，点击则让物品的详细框出现
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (isSell && BuyOrUseBtn.gameObject.name.Equals("Buy"))
            detailedInfoGam.SetActive(true);

        if (BuyOrUseBtn.gameObject.name.Equals("Use"))
            detailedInfoGam.SetActive(true);
    }

    /// <summary>
    /// 指针移出时触发的事件，商店与背包共用，移出时将详细信息清除
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (isSell && BuyOrUseBtn.gameObject.name.Equals("Buy"))
            detailedInfoGam.SetActive(false);

        if (BuyOrUseBtn.gameObject.name.Equals("Use"))
            detailedInfoGam.SetActive(false);
    }

    /// <summary>
    /// 点击使用或购买按钮时触发事件，是一个分支判断，购买或使用
    ///<para>购买：若当前存在此物品，则让物品的持有数量+1，若不存在此物品则让人物持有该物品，并扣除相应零花钱</para>
    ///<para>使用：为用户增加当前状态，若当前状态存在则增加当前状态持续时间，若状态不存在则增加当前状态到面板上</para>
    /// </summary>
    private void OnBuyOrUseButtonClick()
    {
        if (BuyOrUseBtn.gameObject.name.Equals("Buy"))
        {
            // 买得起就扣除相应的钱款，并在bagInventory里增加item，买不起就弹出“你买不起”
            if (GlobalVariable.instance.player.Money >= slotItem.ItemPrice)
            {
                if (GlobalVariable.instance.MyBag.itemList.Contains(slotItem))
                    slotItem.ItemHeld += 1;
                else
                    GlobalVariable.instance.MyBag.itemList.Add(slotItem);
                isSell = false;
                if (RunOutSprite != null)
                    slotImage.sprite = RunOutSprite;
                detailedInfoGam.SetActive(false);
                GlobalVariable.instance.player.Money -= slotItem.ItemPrice;
            }
        }
        else if (BuyOrUseBtn.gameObject.name.Equals("Use"))
        {
            Use();
        }
    }

    /// <summary>
    /// 使用道具
    /// <para>若当前持有该状态，则让当前状态的剩余时间加长</para>
    /// <para>若当前未持有该状态，则增加当前持有的状态</para>
    /// <para></para>
    /// </summary>
    private void Use()
    {
        for (int i = 0; i < CarryingStateStr.Length && CarryingStateStr[i] != null; i++)
        {
            if (GlobalVariable.instance.player.stateDic.ContainsKey(CarryingStateStr[i]))
            {
                GlobalVariable.instance.player.stateDic.GetValue(CarryingStateStr[i]).RemainTime +=
                    GlobalVariable.instance.player.stateDic.GetValue(CarryingStateStr[i]).Duration;
            }
            else
            {
                GlobalVariable.instance.player.AddState(CarryingStateStr[i]);
            }
        }
        if (slotItem.ItemHeld > 1)
        {
            slotItem.ItemHeld -= 1;
        }
        else
        {
            GlobalVariable.instance.MyBag.itemList.Remove(slotItem);
            Destroy(this);
        }
        BagInventoryManager.instance.Refresh();
    }

    /// <summary>
    /// 把效果里的状态转成State类,并存进当前物品携带的类里
    /// </summary>
    /// <param name="effect"></param>
    private void TranslateFromStrToState(string effect)
    {
        // 将字符串按【】进行分割
        string[] splitEffect = effect.Split(new char[2] { '【', '】' });
        splitEffect = ToNoSpaceStr(splitEffect);

        for (int j = 0; j < 2; j++)
        {
            if (splitEffect[j + 1] != null)
            {
                if (XMLManager.Instance.stateDic.ContainsKey(splitEffect[j + 1]))
                {
                    CarryingStateStr[j] = splitEffect[j + 1];
                }
            }
        }
    }

    private string[] ToNoSpaceStr(string[] strs)
    {
        string[] newStrs = new string[3];
        int index = 0;
        for (int i = 0; i < strs.Length; i++)
        {
            if (!strs[i].Equals(""))
            {
                newStrs[index++] = strs[i];
            }
        }
        return newStrs;
    }
}