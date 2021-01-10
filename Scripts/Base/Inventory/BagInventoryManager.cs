using UnityEngine;

namespace Base.Inventory
{
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

            for (int i = 0; i < GlobalManager.Instance.myBag.itemList.Count; i++)
            {
                CreateNewItem(GlobalManager.Instance.myBag.itemList[i]);
            }
        }

        public void CreateNewItem(Item item)
        {
            Slot newItem = Instantiate(slotPrefab, slotGrid.transform);
            newItem.slotItem = item;
            newItem.slotImage.sprite = item.itemImage;
            newItem.heldNum.text = item.itemHeld.ToString();
        }

        public void AddNewItem(Item item)
        {
            if (!GlobalManager.Instance.myBag.itemList.Contains(item))
            {
                GlobalManager.Instance.myBag.itemList.Add(item);
                CreateNewItem(item);
            }
            else
            {
                item.itemHeld += 1;
            }
        }
    }
}