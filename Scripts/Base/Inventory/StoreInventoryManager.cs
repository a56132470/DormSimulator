using DSD.KernalTool;
using UnityEngine;

public class StoreInventoryManager : MonoBehaviour
{
    public static StoreInventoryManager instance;

    /// <summary>
    /// 可售商品堆
    /// </summary>
    public Inventory itemInventory;

    public Inventory Store;
    public GameObject slotGrid;
    public Slot slotPrefab;

    // 保存进来的回合数，若当前回合数与之前进来的回合数不一样就刷新商店
    private int lastRound;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        lastRound = 0;
    }

    private void OnEnable()
    {
        if (lastRound != GlobalVariable.instance.player.CurRound)
        {
            RefreshItem();
            lastRound = GlobalVariable.instance.player.CurRound;
        }
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

        UpdateInventory();
    }

    /// <summary>
    /// 从商品堆里抽6个商品摆放
    /// </summary>
    public void UpdateInventory()
    {
        Store.itemList.Clear();

        int[] randomSequence = Widget.GetRandomSequence(itemInventory.itemList.Count, 6);
        // 从asset里挑选六个生成
        for (int i = 0; i < 6; i++)
        {
            Slot newItem = Instantiate(slotPrefab, slotGrid.transform);
            newItem.slotItem = itemInventory.itemList[randomSequence[i]];
            newItem.slotImage.sprite = itemInventory.itemList[randomSequence[i]].ItemImage;
            newItem.isSell = true;
            Store.itemList.Add(newItem.slotItem);
        }
    }
}