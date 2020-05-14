using UnityEngine;

/// <summary>
/// 背包管理类，与商店管理类区分开
/// </summary>
public class BagInventoryManager : MonoBehaviour
{
    public static BagInventoryManager instance;

    public GameObject slotGrid;
    public Slot slotPrefab;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    private void OnEnable()
    {
        RefreshItem();
    }

    public void Refresh()
    {
        RefreshItem();
    }

    /// <summary>
    /// 初始化背包，清除当前背包，从asset加载背包
    /// </summary>
    private void RefreshItem()
    {
        // 清除背包
        for (int i = 0; i < slotGrid.transform.childCount; i++)
        {
            Destroy(slotGrid.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < GlobalVariable.instance.MyBag.itemList.Count; i++)
        {
            CreateNewItem(GlobalVariable.instance.MyBag.itemList[i]);
        }
    }

    public void CreateNewItem(Item item)
    {
        Slot newItem = Instantiate(slotPrefab, slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.ItemImage;
        newItem.heldNum.text = item.ItemHeld.ToString();
    }

    public void AddNewItem(Item item)
    {
        if (!GlobalVariable.instance.MyBag.itemList.Contains(item))
        {
            GlobalVariable.instance.MyBag.itemList.Add(item);
            CreateNewItem(item);
        }
        else
        {
            item.ItemHeld += 1;
        }
    }
}